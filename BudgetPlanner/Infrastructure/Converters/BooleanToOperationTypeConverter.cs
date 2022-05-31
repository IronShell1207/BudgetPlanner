using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using BudgetPlanner.Objects;

namespace BudgetPlanner.Infrastructure.Converters
{
    public class BooleanToOperationTypeConverter: IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool) value ? OperationsCategories.OperationType.First() : OperationsCategories.OperationType.Last();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value == OperationsCategories.OperationType.First() ? true : false;
        }
        
    }
}
