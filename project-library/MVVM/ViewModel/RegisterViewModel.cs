using Library;
using Microsoft.EntityFrameworkCore;
using project_library.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace project_library.MVVM.ViewModel
{
    public class RegisterViewModel : ObservableObject
    {
        private string _username;
        private string _password;
        private string _name;
        private string _phoneNumber;
        private string _errorMessage;
        private string _successMessage;

        public string Username
        {
            get { return _username; }
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

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
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

        public string SuccessMessage
        {
            get { return _successMessage; }
            set
            {
                _successMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegisterCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(ExecuteRegisterCommand, CanExecuteRegisterCommand);
        }

        private bool CanExecuteRegisterCommand(object arg)
        {
            return true;
        }

        private async void ExecuteRegisterCommand(object obj)
        {
            try
            {
                using (var dbContext = new LibraryDbContext())
                {
                    // Check for duplicate username or password
                    var existingMember = await dbContext.Members
                        .FirstOrDefaultAsync(m => m.username == Username || m.password == Password);

                    if (existingMember != null)
                    {
                        ErrorMessage = "Username or password already in use.";
                        SuccessMessage = string.Empty;
                        return;
                    }

                    // Validate name
                    if (string.IsNullOrWhiteSpace(Name))
                    {
                        ErrorMessage = "Name cannot be empty.";
                        SuccessMessage = string.Empty;
                        return;
                    }

                    // Validate phone number
                    if (!int.TryParse(PhoneNumber, out int parsedPhoneNumber))
                    {
                        ErrorMessage = "Phone number must be a valid number.";
                        SuccessMessage = string.Empty;
                        return;
                    }

                    // Create a new member
                    var newMember = new Members
                    {
                        username = Username,
                        password = Password,
                        name = Name,
                        phone_number = parsedPhoneNumber,
                        is_admin = false // Admin is always false
                    };

                    dbContext.Members.Add(newMember);
                    await dbContext.SaveChangesAsync();

                    SuccessMessage = "Registration successful!";
                    ErrorMessage = string.Empty;
                }
            }
            catch (DbUpdateException dbEx)
            {
                var errorMessage = $"A database error occurred: {dbEx.InnerException?.Message ?? dbEx.Message}";
                ErrorMessage = errorMessage;

                System.IO.File.AppendAllText("error_log.txt", $"{DateTime.Now}: {errorMessage}{Environment.NewLine}");



                SuccessMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                SuccessMessage = string.Empty;
            }
        }
    }
    
}