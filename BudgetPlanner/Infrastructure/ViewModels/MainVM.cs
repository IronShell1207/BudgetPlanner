using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.PointOfService;
using Windows.UI.Xaml;
using BudgetPlanner.Infrastructure.Converters;
using BudgetPlanner.Infrastructure.ViewModels.Base;
using BudgetPlanner.Objects;
using Microsoft.Toolkit.Mvvm;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Infrastructure.ViewModels
{
    public class MainVM : ObservableObject
    {

        public string UserName { get; set; } = "Имя пользователя";
        private List<MoneyOperations> _moneyOperations = new List<MoneyOperations>();
        public List<MoneyOperations> MoneyOperations
        {
            get => _moneyOperations;
            set
            {
                SetProperty(ref _moneyOperations, value);
            }
        }
        private double _balance = 0;
        public double Balance
        {
            get => _balance;
            set
            {
                SetProperty(ref _balance, value);
            }
        }

        private double _todaysChange = 0;
        public double TodaysChange
        {
            get => _todaysChange;
            set
            {
                SetProperty(ref _todaysChange, value);
            }
        }
        public string Currency { get; set; } = "$";

        #region IncCommand

        public ICommand IncCommand { get; }

        private bool CanIncCommandExecute()
        {
            return true;
        }

        private async void OnIncCommandExecuting()
        {
            Balance++;
            await Task.Delay(100);
            UpdateBalance();
            var newOpera = NewOperation;
            MoneyOperations.Add(newOpera);
        }

        #endregion

        public List<string> OperationsTypes => OperationsCategories.OperationType;

        public List<string> OperationKinds { get; set; } = OperationsCategories.RecievedCategories;
        #region AddNewOperation

        public MoneyOperations NewOperation { get; set; } = new MoneyOperations();
        public int SelectedOperationType { get; set; } = 0;

        public DateTimeOffset Date
        {
            get
            {
                DateTimeOffset? ts = DateTimeConverter.DateTimeToDateTimeOffSet(NewOperation.DateTime);
                return ts.GetValueOrDefault(DateTimeOffset.MinValue);
            }
            set
            {
                NewOperation.DateTime = new DateTime(value.Year,
                    value.Month,
                    value.Day,
                    NewOperation.DateTime.Hour,
                    NewOperation.DateTime.Minute,
                    NewOperation.DateTime.Second,
                    NewOperation.DateTime.Millisecond);
            }
        }
        public TimeSpan Time
        {
            get
            {
                TimeSpan? ts = DateTimeConverter.DateTimeToTimeSpan(NewOperation.DateTime);
                return ts.GetValueOrDefault(TimeSpan.MinValue);
            }
            set
            {
                DateTime? dt = DateTimeConverter.TimeSpanToDateTime(value);

                NewOperation.DateTime = new DateTime(NewOperation.DateTime.Year,
                    NewOperation.DateTime.Month,
                    NewOperation.DateTime.Day,
                    dt.GetValueOrDefault(DateTime.MinValue).Hour,
                    dt.GetValueOrDefault(DateTime.MinValue).Minute,
                    dt.GetValueOrDefault(DateTime.MinValue).Second,
                    dt.GetValueOrDefault(DateTime.MinValue).Millisecond);
            }
        }
        public int SelectedOperationKind { get; set; } = 0;
        public RelayCommand SaveNewOperationCommand { get; }

        public bool CanSaveNewOperation()
        {
            return true;
        }
        private async void SaveNewOperation()
        {
            NewOperation.Type = SelectedOperationType == 0 ? true : false;
            NewOperation.OperationCategory = OperationKinds[SelectedOperationKind];
            using (AppDbContext dbContext = new AppDbContext())
            {
                var affectedRows = await dbContext.AddOperationAsync(NewOperation);
                if (affectedRows > 0)
                {
                    MoneyOperations = await dbContext.GetOperationsAsync(50);
                    UpdateBalance();
                }
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
            IncCommand = new RelayCommand(OnIncCommandExecuting, CanIncCommandExecute);
        }
    }
}
