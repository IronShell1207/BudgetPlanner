using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using BudgetPlanner.Infrastructure.ViewModels.Base;

namespace BudgetPlanner.Infrastructure.ViewModels
{
    public class MainVM : ViewModel
    {

        public string AppName { get; set; } = "Me here";

        /// <summary>
        /// Gets or sets a value that indicates whether the app is showing the narrow, details-only view. 
        /// </summary>
        public bool IsInDetailsMode
        {
            get { return _isInDetailsMode; }
            set { SetProperty(ref _isInDetailsMode, value); }
        }
        private bool _isInDetailsMode = false;
        public MainVM()
        {

        }
    }
}
