using Library;
using project_library.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace project_library
{
    /// <summary>
    /// Interaction logic for ChangeAmmountWindow.xaml
    /// </summary>
    public partial class ChangeAmmountWindow : Window
    {
        private readonly Books _selectedBook;

        public ChangeAmmountWindow(Books selectedBook)
        {
            InitializeComponent();
            // Set the DataContext to the ViewModel
            DataContext = new ChangeAmmountWindowViewModel(selectedBook);
            _selectedBook = selectedBook;
        }

        private void newAmmountTxt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Allow only numeric input
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }

        private async void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(newAmmountTxt.Text, out int numberOfCopies) && numberOfCopies > 0)
            {
                using (var dbContext = new LibraryDbContext())
                {
                    for (int i = 0; i < numberOfCopies; i++)
                    {
                        // Create a new book copy
                        var newBook = new Books
                        {
                            title = _selectedBook.title,
                            genre = _selectedBook.genre,
                            published_year = _selectedBook.published_year,
                            author_id = _selectedBook.author_id,
                            pic_name = _selectedBook.pic_name,
                            summary = _selectedBook.summary,
                            publisher = _selectedBook.publisher,
                            availability = true, // Set availability to true
                            deleted = false      // Set deleted to false
                        };

                        dbContext.Books.Add(newBook);
                        await dbContext.SaveChangesAsync(); // Save to generate book_id

                        // Add corresponding author-book relationships
                        var authorBooks = dbContext.AuthorBooks
                            .Where(ab => ab.book_id == _selectedBook.book_id)
                            .ToList();

                        foreach (var authorBook in authorBooks)
                        {
                            var newAuthorBook = new AuthorBook
                            {
                                author_id = authorBook.author_id,
                                book_id = newBook.book_id,
                                author_order = authorBook.author_order
                            };

                            dbContext.AuthorBooks.Add(newAuthorBook);
                        }

                        // Save changes to the database
                        await dbContext.SaveChangesAsync();
                    }
                    if (DataContext is ChangeAmmountWindowViewModel viewModel)
                    {
                        viewModel.LoadCurrentAmount();
                    }
                    MessageBox.Show("Books and corresponding author relationships added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);


                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number greater than 0.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
