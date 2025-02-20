using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views;
using Zorgdossier.Databases;

namespace Zorgdossier.ViewModels
{
    public class RegistrationViewModel : ObservableObject
    {
        #region fields
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private string _deviceName = string.Empty;
        #endregion

        #region constructers
        public RegistrationViewModel(IAppNavigation appNavigation, UserMessage userMessage, string deviceName)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _deviceName = deviceName;

            RegisterCommand = new RelayCommand(ExecuteRegister);

            Student = new Student();
        }

        public RegistrationViewModel()
        {

        }
        #endregion

        #region properties
        public Student Student
        {
            get;
        }
        public UserMessage UserMessage
        {
            get
            {
                return _userMessage;
            }
            set
            {
                _userMessage = value; OnPropertyChanged();
            }
        }
        #endregion

        #region commands
        public ICommand RegisterCommand
        {
            get;
        }
        #endregion

        #region methods
        private void ExecuteRegister(object? obj)
        {
            if (string.IsNullOrWhiteSpace(Student.StudentNumber) || string.IsNullOrWhiteSpace(Student.Name))
            {
                _userMessage.Text = "Voer alle invoervelden in.";
                return;
            }

            if (Student.StudentNumber.Length != 8)
            {
                _userMessage.Text = "Het studentnummer moet 8 karakters lang zijn. (PS123456)";
                return;
            }

            CreateDatabase.InitializeDatabase();

            using (var context = new ApplicationDbContext())
            {
                context.Database.Migrate();

                try
                {
                    var newStudent = new Student
                    {
                        Id = 1,
                        StudentNumber = Student.StudentNumber,
                        Name = Student.Name,
                        DeviceName = _deviceName,
                        CreatedAt = DateOnly.FromDateTime(DateTime.Today)
                    };

                    context.Student.Add(newStudent);
                    context.SaveChanges();
                    RedirectToMainView();
                }
                catch (Exception ex)
                {
                    _userMessage.Text = ("Fout bij het registreren van de student: " + ex.Message);
                }
            }
        }

        private void RedirectToMainView()
        {
            // Sluit de registratie view
            var registrationWindow = Application.Current.Windows.OfType<RegistrationView>().FirstOrDefault();
            if (registrationWindow != null)
            {
                // Open de MainView
                var mainView = new MainView();
                mainView.DataContext = new MainViewModel(_appNavigation, _userMessage);
                mainView.Show();

                registrationWindow.Close();
            }

        }
        #endregion
    }
}