using System.ComponentModel;
using System.Runtime.CompilerServices;
using M120_Finanz_Projekt.Annotations;

namespace M120_Finanz_Projekt.Base
{
    public class Base : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
