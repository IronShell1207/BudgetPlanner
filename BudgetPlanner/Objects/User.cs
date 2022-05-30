using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Objects
{
    public class User
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int Id { get; set; }
        public List<Operations> OperationsList { get; set; } = new List<Operations>();
    }
}
