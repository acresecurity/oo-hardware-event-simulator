
namespace HardwareEvents
{
    public class GenericSelectionModel<T> : ModelBase
    {
        public GenericSelectionModel(T item)
        {
            Item = item;
        }

        private T _item;

        public T Item
        {
            get => _item;
            set
            {
                _item = value;
                RaisePropertyChanged();
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                RaisePropertyChanged();
            }
        }
    }
}
