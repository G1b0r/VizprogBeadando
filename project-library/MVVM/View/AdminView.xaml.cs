﻿using project_library.MVVM.ViewModel;
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

namespace project_library.MVVM.View
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : UserControl
    {
        public AdminView()
        {
            InitializeComponent();
            DataContext = new AdminViewModel();
        }

        private void addBookBtn_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the addbook window
            var addBookWindow = new addbook();

            // Show the window as a dialog
            addBookWindow.ShowDialog();
        }


    }
}
