using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using M120_Finanz_Projekt.Base;
using M120_Finanz_Projekt.Model;
using M120_Finanz_Projekt.View.Windows;

namespace M120_Finanz_Projekt.ViewModel
{
    public class OverviewPaymentsViewModel : FinanzViewModel
    {
        #region Initialization

        public OverviewPaymentsViewModel()
        {
            Payments = new ObservableCollection<OverviewPayments>(DataAccess.GetPaymentsOverview(ViewProperties.LoggedInUser.AccountId));
        }

        #endregion

        #region Properties

        #region DataAccess

        /// <summary>
        /// Gets the data access.
        /// </summary>
        public IBaseDataAccess DataAccess => _dataAccess ?? (_dataAccess = new BaseDataAccess());
        private IBaseDataAccess _dataAccess;

        #endregion

        /// <summary>
        /// Gets or sets the payments.
        /// </summary>
        public IList<OverviewPayments> Payments
        {
            get => _payments ?? (_payments = new ObservableCollection<OverviewPayments>());
            set
            {
                _payments = value;
                RaisePropertyChanged();
            }
        }
        private IList<OverviewPayments> _payments;

        #endregion

        #region Commands

        #region Create invoice

        public ICommand CreateInvoiceCommand => _createInvoiceCommand ?? (_createInvoiceCommand = new RelayCommand(OnCreateInvoice));
        private ICommand _createInvoiceCommand;

        private void OnCreateInvoice(object sender)
        {
            var window = new InvoiceWindow(ViewProperties.LoggedInUser);
            window.ShowDialog();

            Payments = new ObservableCollection<OverviewPayments>(DataAccess.GetPaymentsOverview(ViewProperties.LoggedInUser.AccountId));
        }

        #endregion

        #region New payment

        public ICommand NewPaymentCommand => _newPaymentCommand ?? (_newPaymentCommand = new RelayCommand(OnNewPayment));
        private ICommand _newPaymentCommand;

        private void OnNewPayment(object sender)
        {
            var window = new PaymentWindow(ViewProperties.LoggedInUser);
            window.ShowDialog();

            Payments = new ObservableCollection<OverviewPayments>(DataAccess.GetPaymentsOverview(ViewProperties.LoggedInUser.AccountId));
            ViewProperties.LoggedInUser = DataAccess.GetUser(ViewProperties.LoggedInUser.AccountId, ViewProperties.LoggedInUser.Password);
        }

        #endregion

        #endregion
    }
}
