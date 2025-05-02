using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Library;
using project_library.Core;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace project_library.MVVM.ViewModel
{
    public class AdminViewModel : ObservableObject
    {
        private string _bookId;
        private ObservableCollection<string> _eventLogs;
        public ObservableCollection<string> EventLogs
        {
            get => _eventLogs;
            set
            {
                _eventLogs = value;
                OnPropertyChanged();
            }
        }
        public string BookId
        {
            get => _bookId;
            set
            {
                _bookId = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteBookCommand { get; }

        public AdminViewModel()
        {
            DeleteBookCommand = new RelayCommand(DeleteBook);
            LoadEventLogs();
        }

        private void DeleteBook(object parameter)
        {
            if (int.TryParse(BookId, out int bookId))
            {
                using (var dbContext = new LibraryDbContext())
                {
                    var book = dbContext.Books.Find(bookId);
                    if (book != null)
                    {
                        book.deleted = true;
                        dbContext.SaveChanges();
                        MessageBox.Show($"Book with ID {bookId} has been marked as deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"No book found with ID {bookId}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid numeric Book ID.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadEventLogs()
        {
            EventLogs = new ObservableCollection<string>();

            if (EventLog.SourceExists("LibraryApp"))
            {
                var eventLog = new EventLog("Application", ".", "LibraryApp");
                foreach (EventLogEntry entry in eventLog.Entries)
                {
                    if (entry.Source == "LibraryApp")
                    {
                        EventLogs.Add($"{entry.TimeGenerated}: {entry.Message}");
                    }
                }
            }
        }
    }
}