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
using BudgetPlanner.Objects;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BudgetPlanner.Infrastructure.Controls
{
    public sealed partial class Operations : UserControl
    {
        #region OperationsList : IEnumerable<MoneyOperation> - List of money operations

        /// <summary>List of money operations</summary>
        public static readonly DependencyProperty OperationsListProperty =
            DependencyProperty.Register(
                nameof(OperationsList),
                typeof(IEnumerable<MoneyOperations>),
                typeof(Operations),
                new PropertyMetadata(default(IEnumerable<MoneyOperations>)));

        public IEnumerable<MoneyOperations> OperationsList
        {
            get { return (IEnumerable<MoneyOperations>) GetValue(OperationsListProperty); }
            set { SetValue(OperationsListProperty, value); }
        }

        #endregion
        public Operations()
        {
            this.InitializeComponent();
        }

        private void DataTable_Sorting(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridColumnEventArgs e)
        {

        }
    }
}
