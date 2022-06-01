using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.PointOfService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BudgetPlanner.Infrastructure.Converters;
using BudgetPlanner.Infrastructure.Pages;
using BudgetPlanner.Infrastructure.ViewModels.Base;
using BudgetPlanner.Objects;
using Microsoft.Toolkit.Mvvm;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using SQLitePCL;

namespace BudgetPlanner.Infrastructure.ViewModels
{
    public class MainVM : ObservableObject
    {
        #region Properties

        public string UserName { get; set; } = "Username";

        public ObsCollection<MoneyOperation> MoneyOperations { get; set; } = new ObsCollection<MoneyOperation>()
        {
            new MoneyOperation()
            {
                Id = 0, Comment = "Покупка в ашане", DateTime = DateTime.Now,
                OperationCategory = OperationsCategories.RecievedCategories.First(), Sum = 230, Type = true
            },
            new MoneyOperation()
            {
                Id = 1, Comment = "Покупка в ашане", DateTime = DateTime.Now,
                OperationCategory = OperationsCategories.SpendCategories.First(), Sum = -330, Type = false
            },
            new MoneyOperation()
            {
                Id = 2, Comment = "Покупка в ашане", DateTime = DateTime.Now,
                OperationCategory = OperationsCategories.RecievedCategories[2], Sum = 230, Type = true
            },
            new MoneyOperation()
            {
                Id = 3, Comment = "Покупка в ашане", DateTime = DateTime.Now,
                OperationCategory = OperationsCategories.RecievedCategories[2], Sum = 230, Type = true
            },
            new MoneyOperation()
            {
                Id = 4, Comment = "Покупка в ашане", DateTime = DateTime.Now,
                OperationCategory = OperationsCategories.SpendCategories[3], Sum = -430, Type = false
            },
            new MoneyOperation()
            {
                Id = 5, Comment = "Покупка в ашане", DateTime = DateTime.Now,
                OperationCategory = OperationsCategories.RecievedCategories[2], Sum = 230, Type = true
            },
        };

        private double _balance = 0;

        public double Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                OnPropertyChanged(nameof(Balance));
            }
        }

        private double _todaysChange = 0;

        public double TodaysChange
        {
            get => _todaysChange;
            set { SetProperty(ref _todaysChange, value); }
        }

        public string Currency { get; set; } = "$";
        public List<string> OperationsTypes => OperationsCategories.OperationType;
        public List<string> OperationKinds { get; set; } = OperationsCategories.RecievedCategories;

        #endregion

        private async Task<ContentDialogResult> ShowDialog(string title, string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "Ok"
            };
            return await dialog.ShowAsync();
        }

        #region AddNewOperation

        private MoneyOperation _newOperation = new MoneyOperation();

        public MoneyOperation NewOperation
        {
            get => _newOperation;
            set
            {
                _newOperation = value;
                OnPropertyChanged(nameof(NewOperation));
            }
        }

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

        private async void SaveNewOperation(object param)
        {
            NewOperation.Type = SelectedOperationType == 0;
            if (SelectedOperationKind > -1 && NewOperation.Sum != 0)
            {
                NewOperation.OperationCategory = OperationKinds[SelectedOperationKind];
                AppDbContext dbContext = new AppDbContext();

                var affectedRows = await dbContext.AddOperationAsync(NewOperation);

                dbContext.Dispose();
                if (affectedRows > 0)
                {
                    var result = await ShowDialog("Успех", "Данные успешно сохранены в БД!");
                    UpdateBalance();
                    DataUpdaterService(20);
                }
            }
            else
            {
                await ShowDialog("Ошибка",
                    "Не удалось сохранить данные в БД, так как одно или несколько полей не заполнены!");
            }
        }

        #endregion

        public MainVM()
        {
            SaveNewOperationCommand = new RelayCommand(SaveNewOperation);
            UpdateBalance();
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

        public async void DataUpdaterService(int limit = 20, string orderBy = "ORDER BY DateTime DESC")
        {
            using (AppDbContext dbContext = new AppDbContext())
            {
                var list = await dbContext.GetOperationsAsync(limit,0, orderBy);
                MoneyOperations.Clear();
                foreach (var operation in list)
                {
                    MoneyOperations.Add(operation);
                }
                OnPropertyChanged(nameof(MoneyOperations));
            }
        }

        public async void DiplayDataByTimeFrame(DateTime dateFrom, DateTime dateTo, string sortOrder= "")
        {
            using (AppDbContext dbContext = new AppDbContext())
            {
                var list = await dbContext.GetDataByTimePeriod(dateFrom, dateTo, sortOrder);
                MoneyOperations.Clear();
                foreach (var operation in list)
                {
                    MoneyOperations.Add(operation);
                }
                OnPropertyChanged(nameof(MoneyOperations));
            }
        }

        public void UpdateBalance()
        {
            using (AppDbContext dbContext = new AppDbContext())
            {
                var allOperations = dbContext.GetAllMoneyMoves().Result;
                var todayOperations = dbContext.GetMoneyMovesByDate(DateTime.Now, DateTime.Now + TimeSpan.FromDays(1)).Result;
                Balance = 0;
                TodaysChange = 0;
                foreach (var operation in allOperations)
                    Balance += operation;
                foreach (var operation in todayOperations)
                    TodaysChange += operation;
            }
        }
    }
}
