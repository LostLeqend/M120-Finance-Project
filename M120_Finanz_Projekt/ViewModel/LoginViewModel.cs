using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using M120_Finanz_Projekt.Base;
using M120_Finanz_Projekt.Model;
using M120_Finanz_Projekt.View;

namespace M120_Finanz_Projekt.ViewModel
{
    public class LoginViewModel : Base.Base
    {
        #region Properties

        #region DataAccess

        public IBaseDataAccess DataAccess => _dataAccess ?? (_dataAccess = new BaseDataAccess());
        private IBaseDataAccess _dataAccess;

        #endregion

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public User User
        {
            get => _user ?? (_user = new User());
            set
            {
                _user = value;
                RaisePropertyChanged();
            }
        }
        private User _user;

        /// <summary>
        /// Gets the login password box.
        /// </summary>
        private PasswordBox LoginPasswordBox => GetLoginView().LoginPasswordBox;

        /// <summary>
        /// Gets the register password box.
        /// </summary>
        private PasswordBox RegisterPasswordBox => GetLoginView().RegisterPasswordBox;

        /// <summary>
        /// Invoke to close view.
        /// </summary>
        public Action CloseAction { get; set; }

        /// <summary>
        /// Gets or sets the login visibility.
        /// </summary>
        public Visibility LoginVisibility
        {
            get => _loginVisibility;
            set
            {
                _loginVisibility = value;
                RaisePropertyChanged();
            }
        }
        private Visibility _loginVisibility;

        #endregion

        #region Method

        /// <summary>
        /// Gets the login view.
        /// </summary>
        /// <returns>The login view.</returns>
        private LoginView GetLoginView()
        {
            if (Application.Current.Windows.OfType<LoginView>().FirstOrDefault() is LoginView loginView)
                return loginView;

            return null;
        }

        #endregion

        #region Commands

        #region Login

        public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new RelayCommand(OnLogin));
        private ICommand _loginCommand;

        private void OnLogin(object sender)
        {
            var user = DataAccess.GetUser(User.AccountId, LoginPasswordBox.Password);

            if (user != null)
            {
                var view = new FinanzView(user);
                view.Show();
                CloseAction.Invoke();
            }
            else
            {
                MessageBox.Show("AccountId and password do not match or you do not have an account yet.", "Login", MessageBoxButton.OK);
            }
        }

        #endregion

        #region Login and register switch

        public ICommand LoginAndRegisterSwitchCommand => _loginAndRegisterSwitchCommand ?? (_loginAndRegisterSwitchCommand = new RelayCommand(OnLoginAndRegisterSwitch));
        private ICommand _loginAndRegisterSwitchCommand;

        private void OnLoginAndRegisterSwitch(object sender)
        {
            LoginVisibility = LoginVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

            if (LoginVisibility == Visibility.Collapsed)
            {
                User = new User();
                LoginPasswordBox.Clear();
                RegisterPasswordBox.Clear();
            }
        }

        #endregion

        #region Register

        public ICommand RegisterCommand => _registerCommand ?? (_registerCommand = new RelayCommand(OnRegister));
        private ICommand _registerCommand;

        private void OnRegister(object sender)
        {
            var accountId = this.DataAccess.CreateUser(User.Firstname, User.Lastname, User.Address, User.Plz, User.Canton, RegisterPasswordBox.Password);

            if (accountId > 0)
            {
                MessageBox.Show("You successfully registered \rYour account number is: " + accountId + "\rYou will need it to log in.");
                OnLoginAndRegisterSwitch(null);
            }
            else
            {
                MessageBox.Show("Register did not work. Please check if you filled out all fields correctly.");
            }
        }

        #endregion

        #endregion
    }
}
