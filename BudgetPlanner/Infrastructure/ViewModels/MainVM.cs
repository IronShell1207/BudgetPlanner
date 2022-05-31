﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using BudgetPlanner.Infrastructure.ViewModels.Base;
using BudgetPlanner.Objects;

namespace BudgetPlanner.Infrastructure.ViewModels
{
    public class MainVM : ViewModel
    {

        public string UserName { get; set; } = "Имя пользователя";
        public List<MoneyOperation> MoneyOperations { get; set; }
        public double Balance { get; set; } = 0;
        public double TodaysChange { get; set; } = 0;

        public List<string> OperationsTypes => OperationsCategories.OperationType;

        public List<string> OperationKind => SelectedOperationType == 0
            ? OperationsCategories.RecievedCategories
            : OperationsCategories.SpendCategories;
        #region AddNewOperation
        public MoneyOperation NewOperation { get; set;} = new MoneyOperation();
        public int SelectedOperationType { get; set; } = 0;
        public int SelectedOperationKind { get; set; } = 0;
        public RelayCommand SaveNewOperationCommand { get;  }
        
        public bool CanSaveNewOperation()
        {
            if (NewOperation.Sum != 0.0 && !string.IsNullOrWhiteSpace(NewOperation.OperationCategory))
                return true;
            return false;
        }
        public async void SaveNewOperation()
        {
            if (CanSaveNewOperation())
            {
            }
        }
#endregion

        private async void UpdateBalance()
        {
            using (AppDbContext dbContext = new AppDbContext())
            {
                var allOperations = await dbContext.GetAllMoneyMoves();
                var todayOperations = await dbContext.GetMoneyMovesByDate(DateTime.Now);
                Balance = 0;
                TodaysChange = 0;
                foreach (var operation in allOperations)
                    Balance += operation;
                foreach (var operation in todayOperations)
                    TodaysChange += operation;
                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(TodaysChange));
            }
        }

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
            Task.Run(UpdateBalance);
            SaveNewOperationCommand = new RelayCommand(SaveNewOperation, CanSaveNewOperation);
        }
    }
}
