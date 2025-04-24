using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project_library.Core;
using project_library.MVVM.View;

namespace project_library.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand LoginViewCommand { get; set; }
        public RelayCommand RegisterViewCommand { get; set; }

        public LoginViewModel LoginVM { get; set; }

        public RegisterViewModel RegisterVM { get; set; }

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


        public MainViewModel() 
        {
            LoginVM = new LoginViewModel();
            RegisterVM = new RegisterViewModel();

            CurrentView = LoginVM;

            LoginViewCommand = new RelayCommand(o => { CurrentView = LoginVM; });
            RegisterViewCommand = new RelayCommand(o => { CurrentView = RegisterVM; });
        }

    }
}
