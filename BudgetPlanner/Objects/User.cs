using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Objects
{
    public class User
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<Operations> OperationsList { get; set; } = new List<Operations>();
    }
}
