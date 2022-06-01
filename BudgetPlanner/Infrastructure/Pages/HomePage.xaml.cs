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
using BudgetPlanner.Infrastructure.ViewModels.Base;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Infrastructure.Pages  
{
    
    public sealed partial class HomePage : Page
    {
        public MainVM ViewModel => AppShell.Current.MViewModel;

        public HomePage()
        {
            this.InitializeComponent();
        }

        private async void HomePage_OnLoading(FrameworkElement sender, object args)
        {
            ViewModel.UpdateDataAsync(20);
        }
    }
}
