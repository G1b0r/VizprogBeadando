﻿using Library;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace project_library.MVVM.View
{
    /// <summary>
    /// Interaction logic for DiscoverView.xaml
    /// </summary>
    public partial class DiscoverView : UserControl
    {
        public DiscoverView()
        {
            InitializeComponent();
            
        }
        public DiscoverView(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = new DiscoverViewModel(mainViewModel);
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
