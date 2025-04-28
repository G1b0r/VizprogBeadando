using project_library.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for addbook.xaml
    /// </summary>
    public partial class addbook : Window
    {
        public addbook()
        {
            InitializeComponent();
            DataContext = new AddBookViewModel();
        }
    }
}
