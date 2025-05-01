using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Library;
using project_library.Core;

namespace project_library.MVVM.ViewModel
{
    public class SearchViewModel : ObservableObject
    {
        private string _title;
        private string _authorName;
        private string _category;
        private string _releaseBefore;
        private string _releaseAfter;
        private ObservableCollection<BookSearchResult> _searchResults;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string AuthorName
        {
            get => _authorName;
            set
            {
                _authorName = value;
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

        public string ReleaseBefore
        {
            get => _releaseBefore;
            set
            {
                _releaseBefore = value;
                OnPropertyChanged();
            }
        }

        public string ReleaseAfter
        {
            get => _releaseAfter;
            set
            {
                _releaseAfter = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BookSearchResult> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }

        public SearchViewModel()
        {
            SearchResults = new ObservableCollection<BookSearchResult>();
            SearchCommand = new RelayCommand(ExecuteSearch);
        }

        private void ExecuteSearch(object parameter)
        {
            try
            {
                using (var dbContext = new LibraryDbContext())
                {
                    var query = dbContext.Books
                        .Where(b => b.availability && !b.deleted); // Exclude unavailable and deleted books

                    // Filter by title
                    if (!string.IsNullOrWhiteSpace(Title))
                    {
                        query = query.Where(b => b.title.Contains(Title));
                        Console.WriteLine($"Filtering by Title: {Title}");
                    }

                    // Filter by author name
                    if (!string.IsNullOrWhiteSpace(AuthorName))
                    {
                        query = query.Where(b => b.AuthorBooks.Any(ab => ab.Authors1.name.Contains(AuthorName)));
                        Console.WriteLine($"Filtering by AuthorName: {AuthorName}");
                    }

                    // Filter by category
                    if (!string.IsNullOrWhiteSpace(Category))
                    {
                        query = query.Where(b => b.genre.Contains(Category));
                        Console.WriteLine($"Filtering by Category: {Category}");
                    }

                    // Filter by release year (before)
                    if (int.TryParse(ReleaseBefore, out int beforeYear))
                    {
                        query = query.Where(b => b.published_year < beforeYear);
                        Console.WriteLine($"Filtering by ReleaseBefore: {ReleaseBefore}");
                    }

                    // Filter by release year (after)
                    if (int.TryParse(ReleaseAfter, out int afterYear))
                    {
                        query = query.Where(b => b.published_year > afterYear);
                        Console.WriteLine($"Filtering by ReleaseAfter: {ReleaseAfter}");
                    }

                    // Materialize the query into memory to perform grouping
                    var books = query
                        .Select(b => new
                        {
                            b.title,
                            b.summary,
                            b.genre,
                            b.published_year,
                            b.pic_name,
                            Author = b.AuthorBooks.Select(ab => ab.Authors1.name).FirstOrDefault()
                        })
                        .ToList();

                    // Log the results
                    foreach (var book in books)
                    {
                        Console.WriteLine($"Title: {book.title}, Author: {book.Author}, Genre: {book.genre}, Year: {book.published_year}");
                    }

                    // Group by title and description to ensure unique results
                    var groupedResults = books
                        .GroupBy(b => new { b.title, b.summary })
                        .Select(g => g.First());

                    // Map results to the BookSearchResult class
                    var results = groupedResults.Select(b => new BookSearchResult
                    {
                        Title = b.title,
                        Author = b.Author,
                        Genre = b.genre,
                        PublishedYear = b.published_year,
                        CoverImagePath = b.pic_name
                    }).ToList();

                    // Update the search results
                    SearchResults = new ObservableCollection<BookSearchResult>(results);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., show a message to the user)
                MessageBox.Show($"An error occurred while searching: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }



    public class BookSearchResult
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int PublishedYear { get; set; }
        public string CoverImagePath { get; set; }
    }
}

