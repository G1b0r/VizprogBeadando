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
    public class MainViewModel : ObservableObject
    {

        private object _currentView;
        private readonly Stack<object> _navigationStack = new Stack<object>();

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
        public ICommand MyBooksViewCommand { get; }
        public ICommand BookDetailViewCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand DiscoverViewCommand { get; }
        public ICommand SearchViewCommand { get; }

        public MainViewModel()
        {
            _currentView = null!; // Suppress warning for non-nullable field
            GoBackCommand = null!; // Suppress warning for non-nullable property

            // Initialize commands
            // Initialize commands
            LoginViewCommand = new RelayCommand(o => CurrentView = new LoginViewModel(this));
            RegisterViewCommand = new RelayCommand(o => CurrentView = new RegisterViewModel());
            HomeViewCommand = new RelayCommand(o => CurrentView = new HomeViewModel(this));
            MyBooksViewCommand = new RelayCommand(o => CurrentView = new MyBooksViewModel());
            BookDetailViewCommand = new RelayCommand(o => CurrentView = new BookDetailViewModel(null, this));
            DiscoverViewCommand = new RelayCommand(o => CurrentView = new DiscoverViewModel());
            SearchViewCommand = new RelayCommand(o => CurrentView = new SearchViewModel());
            

            // Set default view
            CurrentView = new LoginViewModel(this);
        }

    




    }
}
