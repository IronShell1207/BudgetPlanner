using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        private void LinkTextBox_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            var keysMass = new List<VirtualKey>()
            {
                VirtualKey.Number0,
                VirtualKey.Number1,
                VirtualKey.Number2,
                VirtualKey.Number3,
                VirtualKey.Number4,
                VirtualKey.Number5,
                VirtualKey.Number6,
                VirtualKey.Number7,
                VirtualKey.Number8,
                VirtualKey.Number9,
                VirtualKey.Decimal,
                VirtualKey.Final
            };
            e.Handled = keysMass.Contains(e.Key);
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.OnPropertyChanged(nameof(viewModel.OperationKind));
        }
    }
}
