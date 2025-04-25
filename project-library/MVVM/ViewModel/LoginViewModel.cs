using Library;
using Microsoft.EntityFrameworkCore;
using project_library.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace project_library.MVVM.ViewModel
{
    public class LoginViewModel : ObservableObject
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _isLoginSuccessful;

        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();

            }
        }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged();

            }
        }
        public bool IsLoginSuccessful
        {
            get { return _isLoginSuccessful; }
            set
            {
                _isLoginSuccessful = value;
                OnPropertyChanged();

            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        }

        private bool CanExecuteLoginCommand(object arg)
        {
            return !string.IsNullOrWhiteSpace(Username) && Username.Length >= 3 &&
                   !string.IsNullOrWhiteSpace(Password) && Password.Length >= 3;
        }

        private void ExecuteLoginCommand(object obj)
        {
            try
            {
                using (var context = new LibraryDbContext()) // Use the Library class as your DbContext
                {
                    var user = context.Members
                        .FirstOrDefault(m => m.username == Username && m.password == Password);

                    if (user != null)
                    {
                        IsLoginSuccessful = true;
                        ErrorMessage = string.Empty;

                        if (user.is_admin)
                        {
                            MessageBox.Show("Login successful! Welcome, Admin.");
                        }
                        else
                        {
                            MessageBox.Show("Login successful! Welcome, User.");
                        }
                        
                    }
                    else
                    {
                        IsLoginSuccessful = false;
                        ErrorMessage = "Invalid username or password.";
                        MessageBox.Show("❌ User not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                IsLoginSuccessful = false;
                ErrorMessage = $"An error occurred: {ex.Message}";

                // Log the full exception details
                Console.WriteLine("Exception Message: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);

                // Optionally, display the full details in a MessageBox (for debugging purposes)
                MessageBox.Show($"An error occurred:\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
    
}
