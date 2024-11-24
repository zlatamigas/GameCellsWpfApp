using System.Globalization;
using System.Windows.Data;

namespace QueenGame.WPF.Converters
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "--:--";
            }

            int timeSec = (int)value;

            return $"{timeSec / 60}:{(timeSec % 60):d2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] timeStr = ((string)value).Split(':');

            if (int.TryParse(timeStr[0], out int min) && int.TryParse(timeStr[1], out int sec))
            {
                return min * 60 + sec;
            }
            else
            {
                return 0;
            }
        }
    }
}
