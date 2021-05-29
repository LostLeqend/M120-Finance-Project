using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using Dapper;
using M120_Finanz_Projekt.Model;

namespace M120_Finanz_Projekt.Base
{
    #region IBaseDataAccess

    public interface IBaseDataAccess
    {
        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>The user.</returns>
        User GetUser(int accountId);

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns>The user.</returns>
        User GetUser(int accountId, string password);

        /// <summary>
        /// Creates a user.
        /// </summary>
        /// <param name="firstname">The firstname.</param>
        /// <param name="lastname">The lastname.</param>
        /// <param name="address">The address.</param>
        /// <param name="plz">The PLZ.</param>
        /// <param name="canton">The canton.</param>
        /// <param name="password">The password.</param>
        int CreateUser(string firstname, string lastname, string address, string plz, string canton, string password);

        /// <summary>
        /// Gets the payments.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>The payments.</returns>
        IEnumerable<Payment> GetPayments(int accountId);

        /// <summary>
        /// Gets the payments overview.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>The payments overview.</returns>
        IEnumerable<OverviewPayments> GetPaymentsOverview(int accountId);

        /// <summary>
        /// Creates the payment.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="paymentReason">The payment reason.</param>
        /// <param name="date">The date.</param>
        /// <param name="transmitterAccountId">The transmitter account identifier.</param>
        /// <param name="recipientAccountId">The recipient account identifier.</param>
        void CreatePayment(double amount, string paymentReason, DateTime date, int transmitterAccountId, int recipientAccountId);

        /// <summary>
        /// Gets the invoices.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>The invoices.</returns>
        IEnumerable<Invoice> GetInvoices(int accountId);

        /// <summary>
        /// Gets the outstanding payments.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>The outstanding payments.</returns>
        IEnumerable<OutstandingPayments> GetOutstandingPayments(int accountId);

        /// <summary>
        /// Creates the invoice.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="paymentReason">The payment reason.</param>
        /// <param name="date">The date.</param>
        /// <param name="transmitterAccountId">The transmitter account identifier.</param>
        /// <param name="recipientAccountId">The recipient account identifier.</param>
        bool CreateInvoice(double amount, string paymentReason, DateTime date, int transmitterAccountId, int recipientAccountId);

        /// <summary>
        /// Tops up balance.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="amount">The amount.</param>
        void TopUpBalance(int accountId, double amount);

        /// <summary>
        /// Declines the invoice.
        /// </summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        void DeclineInvoice(double invoiceId);

        /// <summary>
        /// Pays the invoice.
        /// </summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="transmitterAccountId">The transmitter account identifier.</param>
        /// <param name="recipientAccountId">The recipient account identifier.</param>
        void PayInvoice(int invoiceId, double amount, int transmitterAccountId, int recipientAccountId);
    }

    #endregion

    public class BaseDataAccess : IBaseDataAccess
    {
        private const string ConnectionString = "Server=DESKTOP-RAPHI\\SQLSERVER;Database=M120ProjectDb;Trusted_Connection=True";

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>The user.</returns>
        public User GetUser(int accountId)
        {
            const string sql = "SELECT Account.AccountId, Person.Firstname, Person.Lastname, Person.Address, Person.Plz, Person.Canton FROM Account " +
                               "INNER JOIN Person ON Account.AccountId = Person.FK_AccountId WHERE Account.AccountId = @AccountId";

            var parameters = new { AccountId = accountId };
            User user;

            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    user = connection.QuerySingle<User>(sql, parameters);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return user;
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns>The user.</returns>
        public User GetUser(int accountId, string password)
        {
            const string sql =
                "SELECT * FROM Account INNER JOIN Person ON Account.AccountId = Person.FK_AccountId WHERE Account.AccountId = @AccountId and Account.Password = @Password";
            var parameters = new { AccountId = accountId, Password = password };
            User user;

            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    user = connection.QuerySingle<User>(sql, parameters);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return user;
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="firstname">The firstname.</param>
        /// <param name="lastname">The lastname.</param>
        /// <param name="address">The address.</param>
        /// <param name="plz">The PLZ.</param>
        /// <param name="canton">The canton.</param>
        /// <param name="password">The password.</param>
        public int CreateUser(string firstname, string lastname, string address, string plz, string canton, string password)
        {
            if (string.IsNullOrEmpty(firstname))
                return 0;
            if (string.IsNullOrEmpty(lastname))
                return 0;
            if (string.IsNullOrEmpty(address))
                return 0;
            if (string.IsNullOrEmpty(plz))
                return 0;
            if (string.IsNullOrEmpty(canton))
                return 0;
            if (string.IsNullOrEmpty(password))
                return 0;

            const string insertAccountSql = "INSERT INTO Account (Password, Balance) values (@Password, 10000)";
            var accountParameters = new { Password = password };

            const string getNewAccountIdSql =
                "SELECT AccountId FROM Account WHERE AccountId=(SELECT max(AccountId) FROM Account)";

            const string insertPersonSql =
                "INSERT INTO Person (FirstName, LastName, Address, Plz, Canton, Fk_AccountId) VALUES (@Firstname, @Lastname, @Address, @PLZ, @Canton, @AccountId);";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(insertAccountSql, accountParameters);

                var accountId = connection.QuerySingle<int>(getNewAccountIdSql);
                var insertPersonParameters = new
                {
                    Firstname = firstname,
                    Lastname = lastname,
                    Address = address,
                    PLZ = plz,
                    Canton = canton,
                    AccountId = accountId
                };

                connection.Execute(insertPersonSql, insertPersonParameters);

                return accountId;
            }
        }

        /// <summary>
        /// Gets the payments.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>The payments.</returns>
        public IEnumerable<Payment> GetPayments(int accountId)
        {
            const string sql = "SELECT * FROM Payment WHERE Fk_TransmitterAccountId = @accountId";
            var parameters = new { AccountId = accountId };
            List<Payment> payments;

            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    payments = connection.Query<Payment>(sql, parameters).ToList();
                }
                catch (Exception)
                {
                    return new List<Payment>();
                }
            }

            return payments;
        }

        /// <summary>
        /// Gets the payments overview.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>The payments overview.</returns>
        public IEnumerable<OverviewPayments> GetPaymentsOverview(int accountId)
        {
            const string sql = "SELECT * FROM Payment INNER JOIN Person ON Payment.Fk_RecipientAccountId = Person.FK_AccountId WHERE Payment.Fk_TransmitterAccountId = @AccountId";
            var parameters = new { AccountId = accountId };
            var paymentsOverview = new List<OverviewPayments>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    var dynamicPaymentsOverview = connection.Query(sql, parameters).ToList();

                    foreach (var paymentOverview in dynamicPaymentsOverview)
                    {
                        if (paymentOverview is IDictionary<string, object> fields)
                        {
                            paymentsOverview.Add(new OverviewPayments
                            {
                                Payment = new Payment
                                {
                                    PaymentId = Convert.ToInt32(fields["PaymentId"]),
                                    Amount = Convert.ToDouble(fields["Amount"]),
                                    PaymentReason = Convert.ToString(fields["Paymentreason"]),
                                    Date = Convert.ToDateTime(fields["Date"]),
                                    RecipientAccountId = Convert.ToInt32(fields["Fk_RecipientAccountId"]),
                                    TransmitterAccountId = Convert.ToInt32(fields["Fk_TransmitterAccountId"])
                                },
                                User = new User
                                {
                                    AccountId = Convert.ToInt32(fields["PersonId"]),
                                    Firstname = fields["Firstname"].ToString(),
                                    Lastname = fields["Lastname"].ToString(),
                                    Address = fields["Address"].ToString(),
                                    Plz = fields["Plz"].ToString(),
                                    Canton = fields["Canton"].ToString()
                                }
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    return new List<OverviewPayments>();
                }
            }

            return paymentsOverview;
        }

        /// <summary>
        /// Creates the payment.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="paymentReason">The payment reason.</param>
        /// <param name="date">The date.</param>
        /// <param name="transmitterAccountId">The transmitter account identifier.</param>
        /// <param name="recipientAccountId">The recipient account identifier.</param>
        public void CreatePayment(double amount, string paymentReason, DateTime date, int transmitterAccountId, int recipientAccountId)
        {
            const string insertPaymentSql = "INSERT INTO Payment VALUES (@Amount, @PaymentReason, @Date, @TransmitterAccountId, @RecipientAccountId)";
            var paymentParameters = new
            {
                Amount = amount,
                PaymentReason = paymentReason ?? string.Empty,
                Date = date,
                TransmitterAccountId = transmitterAccountId,
                RecipientAccountId = recipientAccountId
            };

            const string removeBalanceSql =
                "UPDATE Account SET Balance = (SELECT SUM(Balance) from Account WHERE AccountId = @AccountId) - @Amount WHERE AccountId = @AccountId";
            var removeBalanceParameters = new { Amount = amount, AccountId = transmitterAccountId };

            const string addBalanceSql =
                "UPDATE Account SET Balance = (SELECT SUM(Balance) from Account WHERE AccountId = @AccountId) + @Amount WHERE AccountId = @AccountId";
            var addBalanceParameters = new { Amount = amount, AccountId = recipientAccountId };

            using (var connection = new SqlConnection(ConnectionString))
            {
                var successful = connection.Execute(insertPaymentSql, paymentParameters);

                if (successful == 1)
                {
                    connection.Execute(removeBalanceSql, removeBalanceParameters);
                    connection.Execute(addBalanceSql, addBalanceParameters);
                }
            }
        }

        /// <summary>
        /// Gets the invoices.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>The invoices.</returns>
        public IEnumerable<Invoice> GetInvoices(int accountId)
        {
            const string sql = "SELECT * FROM Invoice WHERE Fk_RecipientAccountId = @accountId";
            var parameters = new { AccountId = accountId };
            List<Invoice> invoices;

            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    invoices = connection.Query<Invoice>(sql, parameters).ToList();
                }
                catch (Exception)
                {
                    return new List<Invoice>();
                }
            }

            return invoices;
        }

        /// <summary>
        /// Gets the outstanding payments.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>The outstanding payments.</returns>
        public IEnumerable<OutstandingPayments> GetOutstandingPayments(int accountId)
        {
            const string sql = "SELECT * FROM Invoice INNER JOIN Person ON Invoice.Fk_TransmitterAccountId = Person.FK_AccountId WHERE Invoice.Fk_RecipientAccountId = @AccountId AND Invoice.Status = 0";
            var parameters = new { AccountId = accountId };
            var paymentsOverview = new List<OutstandingPayments>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    var dynamicPaymentsOverview = connection.Query(sql, parameters).ToList();

                    foreach (var paymentOverview in dynamicPaymentsOverview)
                    {
                        if (paymentOverview is IDictionary<string, object> fields)
                        {
                            paymentsOverview.Add(new OutstandingPayments
                            {
                                Invoice = new Invoice
                                {
                                    InvoiceId = Convert.ToInt32(fields["InvoiceId"]),
                                    Amount = Convert.ToDouble(fields["Amount"]),
                                    PaymentReason = Convert.ToString(fields["Paymentreason"]),
                                    Status = (Status)Convert.ToInt32(fields["Status"]),
                                    Date = Convert.ToDateTime(fields["Date"]),
                                    RecipientAccountId = Convert.ToInt32(fields["Fk_RecipientAccountId"]),
                                    TransmitterAccountId = Convert.ToInt32(fields["Fk_TransmitterAccountId"])
                                },
                                User = new User
                                {
                                    AccountId = Convert.ToInt32(fields["PersonId"]),
                                    Firstname = fields["Firstname"].ToString(),
                                    Lastname = fields["Lastname"].ToString(),
                                    Address = fields["Address"].ToString(),
                                    Plz = fields["Plz"].ToString(),
                                    Canton = fields["Canton"].ToString()
                                }
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    return new List<OutstandingPayments>();
                }
            }

            return paymentsOverview;
        }

        /// <summary>
        /// Creates the invoice.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="paymentReason">The payment reason.</param>
        /// <param name="date">The date.</param>
        /// <param name="transmitterAccountId">The transmitter account identifier.</param>
        /// <param name="recipientAccountId">The recipient account identifier.</param>
        public bool CreateInvoice(double amount, string paymentReason, DateTime date, int transmitterAccountId, int recipientAccountId)
        {
            if (amount <= 0)
            {
                MessageBox.Show("Invalid amount");
                return false;
            }

            const string insertPaymentSql = "INSERT INTO Invoice VALUES (@Amount, @PaymentReason, @Status, @Date, @TransmitterAccountId, @RecipientAccountId)";
            var paymentParameters = new
            {
                Amount = amount,
                PaymentReason = paymentReason ?? string.Empty,
                Date = date,
                Status = 0,
                TransmitterAccountId = transmitterAccountId,
                RecipientAccountId = recipientAccountId
            };

            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.Execute(insertPaymentSql, paymentParameters) == 1;
            }
        }

        /// <summary>
        /// Tops up balance.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="amount">The amount.</param>
        public void TopUpBalance(int accountId, double amount)
        {
            const string topUpBalanceSql = "UPDATE Account SET Balance = (SELECT SUM(Balance) from Account WHERE AccountId = @AccountId) + @Amount WHERE AccountId = @AccountId";
            var topUpBalanceParameters = new { Amount = amount, AccountId = accountId };

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(topUpBalanceSql, topUpBalanceParameters);
            }
        }

        /// <summary>
        /// Declines the invoice.
        /// </summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        public void DeclineInvoice(double invoiceId)
        {
            const string declineInvoiceSql = "UPDATE Invoice SET Status = 2 WHERE InvoiceId = @InvoiceId";
            var declineInvoiceParameters = new { InvoiceId = invoiceId };

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(declineInvoiceSql, declineInvoiceParameters);
            }
        }

        /// <summary>
        /// Pays the invoice.
        /// </summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="transmitterAccountId">The transmitter account identifier.</param>
        /// <param name="recipientAccountId">The recipient account identifier.</param>
        public void PayInvoice(int invoiceId, double amount, int transmitterAccountId, int recipientAccountId)
        {
            const string setStatusInvoiceSql = "UPDATE Invoice SET Status = 1 WHERE InvoiceId =  @InvoiceId";
            var setStatusInvoiceParameters = new { InvoiceId = invoiceId };

            const string removeBalanceSql = "UPDATE Account SET Balance = (SELECT SUM(Balance) from Account WHERE AccountId = @AccountId) - @Amount WHERE AccountId = @AccountId";
            var removeBalanceParameters = new { Amount = amount, AccountId = recipientAccountId };

            const string addBalanceSql = "UPDATE Account SET Balance = (SELECT SUM(Balance) from Account WHERE AccountId = @AccountId) + @Amount WHERE AccountId = @AccountId";
            var addBalanceParameters = new { Amount = amount, AccountId = transmitterAccountId };

            using (var connection = new SqlConnection(ConnectionString))
            {
                var successful = connection.Execute(setStatusInvoiceSql, setStatusInvoiceParameters);

                if (successful == 1)
                {
                    connection.Execute(removeBalanceSql, removeBalanceParameters);
                    connection.Execute(addBalanceSql, addBalanceParameters);
                }
            }
        }
    }
}