using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Objects
{
    public class MoneyOperation : INotifyPropertyChanged
    {
        public long Id { get; set; }
        public string OperationCategory { get; set; }
        public double Sum { get; set; } = 0.0;

        /// <summary>
        /// Type of operation - False is spend, True is cash in
        /// </summary>
        public bool Type { get; set; } = false;
        public string Comment { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;

        public string ToString()
        {
            return $"{Id} - {Sum} {OperationCategory} {DateTime.ToShortDateString()}";
        }

        public string GetTypeString()
        {
            return Type ? OperationsCategories.OperationType.First() : OperationsCategories.OperationType.Last();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
