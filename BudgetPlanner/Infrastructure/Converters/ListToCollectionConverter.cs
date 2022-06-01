using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using BudgetPlanner.Objects;

namespace BudgetPlanner.Infrastructure.Converters
{
    internal class ListToCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                var list = value as List<MoneyOperation>;
                return new ObsCollection<MoneyOperation>(list);
            }

            return new ObsCollection<MoneyOperation>();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
