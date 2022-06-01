using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BudgetPlanner.Infrastructure.ViewModels;
using BudgetPlanner.Objects;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BudgetPlanner.Infrastructure.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OperationsPage : Page
    {
        public MainVM ViewModel => AppShell.Current.MViewModel;
        public OperationsPage()
        {
            this.InitializeComponent();
        }
        private async void OperationsPage_OnLoading(FrameworkElement sender, object args)
        {
                ViewModel.DataUpdaterService(999);
            //using (AppDbContext dbContext = new AppDbContext())
            //{
                
            //  var list = await dbContext.GetOperationsAsync(990);
            //  ViewModel.MoneyOperations = new ObsCollection<MoneyOperations>(list);
            //}
        }

        public List<string> DataIntervals => new List<string>()
        {
            "За все время",
            "За сегодня",
            "За три дня",
            "За неделю",
            "За месяц"
        };
        public List<string> SortOrder => new List<string>()
        {
            "Id",
            "Сумме",
            "Типу операции",
            "Виду операции",
            "Дате",
            "Комментарию"
        };

        private List<string> SortOrderByTag = new List<string>()
        {
            "Id","Sum","Type", "OperationCategory","DateTime","Comment" 
        };
        public bool IsSortDescending { get; set; } = true;
        public int SelectedSortingIndex { get; set; } = 4;
        public int SelectedIntervalIndex { get; set; } = 0;
        private void ComboInterval_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var isDescending = IsSortDescending ? "DESC" : "";
            switch (SelectedIntervalIndex)
            {
                case 0:
                    ViewModel.DataUpdaterService(999,$"ORDER BY {SortOrderByTag[SelectedSortingIndex]} {isDescending}");
                    break;
                case 1:
                    ViewModel.DiplayDataByTimeFrame(DateTime.Now, DateTime.Now+TimeSpan.FromDays(1), $"ORDER BY {SortOrderByTag[SelectedSortingIndex]} {isDescending}" );
                    break;
                case 2:
                    ViewModel.DiplayDataByTimeFrame(DateTime.Now - TimeSpan.FromDays(2), DateTime.Now + TimeSpan.FromDays(1), $"ORDER BY {SortOrderByTag[SelectedSortingIndex]} {isDescending}");
                    break;
                case 3:
                    ViewModel.DiplayDataByTimeFrame(DateTime.Now - TimeSpan.FromDays(6), DateTime.Now + TimeSpan.FromDays(1), $"ORDER BY {SortOrderByTag[SelectedSortingIndex]} {isDescending}");
                    break;
                case 4:
                    ViewModel.DiplayDataByTimeFrame(DateTime.Now - TimeSpan.FromDays(30), DateTime.Now + TimeSpan.FromDays(1), $"ORDER BY {SortOrderByTag[SelectedSortingIndex]} {isDescending}");
                    break;
                default:
                    ViewModel.DataUpdaterService(999, $"ORDER BY {SortOrderByTag[SelectedSortingIndex]} {isDescending}");
                    break;

            }
            
        }

        private void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            ComboInterval_OnSelectionChanged(null,null);
        }
    }
}
