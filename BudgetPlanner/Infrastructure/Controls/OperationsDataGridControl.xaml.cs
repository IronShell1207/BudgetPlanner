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
    public sealed partial class OperationsDataGridControl : UserControl
    {
        #region ListOperations : ObsCollection<MoneyOperations> - List of operations

        /// <summary>List of operations</summary>
        public static readonly DependencyProperty ListOperationsProperty =
            DependencyProperty.Register(
                nameof(ListOperations),
                typeof(ObsCollection<MoneyOperation>),
                typeof(OperationsDataGridControl),
                new PropertyMetadata(default(ObsCollection<MoneyOperation>)));

        public ObsCollection<MoneyOperation> ListOperations
        {
            get { return (ObsCollection<MoneyOperation>) GetValue(ListOperationsProperty); }
            set 
            { 
                SetValue(ListOperationsProperty, value);
               // DataTable.ItemsSource = ListOperations;
            }    
        }

        #endregion

        #region Title : string - Title of the data table

        /// <summary>Title of the data table</summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(OperationsDataGridControl),
                new PropertyMetadata(default(string)));

        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        #endregion

        #region SelectedItemInRow : MoneyOperation - Selected row in the table

        /// <summary>Selected row in the table</summary>
        public static readonly DependencyProperty SelectedItemInRowProperty =
            DependencyProperty.Register(
                nameof(SelectedItemInRow),
                typeof(MoneyOperation),
                typeof(OperationsDataGridControl),
                new PropertyMetadata(default(MoneyOperation)));

        public MoneyOperation SelectedItemInRow
        {
            get { return (MoneyOperation) GetValue(SelectedItemInRowProperty); }
            set { SetValue(SelectedItemInRowProperty, value); }
        }

        #endregion
        public OperationsDataGridControl()
        {
            this.InitializeComponent();
        }

        private void DataTable_Sorting(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridColumnEventArgs e)
        {

        }

        private void Operations_OnLoaded(object sender, RoutedEventArgs e)
        {
            //DataTable.ItemsSource = ListOperations;
        }
    }
}
