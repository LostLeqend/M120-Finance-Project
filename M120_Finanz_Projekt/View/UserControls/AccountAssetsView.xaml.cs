using M120_Finanz_Projekt.ViewModel;

namespace M120_Finanz_Projekt.View.UserControls
{
    /// <summary>
    /// Interaction logic for AccountAssetsView.xaml
    /// </summary>
    public partial class AccountAssetsView
    {
        public AccountAssetsView()
        {
            InitializeComponent();
            DataContext = new AccountAssetsViewModel();
        }
    }
}
