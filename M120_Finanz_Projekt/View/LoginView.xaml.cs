using System.Windows;
using System.Windows.Controls;
using M120_Finanz_Projekt.ViewModel;

namespace M120_Finanz_Projekt.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView
    {
        public LoginView()
        {
            InitializeComponent(); 

            var viewModel = new LoginViewModel();
            DataContext = viewModel;

            if(viewModel.CloseAction == null)
                viewModel.CloseAction = this.Close;
        }
    }
}
