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

namespace project_library.MVVM.ViewModel
{
    public class BookDetailViewModel : ObservableObject
    {
        private Books _selectedBook;
        private ObservableCollection<Authors> _authors;
        private readonly MainViewModel _mainViewModel;
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

        public ICommand GoBackCommand { get; }
        public ICommand ChangeAmountCommand { get; }

        public BookDetailViewModel(Books selectedBook, MainViewModel mainViewModel)
        {
            SelectedBook = selectedBook;
            _mainViewModel = mainViewModel;
            GoBackCommand = new RelayCommand(o => _mainViewModel.CurrentView = new HomeViewModel(_mainViewModel));
            ChangeAmountCommand = new RelayCommand(o => OpenChangeAmountWindow());
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
            changeAmountWindow.ShowDialog();
        }
    }

}
