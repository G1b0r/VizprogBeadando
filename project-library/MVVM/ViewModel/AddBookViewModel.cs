using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Library;
using project_library.Core;
using project_library.Utilities;

namespace project_library.MVVM.ViewModel
{
    public class AddBookViewModel : ObservableObject
    {
        private string _title;
        private string _category;
        private string _releaseDate;
        private string _publisher;
        private string _description;
        private string _coverPicLink;
        private ObservableCollection<AuthorViewModel> _authors;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        public string ReleaseDate
        {
            get => _releaseDate;
            set
            {
                _releaseDate = value;
                OnPropertyChanged();
            }
        }

        public string Publisher
        {
            get => _publisher;
            set
            {
                _publisher = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public string CoverPicLink
        {
            get => _coverPicLink;
            set
            {
                _coverPicLink = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AuthorViewModel> Authors
        {
            get => _authors;
            set
            {
                _authors = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddAuthorCommand { get; }
        public ICommand SaveBookCommand { get; }

        public AddBookViewModel()
        {
            Authors = new ObservableCollection<AuthorViewModel>();
            AddAuthorCommand = new RelayCommand(AddAuthor);
            SaveBookCommand = new RelayCommand(SaveBook);
        }

        private void AddAuthor(object obj)
        {
            Authors.Add(new AuthorViewModel());
        }

        private void SaveBook(object obj)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Category) || string.IsNullOrWhiteSpace(ReleaseDate))
                {
                    MessageBox.Show("Please fill out all required fields (Title, Category, Release Date).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Authors.Count == 0 || Authors.Any(a => string.IsNullOrWhiteSpace(a.Name)))
                {
                    MessageBox.Show("Please add at least one author with valid details.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var dbContext = new LibraryDbContext())
                {
                    // Add book
                    var book = new Books
                    {
                        title = Title,
                        genre = Category,
                        published_year = int.Parse(ReleaseDate),
                        availability = true,
                        deleted = false,
                        pic_name = CoverPicLink,
                        summary = Description,
                        publisher = Publisher
                    };
                    dbContext.Books.Add(book);
                    dbContext.SaveChanges();

                    // Log the event
                    EventLogger.LogEvent(
                        $"New book added: ID {book.book_id}, Title: {book.title}, Category: {book.genre}, Release Year: {book.published_year}.",
                        EventLogEntryType.Information);

                    // Add authors and author-book relationships
                    foreach (var (author, index) in Authors.Select((a, i) => (a, i + 1)))
                    {
                        // Check if author already exists
                        var existingAuthor = dbContext.Authors.FirstOrDefault(a => a.name == author.Name && a.birthdate == author.Birthdate);
                        if (existingAuthor == null)
                        {
                            // Add new author
                            existingAuthor = new Authors
                            {
                                name = author.Name,
                                birthdate = author.Birthdate,
                                nationality = author.Nationality
                            };
                            dbContext.Authors.Add(existingAuthor);
                            dbContext.SaveChanges();
                        }

                        // Add author-book relationship
                        var authorBook = new AuthorBook
                        {
                            author_id = existingAuthor.author_id,
                            book_id = book.book_id,
                            author_order = index
                        };
                        dbContext.AuthorBooks.Add(authorBook);
                    }

                    dbContext.SaveChanges();

                    // Clear the text boxes by resetting the properties
                    Title = string.Empty;
                    Category = string.Empty;
                    ReleaseDate = string.Empty;
                    Publisher = string.Empty;
                    Description = string.Empty;
                    CoverPicLink = string.Empty;
                    Authors.Clear();
                }

                MessageBox.Show("Book added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding the book: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class AuthorViewModel : ObservableObject
    {
        private string _name;
        private string _birthdate;
        private string _nationality;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Birthdate
        {
            get => _birthdate;
            set
            {
                _birthdate = value;
                OnPropertyChanged();
            }
        }

        public string Nationality
        {
            get => _nationality;
            set
            {
                _nationality = value;
                OnPropertyChanged();
            }
        }
    }
}
