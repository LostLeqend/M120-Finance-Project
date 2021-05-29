using M120_Finanz_Projekt.Model;
using M120_Finanz_Projekt.ViewModel;

namespace M120_Finanz_Projekt.View.Windows
{
    /// <summary>
    /// Interaction logic for InvoiceWindow.xaml
    /// </summary>
    public partial class InvoiceWindow
    {
        public InvoiceWindow(User user)
        {
            InitializeComponent();

            var viewModel = new InvoiceViewModel(user);
            DataContext = viewModel;

            if(viewModel.CloseAction == null)
                viewModel.CloseAction = this.Close;
        }
    }
}
