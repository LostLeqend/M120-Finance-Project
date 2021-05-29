using System;

namespace M120_Finanz_Projekt.Model
{
    #region IPayment

    public interface IPayment
    {
        /// <summary>
        /// Gets or sets the payment identifier.
        /// </summary>
        int PaymentId { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        double Amount { get; set; }

        /// <summary>
        /// Gets or sets the payment reason.
        /// </summary>
        string PaymentReason { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the transmitter account identifier.
        /// </summary>
        int TransmitterAccountId { get; set; }

        /// <summary>
        /// Gets or sets the recipient account identifier.
        /// </summary>
        int RecipientAccountId { get; set; }
    }

    #endregion

    public class Payment : IPayment
    {
        /// <summary>
        /// Gets or sets the payment identifier.
        /// </summary>
        public int PaymentId { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Gets or sets the payment reason.
        /// </summary>
        public string PaymentReason { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the transmitter account identifier.
        /// </summary>
        public int TransmitterAccountId { get; set; }

        /// <summary>
        /// Gets or sets the recipient account identifier.
        /// </summary>
        public int RecipientAccountId { get; set; }
    }
}
