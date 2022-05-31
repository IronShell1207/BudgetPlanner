using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Objects
{
    public class OperationsCategories
    {
        public static List<string> SpendCategories = new List<string>()
        {
            "Развлечения",
            "Еда",
            "Транспорт",
            "Услуги",
            "Связь",
            "Комунальные услуги",
            "Одежда и обувь",
            "Спорт и отдых"
        };

        public static List<string> RecievedCategories = new List<string>()
        {
            "Заработная плата",
            "Возврат долга",
            "Диведенды"
        };

        public static List<string> OperationType = new List<string>()
        {
            "Доходы",
            "Расходы"
        };
    }
}
