using System;
using System.Windows;
using System.Windows.Input;
using M120_Finanz_Projekt.Base;
using M120_Finanz_Projekt.Model;

namespace M120_Finanz_Projekt.ViewModel
{
    public class InvoiceViewModel : Base.Base
    {
        #region Initialization

        public InvoiceViewModel(User transmitter)
        {
            Transmitter = transmitter;
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
        /// Gets or sets the transmitter.
        /// </summary>
        public User Transmitter
        {
            get => _transmitter ?? (_transmitter = new User());
            set
            {
                _transmitter = value;
                RaisePropertyChanged();
            }
        }
        private User _transmitter;

        /// <summary>
        /// Gets or sets the Recipient.
        /// </summary>
        public User Recipient
        {
            get => _recipient ?? (_recipient = new User());
            set
            {
                _recipient = value;
                RaisePropertyChanged();
            }
        }
        private User _recipient;

        /// <summary>
        /// Gets or sets a value indicating whether it has found the recipient.
        /// </summary>
        public bool HasFoundRecipient
        {
            get => _hasFoundRecipient;
            set
            {
                _hasFoundRecipient = value;
                RaisePropertyChanged();
            }
        }
        private bool _hasFoundRecipient;

        /// <summary>
        /// Gets or sets the payment.
        /// </summary>
        public Invoice Invoice
        {
            get => _invoice ?? (_invoice = new Invoice());
            set
            {
                _invoice = value;
                RaisePropertyChanged();
            }
        }
        private Invoice _invoice;

        /// <summary>
        /// Gets or sets the close action.
        /// </summary>
        public Action CloseAction { get; set; }

        #endregion

        #region Commands

        #region Find

        public ICommand FindCommand => _findCommand ?? (_findCommand = new RelayCommand(OnFind));
        private ICommand _findCommand;

        private void OnFind(object sender)
        {
            var backupAccountId = Recipient.AccountId;

            if (Recipient.AccountId != Transmitter.AccountId)
            {
                Recipient = this.DataAccess.GetUser(Recipient.AccountId);
            }
            else
            {
                MessageBox.Show("This is your own AccountNumber, please enter a valid AccountNumber.");
                return;
            }

            if (!string.IsNullOrEmpty(Recipient.Lastname))
            {
                HasFoundRecipient = true;
            }
            else
            {
                HasFoundRecipient = false;
                Recipient.AccountId = backupAccountId;
                MessageBox.Show("User with this AccountNumber could not be found, please try again.");
            }
        }

        #endregion

        #region Request

        public ICommand RequestCommand => _requestCommand ?? (_requestCommand = new RelayCommand(OnRequest));
        private ICommand _requestCommand;

        private void OnRequest(object sender)
        {
            OnFind(null);

            if (!HasFoundRecipient)
                return;

            var successful = this.DataAccess.CreateInvoice(Invoice.Amount, Invoice.PaymentReason, DateTime.Now, Transmitter.AccountId, Recipient.AccountId);

            if (successful)
            {
                MessageBox.Show("Sent invoice to recipient.");
                CloseAction.Invoke();
            }
            else
            {
                MessageBox.Show("Could not sent invoice to recipient.");
            }
        }

        #endregion

        #region Cancel

        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(OnCancel));
        private ICommand _cancelCommand;

        private void OnCancel(object sender)
        {
            CloseAction.Invoke();
        }

        #endregion

        #endregion
    }
}
