namespace M120_Finanz_Projekt.Model
{
    public class User : Base.Base
    {
        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        public int AccountId
        {
            get => _accountId;
            set
            {
                _accountId = value;
                RaisePropertyChanged();
            }
        }
        private int _accountId;

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        public float Balance { get; set; }

        /// <summary>
        /// Gets or sets the firstname.
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// Gets or sets the lastname.
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the PLZ.
        /// </summary>
        public string Plz { get; set; }

        /// <summary>
        /// Gets or sets the canton.
        /// </summary>
        public string Canton { get; set; }
    }
}
