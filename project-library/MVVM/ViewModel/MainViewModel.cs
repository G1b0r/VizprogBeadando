using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using project_library.Core;
using project_library.MVVM.View;
using Library;
using System.Windows;

namespace project_library.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {

        private object _currentView;
        private readonly Stack<object> _navigationStack = new Stack<object>();
        private bool _isLoggedIn;
        private Members _currentUser; 

        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value; 
                OnPropertyChanged(); 
            }
        }
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                _isLoggedIn = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAdmin));
            }
        }

        public Members CurrentUser 
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAdmin));
            }
        }

        public Visibility IsAdmin
        {
            get
            {
                return CurrentUser != null && CurrentUser.is_admin ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public ICommand LoginViewCommand { get; }
        public ICommand RegisterViewCommand { get; }
        public ICommand HomeViewCommand { get; }
        public ICommand MyBooksViewCommand { get; }
        public ICommand BookDetailViewCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand DiscoverViewCommand { get; }
        public ICommand SearchViewCommand { get; }
        public ICommand AdminViewCommand { get; }
        public ICommand LogoutCommand { get; }



        public MainViewModel()
        {
            _isLoggedIn = false; // Initially, the user is not logged in
            _currentView = null!; // Suppress warning for non-nullable field
            GoBackCommand = null!; // Suppress warning for non-nullable property

            // Initialize commands
            // Initialize commands
            LoginViewCommand = new RelayCommand(o => CurrentView = new LoginViewModel(this));
            RegisterViewCommand = new RelayCommand(o => CurrentView = new RegisterViewModel());
            HomeViewCommand = new RelayCommand(o => CurrentView = new HomeViewModel(this), o => IsLoggedIn);
            MyBooksViewCommand = new RelayCommand(o => CurrentView = new MyBooksViewModel(this), o => IsLoggedIn);
            BookDetailViewCommand = new RelayCommand(o => CurrentView = new BookDetailViewModel(null, this));
            DiscoverViewCommand = new RelayCommand(o => CurrentView = new DiscoverViewModel(this));
            SearchViewCommand = new RelayCommand(o => CurrentView = new SearchViewModel(this));
            AdminViewCommand = new RelayCommand(o => CurrentView = new AdminViewModel());
            LogoutCommand = new RelayCommand(ExecuteLogoutCommand);



            // Set default view
            CurrentView = new LoginViewModel(this);
        }
        private void ExecuteLogoutCommand(object parameter)
        {
            // Clear the current user session
            CurrentUser = null;
            IsLoggedIn = false;

            // Navigate to the LoginViewModel
            CurrentView = new LoginViewModel(this);
        }
        





    }
}
