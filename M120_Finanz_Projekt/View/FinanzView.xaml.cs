using M120_Finanz_Projekt.Model;
using M120_Finanz_Projekt.ViewModel;

namespace M120_Finanz_Projekt.View
{
    /// <summary>
    /// Interaction logic for FinanzView.xaml
    /// </summary>
    public partial class FinanzView
    {
        public FinanzView(User user)
        {
            InitializeComponent();
            
            var viewModel = new FinanzViewModel(user);
            DataContext = viewModel;

            if(viewModel.CloseAction == null)
                viewModel.CloseAction = this.Close;
        }
    }
}
