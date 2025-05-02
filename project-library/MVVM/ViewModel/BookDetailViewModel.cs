using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Library;
using Microsoft.EntityFrameworkCore;
using project_library.Core;
using System.Windows.Input;
using System.Windows;
using project_library.Utilities;
using System.Diagnostics;

namespace project_library.MVVM.ViewModel
{
    public class BookDetailViewModel : ObservableObject
    {
        private Books _selectedBook;
        private ObservableCollection<Authors> _authors;
        private readonly MainViewModel _mainViewModel;
        private readonly ObservableObject _previousViewModel;
        private int _availableCopies;

        public Books SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Authors> Authors
        {
            get { return _authors; }
            set
            {
                _authors = value;
                OnPropertyChanged();
            }
        }

        public int AvailableCopies
        {
            get { return _availableCopies; }
            set
            {
                _availableCopies = value;
                OnPropertyChanged();
            }
        }
        public string FormattedAuthors
        {
            get
            {
                if (Authors == null || !Authors.Any())
                    return "Unknown Author";

                // Join author names with a separator (e.g., ", ")
                return string.Join(", ", Authors.Select(a => a.name));
            }
        }

        public ICommand GoBackCommand { get; }
        public ICommand ChangeAmountCommand { get; }
        public ICommand TakeBookOutCommand { get; }

        public BookDetailViewModel(Books selectedBook, MainViewModel mainViewModel, ObservableObject previousViewModel)
        {
            SelectedBook = selectedBook;
            _mainViewModel = mainViewModel;
            _previousViewModel = previousViewModel;
            GoBackCommand = new RelayCommand(o =>
            {
                _mainViewModel.CurrentView = _previousViewModel;

                // Trigger reload if the previous view model implements IReloadable
                if (_previousViewModel is IReloadable reloadableView)
                {
                    reloadableView.Reload();
                }
            });
            ChangeAmountCommand = new RelayCommand(o => OpenChangeAmountWindow());
            TakeBookOutCommand = new RelayCommand(async o => await TakeBookOut());
            LoadAuthors();
            LoadAvailableCopies();
        }

        private async void LoadAuthors()
        {
            using (var dbContext = new LibraryDbContext())
            {
                var authors = await dbContext.AuthorBooks
                    .Where(ab => ab.book_id == SelectedBook.book_id)
                    .Select(ab => ab.Authors1)
                    .ToListAsync();

                Authors = new ObservableCollection<Authors>(authors);
                OnPropertyChanged(nameof(FormattedAuthors));
            }
        }
        private async void LoadAvailableCopies()
        {
            using (var dbContext = new LibraryDbContext())
            {
                AvailableCopies = await dbContext.Books
                    .Where(b => b.title == SelectedBook.title &&
                                b.summary == SelectedBook.summary &&
                                b.availability == true &&
                                b.deleted == false)
                    .CountAsync();
            }
        }
        private void OpenChangeAmountWindow()
        {
            // Pass the selected book to the ChangeAmmountWindow
            var changeAmountWindow = new ChangeAmmountWindow(SelectedBook);

            // Attach a Closed event handler to refresh AvailableCopies
            changeAmountWindow.Closed += (s, e) =>
            {
                RefreshAvailableCopies();

                EventLogger.LogEvent(
                $"Book ID {SelectedBook.book_id} ({SelectedBook.title}) was copied. Current available copies: {AvailableCopies}.",
                    EventLogEntryType.Information);
            };

            changeAmountWindow.ShowDialog();
        }


        public async void RefreshAvailableCopies()
        {
            using (var dbContext = new LibraryDbContext())
            {
                AvailableCopies = await dbContext.Books
                    .Where(b => b.title == SelectedBook.title &&
                                b.summary == SelectedBook.summary &&
                                b.availability == true &&
                                b.deleted == false)
                    .CountAsync();

                OnPropertyChanged(nameof(AvailableCopies));
            }
        }

        private async Task TakeBookOut()
        {
            try
            {
                using (var dbContext = new LibraryDbContext())
                {
                    // Find a book with availability = true and deleted = false
                    var bookToTakeOut = await dbContext.Books
                        .FirstOrDefaultAsync(b => b.title == SelectedBook.title &&
                                                  b.summary == SelectedBook.summary &&
                                                  b.availability == true &&
                                                  b.deleted == false);

                    if (bookToTakeOut == null)
                    {
                        MessageBox.Show("No available copies of this book.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Get the logged-in user's member_id
                    var loggedInUser = _mainViewModel.CurrentUser;
                    if (loggedInUser == null)
                    {
                        MessageBox.Show("User not found. Please log in again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Create a new transaction
                    var transaction = new Transactions
                    {
                        book_id = bookToTakeOut.book_id,
                        member_id = loggedInUser.member_id,
                        borrow_date = DateTime.Now // Set the borrow date to the current date
                    };

                    dbContext.Transactions.Add(transaction);

                    // Update the book's availability
                    bookToTakeOut.availability = false;

                    // Save changes to the database
                    await dbContext.SaveChangesAsync();

                    EventLogger.LogEvent($"User {loggedInUser.username} has taken out book ID {bookToTakeOut.book_id} ({bookToTakeOut.title}) on {DateTime.Now}.", EventLogEntryType.Information);

                    MessageBox.Show("Book successfully taken out!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

}
