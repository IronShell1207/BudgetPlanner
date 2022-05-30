﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Constants
{
    internal class ProgPathes
    {
        public static string DataFolderPath
        {
            get
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+@"\BudgetPlannetIrs\";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }
    }
}
