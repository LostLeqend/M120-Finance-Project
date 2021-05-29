using M120_Finanz_Projekt.Model;
using M120_Finanz_Projekt.ViewModel;

namespace M120_Finanz_Projekt.View.Windows
{
    /// <summary>
    /// Interaction logic for PaymentWindow.xaml
    /// </summary>
    public partial class PaymentWindow
    {
        public PaymentWindow(User user)
        {
            InitializeComponent();

            var viewModel = new PaymentViewModel(user);
            DataContext = viewModel;

            if(viewModel.CloseAction == null)
                viewModel.CloseAction = this.Close;
        }
    }
}
