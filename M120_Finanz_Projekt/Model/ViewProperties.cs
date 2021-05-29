namespace M120_Finanz_Projekt.Model
{
    public class ViewProperties : Base.Base
    {
        /// <summary>
        /// Gets or sets the logged in user.
        /// </summary>
        public User LoggedInUser
        {
            get => _loggedInUser;
            set
            {
                _loggedInUser = value;
                RaisePropertyChanged();
            }
        }
        private User _loggedInUser;
    }
}
