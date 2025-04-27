using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project_library.Core;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Library;
using Microsoft.EntityFrameworkCore;


namespace project_library.MVVM.ViewModel
{
    public class MyBooksViewModel : ObservableObject
    {
        private readonly MainViewModel _mainViewModel;
        private ObservableCollection<Books> _kivettLista;

        public ObservableCollection<Books> kivett_lista
        {
            get { return _kivettLista; }
            set
            {
                _kivettLista = value;
                OnPropertyChanged();
            }
        }

        public ICommand ReturnBookCommand { get; }

        public MyBooksViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            kivett_lista = new ObservableCollection<Books>();
            ReturnBookCommand = new RelayCommand(async book => await ReturnBook(book as Books));
            LoadTakenOutBooks();
        }

        private async void LoadTakenOutBooks()
        {
            try
            {
                using (var dbContext = new LibraryDbContext())
                {
                    // Fetch books taken out by the current user
                    var userId = _mainViewModel.CurrentUser.member_id;
                    if (userId == null)
                    {
                        MessageBox.Show("No user is logged in.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var books = await dbContext.Transactions
                        .Where(t => t.member_id == userId && t.return_date == null)
                        .Select(t => t.Books2)
                        .ToListAsync();
                    if (books.Count == 0)
                    {
                        MessageBox.Show("No books found for the logged-in user.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    kivett_lista = new ObservableCollection<Books>(books);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading books: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ReturnBook(Books book)
        {
            try
            {
                using (var dbContext = new LibraryDbContext())
                {
                    // Find the transaction for the book
                    var transaction = await dbContext.Transactions
                        .FirstOrDefaultAsync(t => t.book_id == book.book_id && t.member_id == _mainViewModel.CurrentUser.member_id && t.return_date == null);

                    if (transaction == null)
                    {
                        MessageBox.Show("Transaction not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Update the transaction's return_date
                    transaction.return_date = DateTime.Now;

                    // Update the book's availability
                    var bookToUpdate = await dbContext.Books.FirstOrDefaultAsync(b => b.book_id == book.book_id);
                    if (bookToUpdate != null)
                    {
                        bookToUpdate.availability = true;
                    }

                    // Save changes to the database
                    await dbContext.SaveChangesAsync();

                    // Remove the book from the list
                    kivett_lista.Remove(book);

                    MessageBox.Show("Book successfully returned!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while returning the book: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
