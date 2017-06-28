using System;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace HardwareEvents.Converters
{
    public class BaseAssetImageConverter : MarkupExtension,  IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        protected static readonly string ImagePath = @"/HardwareEvents;component/Assets/";

        protected virtual BitmapImage ToImage(string imageSource)
        {
            if (!string.IsNullOrEmpty(imageSource))
            {
                if (Uri.TryCreate(ImagePath + imageSource, UriKind.RelativeOrAbsolute, out var imageUri))
                    return new BitmapImage(imageUri);
            }
            return null;
        }

        public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
