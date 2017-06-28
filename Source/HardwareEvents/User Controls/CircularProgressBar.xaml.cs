using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HardwareEvents
{
    /// <summary>
    /// Interaction logic for CircularProgressBar.xaml
    /// </summary>
    public partial class CircularProgressBar
    {
        public CircularProgressBar()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                Background = Brushes.Transparent;
        }

        public static readonly DependencyProperty ProgressBarForegroundProperty = DependencyProperty.Register("ProgressBarForeground",
            typeof(Brush), typeof(CircularProgressBar), new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.None, ProgressBarForegroundChanged));

        private static void ProgressBarForegroundChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var bar = sender as CircularProgressBar;
            if (bar?.ParentCanvas == null)
                return;

            foreach (UIElement element in bar.ParentCanvas.Children)
                element.SetValue(Shape.FillProperty, e.NewValue);
        }

        public Brush ProgressBarForeground
        {
            get => (Brush)GetValue(ProgressBarForegroundProperty);
            set => SetValue(ProgressBarForegroundProperty, value);
        }
    }
}
