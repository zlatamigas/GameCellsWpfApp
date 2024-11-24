using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace QueenGame.WPF.Converters
{
    public class ColorToSolidBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color) value;

            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = (SolidColorBrush) value;

            return brush.Color;
        }
    }
}
