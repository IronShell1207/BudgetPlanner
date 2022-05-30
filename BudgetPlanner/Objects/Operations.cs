using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Objects
{
    public class Operations
    {
        public int Id { get; set; }
        public string OperationCategory { get; set; }
        public double Sum { get; set; }
        /// <summary>
        /// Type of operation - False is spend, True is cash in
        /// </summary>
        public bool Type { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }


    }
}
