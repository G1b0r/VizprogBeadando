using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Library;
using project_library.MVVM.ViewModel;


namespace project_library.MVVM.View
{
    /// <summary>
    /// Interaction logic for BookDetailView.xaml
    /// </summary>
    public partial class BookDetailView : UserControl
    {
        public BookDetailView()
        {
            InitializeComponent();
        }

        private void changeAmmount_Click(object sender, RoutedEventArgs e)
        {
            // Get the SelectedBook from the DataContext (BookDetailViewModel)
            if (DataContext is BookDetailViewModel viewModel && viewModel.SelectedBook != null)
            {
                // Pass the SelectedBook to the ChangeAmmountWindow
                ChangeAmmountWindow changAmm = new ChangeAmmountWindow(viewModel.SelectedBook);
                changAmm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No book is selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
