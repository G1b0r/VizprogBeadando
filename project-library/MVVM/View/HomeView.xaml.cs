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
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {

        // Parameterless constructor for WPF
        public HomeView()
        {
            InitializeComponent();
            
        }
        /*private void SetInitialButtonSize(Button button)
        {
            // Set the initial size of the buttons
            setButtonSize(button, 200, 250);
        }
        private void setButtonSize(Button button, double width, double height)
        {
            button.Width = width;
            button.Height = height;
            (button.Content as Image).Width = width;
            (button.Content as Image).Height = height;
        }*/

        // Constructor that accepts MainViewModel
        public HomeView(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = new HomeViewModel(mainViewModel);
        }
        private void ListBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is DiscoverViewModel viewModel && sender is ListBoxItem item && item.DataContext is Books selectedBook)
            {
                viewModel.SelectBookCommand.Execute(selectedBook);
            }
        }

    }
}