using Library;
using project_library.MVVM.ViewModel;
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
using System.Windows.Shapes;

namespace project_library
{
    /// <summary>
    /// Interaction logic for ChangeAmmountWindow.xaml
    /// </summary>
    public partial class ChangeAmmountWindow : Window
    {
        public ChangeAmmountWindow(Books selectedBook)
        {
            InitializeComponent();
            // Set the DataContext to the ViewModel
            DataContext = new ChangeAmmountWindowViewModel(selectedBook);
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Confirm button clicked!");
        }
    }
}
