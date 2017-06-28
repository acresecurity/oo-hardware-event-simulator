using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OpenOptions.dnaFusion.Flex.V1;

namespace HardwareEvents
{
    public class DoorModeIcon : ContentControl, INotifyPropertyChanged
    {
        public DoorModeIcon()
        {
            DefaultStyleKey = typeof(DoorModeIcon);
        }

        public static readonly DependencyProperty DoorModeProperty = DependencyProperty.Register(
            "DoorMode", typeof(DNADoorMode), typeof(DoorModeIcon), new PropertyMetadata(OnDoorModePropertyChanged));

        public DNADoorMode DoorMode
        {
            get => (DNADoorMode)GetValue(DoorModeProperty);
            set
            {
                SetValue(DoorModeProperty, value);
                OnPropertyChanged("DoorMode");
            }
        }

        private static void OnDoorModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as DoorModeIcon;
            source.UpdateText();
        }

        private void UpdateText()
        {
            ControlTemplate template = null;

            switch (DoorMode)
            {
                case DNADoorMode.Disable:
                    template = DisabledContentTemplate;
                    break;
                case DNADoorMode.Unlocked:
                    template = UnlockedContentTemplate;
                    break;
                case DNADoorMode.Locked:
                    template = LockedContentTemplate;
                    break;
                case DNADoorMode.FacilityCodeOnly:
                    template = FacilityCodeOnlyContentTemplate;
                    break;
                case DNADoorMode.CardOnly:
                    template = CardOnlyContentTemplate;
                    break;
                case DNADoorMode.PinOnly:
                    template = PinOnlyContentTemplate;
                    break;
                case DNADoorMode.CardAndPinRequired:
                    template = CardAndPinRequiredContentTemplate;
                    break;
                case DNADoorMode.CardOrPinRequired:
                    template = CardOrPinRequiredContentTemplate;
                    break;
            }

            if ((int)DoorMode == DefaultMode - 1 && (int)DoorMode == OfflineMode - 1)
            {
                Foreground = DefaultAndOfflineModeForeground;
                ToolTipService.SetToolTip(this, "Default and Offline Reader Mode");
            }
            else if ((int)DoorMode == DefaultMode - 1)
            {
                Foreground = DefaultModeForeground;
                ToolTipService.SetToolTip(this, "Default Reader Mode");
            }
            else if ((int)DoorMode == OfflineMode - 1)
            {
                Foreground = OfflineModeForeground;
                ToolTipService.SetToolTip(this, "Offline Reader Mode");
            }
            else
            {
                Foreground = StandardForeground;
                ToolTipService.SetToolTip(this, "");
            }

            if (template != null)
                Template = template;
        }

        private void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        public static readonly DependencyProperty DisabledContentTemplateProperty = DependencyProperty.Register(
            "DisabledContentTemplate", typeof(ControlTemplate), typeof(DoorModeIcon), new PropertyMetadata(OnDoorModePropertyChanged));

        public ControlTemplate DisabledContentTemplate
        {
            get => (ControlTemplate)GetValue(DisabledContentTemplateProperty);
            set => SetValue(DisabledContentTemplateProperty, value);
        }

        public static readonly DependencyProperty UnlockedContentTemplateProperty = DependencyProperty.Register(
            "UnlockedContentTemplate", typeof(ControlTemplate), typeof(DoorModeIcon), new PropertyMetadata(OnDoorModePropertyChanged));

        public ControlTemplate UnlockedContentTemplate
        {
            get => (ControlTemplate)GetValue(UnlockedContentTemplateProperty);
            set => SetValue(UnlockedContentTemplateProperty, value);
        }

        public static readonly DependencyProperty LockedContentTemplateProperty = DependencyProperty.Register(
            "LockedContentTemplate", typeof(ControlTemplate), typeof(DoorModeIcon), new PropertyMetadata(OnDoorModePropertyChanged));

        public ControlTemplate LockedContentTemplate
        {
            get => (ControlTemplate)GetValue(LockedContentTemplateProperty);
            set => SetValue(LockedContentTemplateProperty, value);
        }

        public static readonly DependencyProperty FacilityCodeOnlyContentTemplateProperty = DependencyProperty.Register(
            "FacilityCodeOnlyContentTemplate", typeof(ControlTemplate), typeof(DoorModeIcon), new PropertyMetadata(OnDoorModePropertyChanged));

        public ControlTemplate FacilityCodeOnlyContentTemplate
        {
            get => (ControlTemplate)GetValue(FacilityCodeOnlyContentTemplateProperty);
            set => SetValue(FacilityCodeOnlyContentTemplateProperty, value);
        }

        public static readonly DependencyProperty CardOnlyContentTemplateProperty = DependencyProperty.Register(
            "CardOnlyContentTemplate", typeof(ControlTemplate), typeof(DoorModeIcon), new PropertyMetadata(OnDoorModePropertyChanged));

        public ControlTemplate CardOnlyContentTemplate
        {
            get => (ControlTemplate)GetValue(CardOnlyContentTemplateProperty);
            set => SetValue(CardOnlyContentTemplateProperty, value);
        }

        public static readonly DependencyProperty PinOnlyContentTemplateProperty = DependencyProperty.Register(
            "PinOnlyContentTemplate", typeof(ControlTemplate), typeof(DoorModeIcon), new PropertyMetadata(OnDoorModePropertyChanged));

        public ControlTemplate PinOnlyContentTemplate
        {
            get => (ControlTemplate)GetValue(PinOnlyContentTemplateProperty);
            set => SetValue(PinOnlyContentTemplateProperty, value);
        }

        public static readonly DependencyProperty CardAndPinRequiredContentTemplateProperty = DependencyProperty.Register(
            "CardAndPinRequiredContentTemplate", typeof(ControlTemplate), typeof(DoorModeIcon), new PropertyMetadata(OnDoorModePropertyChanged));

        public ControlTemplate CardAndPinRequiredContentTemplate
        {
            get => (ControlTemplate)GetValue(CardAndPinRequiredContentTemplateProperty);
            set => SetValue(CardAndPinRequiredContentTemplateProperty, value);
        }

        public static readonly DependencyProperty CardOrPinRequiredContentTemplateProperty = DependencyProperty.Register(
            "CardOrPinRequiredContentTemplate", typeof(ControlTemplate), typeof(DoorModeIcon), new PropertyMetadata(OnDoorModePropertyChanged));

        public ControlTemplate CardOrPinRequiredContentTemplate
        {
            get => (ControlTemplate)GetValue(CardOrPinRequiredContentTemplateProperty);
            set => SetValue(CardOrPinRequiredContentTemplateProperty, value);
        }

        public static readonly DependencyProperty StandardForegroundProperty = DependencyProperty.Register(
            "StandardForeground", typeof(Brush), typeof(DoorModeIcon), new PropertyMetadata(OnDoorModePropertyChanged));

        public Brush StandardForeground
        {
            get => (Brush)GetValue(StandardForegroundProperty);
            set => SetValue(StandardForegroundProperty, value);
        }

        public static readonly DependencyProperty DefaultModeForegroundProperty = DependencyProperty.Register(
            "DefaultModeForeground", typeof(Brush), typeof(DoorModeIcon), new PropertyMetadata(OnDoorModePropertyChanged));

        public Brush DefaultModeForeground
        {
            get => (Brush)GetValue(DefaultModeForegroundProperty);
            set => SetValue(DefaultModeForegroundProperty, value);
        }

        public static readonly DependencyProperty OfflineModeForegroundProperty = DependencyProperty.Register(
            "OfflineModeForeground", typeof(Brush), typeof(DoorModeIcon), new PropertyMetadata(OnDoorModePropertyChanged));

        public Brush OfflineModeForeground
        {
            get => (Brush)GetValue(OfflineModeForegroundProperty);
            set => SetValue(OfflineModeForegroundProperty, value);
        }

        public static readonly DependencyProperty DefaultAndOfflineModeForegroundProperty = DependencyProperty.Register(
            "DefaultAndOfflineModeForeground", typeof(Brush), typeof(DoorModeIcon), new PropertyMetadata(OnDoorModePropertyChanged));

        public Brush DefaultAndOfflineModeForeground
        {
            get => (Brush)GetValue(DefaultAndOfflineModeForegroundProperty);
            set => SetValue(DefaultAndOfflineModeForegroundProperty, value);
        }

        public static readonly DependencyProperty DefaultModeProperty = DependencyProperty.Register(
            "DefaultMode", typeof(int), typeof(DoorModeIcon), new PropertyMetadata(OnDoorModePropertyChanged));

        public int DefaultMode
        {
            get => (int)GetValue(DefaultModeProperty);
            set => SetValue(DefaultModeProperty, value);
        }

        public static readonly DependencyProperty OfflineModeProperty = DependencyProperty.Register(
            "OfflineMode", typeof(int), typeof(DoorModeIcon), new PropertyMetadata(OnDoorModePropertyChanged));

        public int OfflineMode
        {
            get => (int)GetValue(OfflineModeProperty);
            set => SetValue(OfflineModeProperty, value);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
