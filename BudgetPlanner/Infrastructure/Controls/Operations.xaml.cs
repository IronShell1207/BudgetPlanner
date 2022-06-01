using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BudgetPlanner.Infrastructure.Controls
{
    public sealed partial class Operations : UserControl
    {
        #region ListOperations : ObservableCollection<MoneyOperations> - List of operations

        /// <summary>List of operations</summary>
        public static readonly DependencyProperty ListOperationsProperty =
            DependencyProperty.Register(
                nameof(ListOperations),
                typeof(ObservableCollection<MoneyOperations>),
                typeof(Operations),
                new PropertyMetadata(default(ObservableCollection<MoneyOperations>)));

        public ObservableCollection<MoneyOperations> ListOperations
        {
            get { return (ObservableCollection<MoneyOperations>) GetValue(ListOperationsProperty); }
            set { SetValue(ListOperationsProperty, value); }    
        }

        #endregion

        public Operations()
        {
            this.InitializeComponent();
            
        }

        private void DataTable_Sorting(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridColumnEventArgs e)
        {

        }

        private void Operations_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataTable.ItemsSource = ListOperations;
        }
    }
}
