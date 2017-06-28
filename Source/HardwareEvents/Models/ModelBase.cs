using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HardwareEvents
{
    public class ModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged, INotifyPropertyChanging

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
