using System.Linq;
using System.Threading.Tasks;
using Library;
using Microsoft.EntityFrameworkCore;
using project_library.Core;

namespace project_library.MVVM.ViewModel
{
    public class ChangeAmmountWindowViewModel : ObservableObject
    {
        private int _currentAmount;
        private readonly Books _selectedBook;

        public int CurrentAmount
        {
            get { return _currentAmount; }
            set
            {
                _currentAmount = value;
                OnPropertyChanged();
            }
        }

        public ChangeAmmountWindowViewModel(Books selectedBook)
        {
            _selectedBook = selectedBook;
            LoadCurrentAmount();
        }

        public async void LoadCurrentAmount()
        {
            using (var dbContext = new LibraryDbContext())
            {
                // Query the database to count the number of books matching the criteria
                CurrentAmount = await dbContext.Books
                    .Where(b => b.title == _selectedBook.title &&
                                b.summary == _selectedBook.summary &&
                                b.deleted == false) // Deleted is set to false
                    .CountAsync();
            }
        }
    }
}

