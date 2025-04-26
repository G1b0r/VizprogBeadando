using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Microsoft.EntityFrameworkCore;
using project_library.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace project_library.MVVM.ViewModel
{
    public class HomeViewModel : ObservableObject
    {
        private ObservableCollection<Books> _topBooks;
        private readonly MainViewModel _mainViewModel;
        public double ButtonWidth { get; set; } = 200;
        public double ButtonHeight { get; set; } = 250;


        public ObservableCollection<Books> TopBooks
        {
            get { return _topBooks; }
            set
            {
                _topBooks = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectBookCommand { get; }

        public HomeViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            SelectBookCommand = new RelayCommand(ExecuteSelectBookCommand);
            LoadTopBooks();
        }

        private async void LoadTopBooks()
        {
            using (var dbContext = new LibraryDbContext())
            {
                // Fetch the 4 books with the smallest IDs
                var books = await dbContext.Books
                    .OrderBy(b => b.book_id)
                    .Take(4)
                    .ToListAsync();

                // Populate the ObservableCollection
                TopBooks = new ObservableCollection<Books>(books);
            }
        }

        private void ExecuteSelectBookCommand(object parameter)
        {
            if (parameter is Books selectedBook)
            {
                var bookDetailViewModel = new BookDetailViewModel(selectedBook, _mainViewModel);
                _mainViewModel.CurrentView = bookDetailViewModel;
            }
        }
    }
}