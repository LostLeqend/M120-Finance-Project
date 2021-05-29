using System;

namespace M120_Finanz_Projekt.Model
{
    public class Invoice : IPayment
    {
        /// <summary>
        /// Gets or sets the invoice identifier.
        /// </summary>
        public int InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public Status Status { get; set; }

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

    /// <summary>
    /// Status Enum
    /// </summary>
    public enum Status
    {
        Open = 0,
        Paid = 1,
        Declined =2
    }
}

