using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Objects
{
    public class MoneyOperations
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

    }
}
