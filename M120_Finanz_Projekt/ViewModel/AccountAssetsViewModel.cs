using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using M120_Finanz_Projekt.Base;
using M120_Finanz_Projekt.Model;

namespace M120_Finanz_Projekt.ViewModel
{
    public class AccountAssetsViewModel : FinanzViewModel
    {
        #region Initialization

        public AccountAssetsViewModel()
        {
            CreateNewDataSeries( new List<IPayment>(this.DataAccess.GetPayments(ViewProperties.LoggedInUser.AccountId)), "Payments");
            CreateNewDataSeries(new List<IPayment>(this.DataAccess.GetInvoices(ViewProperties.LoggedInUser.AccountId).Where(x => x.Status == Status.Paid)), "Invoices");
        }

        #endregion

        #region Properties

        #region DataAccess

        public IBaseDataAccess DataAccess => _dataAccess ?? (_dataAccess = new BaseDataAccess());
        private IBaseDataAccess _dataAccess;

        #endregion

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public double Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                RaisePropertyChanged();
            }
        }
        private double _amount;

        /// <summary>
        /// Gets or sets the series collection.
        /// </summary>
        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection ?? (_seriesCollection = new SeriesCollection());
            set
            {
                _seriesCollection = value;
                RaisePropertyChanged();
            }
        }
        private SeriesCollection _seriesCollection;

        /// <summary>
        /// Gets or sets the labels.
        /// </summary>
        public List<string> Labels
        {
            get => _labels ?? (_labels = new List<string>());
            set
            {
                _labels = value;
                RaisePropertyChanged();
            }
        }
        private List<string> _labels;

        #endregion

        #region Methods

        private void CreateNewDataSeries(IList<IPayment> payments, string title)
        {
            var chart = new ColumnSeries
            {
                Title = title,
                Values = new ChartValues<double>(),
                Fill = Brushes.Transparent,
                StrokeThickness = 2,
            };

            foreach (var payment in payments)
            {
                chart.Values.Add(payment.Amount);
            }

            SeriesCollection.Add(chart);

            foreach (var payment in payments)
            {
                Labels.Add(payment.PaymentReason);
            }
        }

        #endregion

        #region Commands

        #region Top up balance

        public ICommand TopUpBalanceCommand => _topUpBalanceCommand ?? (_topUpBalanceCommand = new RelayCommand(OnTopUpBalance));
        private ICommand _topUpBalanceCommand;

        private void OnTopUpBalance(object sender)
        {
            this.DataAccess.TopUpBalance(ViewProperties.LoggedInUser.AccountId, Amount);

            Amount = 0;
            ViewProperties.LoggedInUser = this.DataAccess.GetUser(ViewProperties.LoggedInUser.AccountId, ViewProperties.LoggedInUser.Password);
        }

        #endregion

        #endregion
    }
}
