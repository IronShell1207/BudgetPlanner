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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BudgetPlanner.Infrastructure.Controls
{
    public sealed partial class AccountInfo : UserControl
    {
        #region UserName : string - User name

        /// <summary>User name</summary>
        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register(
                nameof(UserName),
                typeof(string),
                typeof(AccountInfo),
                new PropertyMetadata(default(string)));

        public string UserName
        {
            get { return (string) GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }
            
        #endregion

        #region BalanceCurrent : double - Current balance of the user

        /// <summary>Current balance of the user</summary>
        public static readonly DependencyProperty BalanceCurrentProperty =
            DependencyProperty.Register(
                nameof(BalanceCurrent),
                typeof(double),
                typeof(AccountInfo),
                new PropertyMetadata(0.0));

        public double BalanceCurrent
        {
            get { return (double) GetValue(BalanceCurrentProperty); }
            set { SetValue(BalanceCurrentProperty, value); }
        }

        #endregion

        #region Currency : string - Currency symbol

        /// <summary>Currency symbol</summary>
        public static readonly DependencyProperty CurrencyProperty =
            DependencyProperty.Register(
                nameof(Currency),
                typeof(string),
                typeof(AccountInfo),
                new PropertyMetadata("$"));

        public string Currency
        {
            get { return (string) GetValue(CurrencyProperty); }
            set { SetValue(CurrencyProperty, value); }
        }

        #endregion

        #region TodayBalanceChange : double - Today balance change

        /// <summary>Today balance change</summary>
        public static readonly DependencyProperty TodayBalanceChangeProperty =
            DependencyProperty.Register(
                nameof(TodayBalanceChange),
                typeof(double),
                typeof(AccountInfo),
                new PropertyMetadata(default(double)));

        public double TodayBalanceChange
        {
            get { return (double) GetValue(TodayBalanceChangeProperty); }
            set { SetValue(TodayBalanceChangeProperty, value); }
        }

        #endregion

        public AccountInfo()
        {
            this.InitializeComponent();
        }
    }
}
