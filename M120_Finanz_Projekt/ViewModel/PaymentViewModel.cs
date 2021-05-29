using System;
using System.Windows;
using System.Windows.Input;
using M120_Finanz_Projekt.Base;
using M120_Finanz_Projekt.Model;

namespace M120_Finanz_Projekt.ViewModel
{
    public class PaymentViewModel : Base.Base
    {
        #region Initialization

        public PaymentViewModel(User transmitter)
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
        public Payment Payment
        {
            get => _payment ?? (_payment = new Payment());
            set
            {
                _payment = value;
                RaisePropertyChanged();
            }
        }
        private Payment _payment;

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

        #region Pay

        public ICommand PayCommand => _payCommand ?? (_payCommand = new RelayCommand(OnPay));
        private ICommand _payCommand;

        private void OnPay(object sender)
        {
            if (Payment.Amount <= 0)
            {
                MessageBox.Show("Invalid Amount.");
                return;
            }

            if (Payment.Amount > Transmitter.Balance)
            {
                MessageBox.Show("Chosen amount can not be bigger than your current Balance.");
                return;
            }

            OnFind(null);
            if (!HasFoundRecipient)
                return;

            DataAccess.CreatePayment(Payment.Amount, Payment.PaymentReason, DateTime.Now, Transmitter.AccountId, Recipient.AccountId);
            CloseAction.Invoke();
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
