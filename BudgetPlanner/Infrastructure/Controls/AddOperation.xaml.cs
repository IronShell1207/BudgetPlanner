using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
    public sealed partial class AddOperation : UserControl
    {
        
        private MainVM viewModel => AppShell.Current.MViewModel;
        public AddOperation()
        {
            this.InitializeComponent();
        }


        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((sender as ComboBox).SelectedItem as string) == OperationsCategories.OperationType.First())
            {
                viewModel.OperationKinds = OperationsCategories.RecievedCategories;
                OperationKindComboBox.ItemsSource = viewModel.OperationKinds;
            }
            else
            {
                viewModel.OperationKinds = OperationsCategories.SpendCategories;
                OperationKindComboBox.ItemsSource = viewModel.OperationKinds;
            }
        }

        private void SumTextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
           
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void SumTextBox_OnLosingFocus(UIElement sender, LosingFocusEventArgs args)
        {   
            if (string.IsNullOrWhiteSpace((sender as TextBox).Text)) (sender as TextBox).Text = "0";
        }
    }
}
