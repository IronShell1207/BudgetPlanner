using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BudgetPlanner.Infrastructure.Converters
{
    public class BooleanToOperationTypeConverter: IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool) value ? "Пополнения" : "Затраты";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value == "Пополнения" ? true : false;
        }
        
    }
}
