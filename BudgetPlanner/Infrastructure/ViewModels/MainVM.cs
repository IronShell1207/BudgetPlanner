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

        public RelayCommand EditOperationCommand { get; }

        private async void EditOperation(object param)
        {
            var editableOperation = param as MoneyOperation;
            if (editableOperation.Sum != 0)
            {
                AppDbContext dbContext = new AppDbContext();
                var affectedRows = await dbContext.EditOperationAsync(editableOperation);
                dbContext.Dispose();
                if (affectedRows > 0)
                {
                    var result = await ShowDialog("Успех", "Данные успешно отредактированы!");
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
        public RelayCommand SaveNewOperationCommand { get; }

        private async void SaveNewOperation(object param)
        {
            var opetation = param as MoneyOperation;
            if (opetation.Sum != 0)
            {
               
                AppDbContext dbContext = new AppDbContext();

                var affectedRows = await dbContext.AddOperationAsync(opetation);

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

        public MainVM()
        {
            SaveNewOperationCommand = new RelayCommand(SaveNewOperation);
            EditOperationCommand = new RelayCommand(EditOperation);
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
