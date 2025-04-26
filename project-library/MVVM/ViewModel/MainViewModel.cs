using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using project_library.Core;
using project_library.MVVM.View;

namespace project_library.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value; 
                OnPropertyChanged(); 
            }
        }


        public ICommand LoginViewCommand { get; }
        public ICommand RegisterViewCommand { get; }
        public ICommand HomeViewCommand { get; }

        public MainViewModel()
        {
            // Initialize commands
            LoginViewCommand = new RelayCommand(o => CurrentView = new LoginViewModel());
            RegisterViewCommand = new RelayCommand(o => CurrentView = new RegisterViewModel());
            HomeViewCommand = new RelayCommand(o => CurrentView = new HomeViewModel());

            // Set default view
            CurrentView = new LoginViewModel();
        }



        /*private bool? _isDarkTheme;

        public bool? IsDarkTheme
        {
          get { return _isDarkTheme; }
          set
          {
           _isDarkTheme = value;
           OnPropertyChanged();
          }
        }*/



    }
}
