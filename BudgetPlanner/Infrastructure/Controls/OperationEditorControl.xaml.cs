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
using BudgetPlanner.Infrastructure.Converters;
using BudgetPlanner.Infrastructure.ViewModels;
using BudgetPlanner.Objects;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BudgetPlanner.Infrastructure.Controls
{
    public sealed partial class OperationEditorControl : UserControl
    {
        public MainVM viewModel => AppShell.Current.MViewModel;
        #region OpData : MoneyOperation - Принимает класс MoneyOperation

        /// <summary>Принимает класс MoneyOperation</summary>
        public static readonly DependencyProperty OpDataProperty =
            DependencyProperty.Register(
                nameof(OpData),
                typeof(MoneyOperation),
                typeof(OperationEditorControl),
                new PropertyMetadata(default(MoneyOperation)));

        public MoneyOperation OpData
        {
            get { return (MoneyOperation) GetValue(OpDataProperty); }
            set { SetValue(OpDataProperty, value); }
        }

        #endregion
        public OperationEditorControl()
        {
            this.InitializeComponent();
        }
        #region ButtonCommand
        public static readonly DependencyProperty ButtonClickedCommandProperty =
            DependencyProperty.Register("ButtonClickedCommand", 
                typeof(System.Windows.Input.ICommand), 
                typeof(OperationEditorControl), 
                new PropertyMetadata(null));

        public System.Windows.Input.ICommand ButtonClickedCommand
        {
            get
            {
                return (System.Windows.Input.ICommand)GetValue(ButtonClickedCommandProperty);
            }
            set
            {
                SetValue(ButtonClickedCommandProperty, value);
            }
        }
        #endregion

        public DateTimeOffset Date
        {
            get
            {
                DateTimeOffset? ts = DateTimeConverter.DateTimeToDateTimeOffSet(OpData.DateTime);
                return ts.GetValueOrDefault(DateTimeOffset.MinValue);
            }
            set
            {
                OpData.DateTime = new DateTime(value.Year,
                    value.Month,
                    value.Day,
                    OpData.DateTime.Hour,
                    OpData.DateTime.Minute,
                    OpData.DateTime.Second,
                    OpData.DateTime.Millisecond);
            }
        }

        public TimeSpan Time
        {
            get
            {
                TimeSpan? ts = DateTimeConverter.DateTimeToTimeSpan(OpData.DateTime);
                return ts.GetValueOrDefault(TimeSpan.MinValue);
            }
            set
            {
                DateTime? dt = DateTimeConverter.TimeSpanToDateTime(value);

                OpData.DateTime = new DateTime(OpData.DateTime.Year,
                    OpData.DateTime.Month,
                    OpData.DateTime.Day,
                    dt.GetValueOrDefault(DateTime.MinValue).Hour,
                    dt.GetValueOrDefault(DateTime.MinValue).Minute,
                    dt.GetValueOrDefault(DateTime.MinValue).Second,
                    dt.GetValueOrDefault(DateTime.MinValue).Millisecond);
            }
        }
        public List<string> OperationsTypes => OperationsCategories.OperationType;
        public List<string> OperationKinds { get; set; } = OperationsCategories.RecievedCategories;

        private void OpTypeCB_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedOperationType == 0)
            {
                OperationKinds = OperationsCategories.RecievedCategories;
                OperationKindComboBox.ItemsSource = OperationKinds;
                OpData.Type = true;
            }
            else
            {
                OperationKinds = OperationsCategories.SpendCategories;
                OperationKindComboBox.ItemsSource = OperationKinds;
                OpData.Type = false;
            }
        }
        public int SelectedOperationType { get; set; } = 0;
        public int SelectedOperationKind { get; set; } = 0;

        private void SumTextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
           
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void SumTextBox_OnLosingFocus(UIElement sender, LosingFocusEventArgs args)
        {   
            if (string.IsNullOrWhiteSpace((sender as TextBox).Text)) (sender as TextBox).Text = "0";
        }

        private void OperationKindComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedOperationKind>-1)
                OpData.OperationCategory = OperationKinds[SelectedOperationKind];
        }
    }
}
