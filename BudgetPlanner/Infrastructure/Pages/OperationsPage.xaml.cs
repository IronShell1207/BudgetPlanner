﻿using System;
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
            using (AppDbContext dbContext = new AppDbContext())
            {
                
              var list = await dbContext.GetOperationsAsync(50);
              ViewModel.MoneyOperations = list;
            }
        }
    }
}
