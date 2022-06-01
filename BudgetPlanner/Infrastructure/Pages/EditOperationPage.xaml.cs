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
    public sealed partial class EditOperationPage : Page
    {
        public MainVM ViewModel => AppShell.Current.MViewModel;
        public EditOperationPage()
        {
            this.InitializeComponent();
        }

        public List<string> ListOperationsStrings
        {
            get
            {
                var list = new List<string>();
                if (ViewModel.MoneyOperations.Any())
                    foreach (var operation in ViewModel.MoneyOperations)
                        list.Add(operation.ToString());
                return list;
            }
        }

        private void EditOperationPage_OnLoading(FrameworkElement sender, object args)
        {
            ViewModel.UpdateDataAsync(999);
        }

        public int SelectedIndexCombo { get; set; } = 0;
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedIndexCombo != -1)
                EditorControl.OpData = ViewModel.MoneyOperations[SelectedIndexCombo];
        }
    }
}
