using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using M120_Finanz_Projekt.Base;
using M120_Finanz_Projekt.Model;

namespace M120_Finanz_Projekt.ViewModel
{
    public class OutstandingPaymentsViewModel : FinanzViewModel
    {
        #region Initialization

        public OutstandingPaymentsViewModel()
        {
            Invoices = new ObservableCollection<OutstandingPayments>(this.DataAccess.GetOutstandingPayments(ViewProperties.LoggedInUser.AccountId));

            if (!Invoices.Any())
                HasInvoiceVisibility = Visibility.Visible;
        }

        #endregion

        #region Properties

        #region DataAccess

        public IBaseDataAccess DataAccess => _dataAccess ?? (_dataAccess = new BaseDataAccess());
        private IBaseDataAccess _dataAccess;

        #endregion

        /// <summary>
        /// Gets or sets the invoices.
        /// </summary>
        public IList<OutstandingPayments> Invoices
        {
            get => _invoice;
            set
            {
                _invoice = value;
                RaisePropertyChanged();
            }
        }
        private IList<OutstandingPayments> _invoice;

        /// <summary>
        /// Gets or sets the selected invoice.
        /// </summary>
        public OutstandingPayments SelectedInvoice
        {
            get => _selectedInvoice;
            set
            {
                _selectedInvoice = value;
                RaisePropertyChanged();
            }
        }
        private OutstandingPayments _selectedInvoice;

        /// <summary>
        /// Gets or sets the has invoice visibility.
        /// </summary>
        public Visibility HasInvoiceVisibility
        {
            get => _hasInvoiceVisibility;
            set
            {
                _hasInvoiceVisibility = value;
                RaisePropertyChanged();
            }
        }
        private Visibility _hasInvoiceVisibility = Visibility.Collapsed;

        #endregion

        #region Commands

        #region Decline invoice

        public ICommand DeclineInvoiceCommand => _declineInvoiceCommand ?? (_declineInvoiceCommand = new RelayCommand(OnDeclineInvoice));
        private ICommand _declineInvoiceCommand;

        private void OnDeclineInvoice(object sender)
        {
            if (SelectedInvoice == null)
            {
                MessageBox.Show("Please select a invoice.");
                return;
            }

            this.DataAccess.DeclineInvoice(SelectedInvoice.Invoice.InvoiceId);
            Invoices = new ObservableCollection<OutstandingPayments>(this.DataAccess.GetOutstandingPayments(ViewProperties.LoggedInUser.AccountId));
            HasInvoiceVisibility = !Invoices.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #region Pay invoice

        public ICommand PayInvoiceCommand => _payInvoiceCommand ?? (_payInvoiceCommand = new RelayCommand(OnPayInvoice));
        private ICommand _payInvoiceCommand;

        private void OnPayInvoice(object sender)
        {
            if (SelectedInvoice == null)
            {
                MessageBox.Show("Please select a invoice.");
                return;
            }

            if (SelectedInvoice.Invoice.Amount > ViewProperties.LoggedInUser.Balance)
            {
                MessageBox.Show("Please top up your Balance, you have to insufficient Balance.");
                return;
            }

            this.DataAccess.PayInvoice(SelectedInvoice.Invoice.InvoiceId, SelectedInvoice.Invoice.Amount, SelectedInvoice.Invoice.TransmitterAccountId, SelectedInvoice.Invoice.RecipientAccountId);
            Invoices = new ObservableCollection<OutstandingPayments>(this.DataAccess.GetOutstandingPayments(ViewProperties.LoggedInUser.AccountId));
            ViewProperties.LoggedInUser = this.DataAccess.GetUser(ViewProperties.LoggedInUser.AccountId, ViewProperties.LoggedInUser.Password);
            HasInvoiceVisibility = !Invoices.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #endregion
    }
}
