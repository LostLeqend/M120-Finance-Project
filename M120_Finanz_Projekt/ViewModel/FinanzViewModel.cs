using System;
using System.Windows.Input;
using M120_Finanz_Projekt.Base;
using M120_Finanz_Projekt.Model;
using M120_Finanz_Projekt.View;

namespace M120_Finanz_Projekt.ViewModel
{
    public class FinanzViewModel : Base.Base
    {
        #region Initialization

        public FinanzViewModel()
        {
        }

        public FinanzViewModel(User loggedInUser)
        {
            ViewProperties.LoggedInUser = loggedInUser;
        }

        #endregion

        #region Properties

        #region DataAccess

        public IBaseDataAccess DataAccess => _dataAccess ?? (_dataAccess = new BaseDataAccess());
        private IBaseDataAccess _dataAccess;

        #endregion

        public static ViewProperties ViewProperties
        {
            get => _viewProperties ?? (_viewProperties = new ViewProperties());
            set => _viewProperties = value;
        }
        private static ViewProperties _viewProperties;

        public object ViewModel
        {
            get => _viewModel ?? (_viewModel = new OverviewPaymentsViewModel());
            set
            {
                _viewModel = value;
                RaisePropertyChanged();
            }
        }
        private object _viewModel;

        /// <summary>
        /// Invoke to close view.
        /// </summary>
        public Action CloseAction { get; set; }

        #endregion

        #region Commands

        #region Overview Payments 

        public ICommand OverviewPaymentsCommand => _overviewPaymentsCommand ?? (_overviewPaymentsCommand = new RelayCommand(OnOverviewPayments));
        private ICommand _overviewPaymentsCommand;

        private void OnOverviewPayments(object sender)
        {
            if (ViewModel is OverviewPaymentsViewModel) 
                return;

            ViewModel = new OverviewPaymentsViewModel();
        }

        #endregion

        #region Outstanding Payments

        public ICommand OutstandingPaymentsCommand => _outstandingPaymentsCommand ?? (_outstandingPaymentsCommand = new RelayCommand(OnOutstandingPayments));
        private ICommand _outstandingPaymentsCommand;

        private void OnOutstandingPayments(object sender)
        {
            if (ViewModel is OutstandingPaymentsViewModel) 
                return;

            ViewModel = new OutstandingPaymentsViewModel();
        }

        #endregion

        #region Account Assets

        public ICommand AccountAssetsCommand => _accountAssetsCommand ?? (_accountAssetsCommand = new RelayCommand(OnAccountAssets));
        private ICommand _accountAssetsCommand;

        private void OnAccountAssets(object sender)
        {
            if (ViewModel is AccountAssetsViewModel) 
                return;

            ViewModel = new AccountAssetsViewModel();
        }

        #endregion

        #region Logout

        public ICommand LogoutCommand => _logoutCommand ?? (_logoutCommand = new RelayCommand(OnLogout));
        private ICommand _logoutCommand;

        private void OnLogout(object sender)
        {
            var view = new LoginView();
            view.Show();
            CloseAction.Invoke();
        }

        #endregion

        #endregion
    }
}
