using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Library;
using Microsoft.EntityFrameworkCore;
using project_library.Core;

namespace project_library.MVVM.ViewModel
{
    public class DiscoverViewModel : ObservableObject, IReloadable
    {
        private readonly MainViewModel _mainViewModel;
        private ObservableCollection<Books> _uniqueBooks;
        private Books _selectedBook;

        public ObservableCollection<Books> UniqueBooks
        {
            get { return _uniqueBooks; }
            set
            {
                _uniqueBooks = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectBookCommand { get; }

        public DiscoverViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            SelectBookCommand = new RelayCommand(ExecuteSelectBookCommand);
            LoadUniqueBooks();
        }

        private async void LoadUniqueBooks()
        {
            using (var dbContext = new LibraryDbContext())
            {
                // Fetch unique books based on title and summary
                var books = await dbContext.Books
                    .Where(b => !b.deleted && b.availability)
                    .GroupBy(b => new { b.title, b.summary })
                    .Select(g => g.First())
                    .ToListAsync();

                UniqueBooks = new ObservableCollection<Books>(books);
            }
        }

        private void ExecuteSelectBookCommand(object parameter)
        {
            if (parameter is Books selectedBook)
            {
                // Navigate to the BookDetailPage with the selected book
                var bookDetailViewModel = new BookDetailViewModel(selectedBook, _mainViewModel, this);
                _mainViewModel.CurrentView = bookDetailViewModel;
            }
        }
        public void Reload()
        {
            // Reload the unique books
            LoadUniqueBooks();
        }
    }
}
