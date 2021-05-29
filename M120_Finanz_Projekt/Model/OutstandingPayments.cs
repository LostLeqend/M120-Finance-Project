namespace M120_Finanz_Projekt.Model
{
    public class OutstandingPayments
    {
        /// <summary>
        /// Gets or sets the invoice.
        /// </summary>
        public Invoice Invoice { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public User User { get; set; }
    }
}
