using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BudgetPlanner.Infrastructure.Converters
{
    public class DateTimeToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                DateTime dt = (DateTime)value;
                TimeSpan? ts = DateTimeConverter.DateTimeToTimeSpan(dt);
                return ts.GetValueOrDefault(TimeSpan.MinValue);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return TimeSpan.MinValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                TimeSpan ts = (TimeSpan)value;
                DateTime? dt = DateTimeConverter.TimeSpanToDateTime(ts);
                return dt.GetValueOrDefault(DateTime.MinValue);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return DateTime.MinValue;
            }
        }
    }
}
