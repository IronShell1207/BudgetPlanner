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
using BudgetPlanner.Infrastructure.Controls;
using BudgetPlanner.Infrastructure.Pages;
using BudgetPlanner.Infrastructure.ViewModels;
using BudgetPlanner.Objects;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BudgetPlanner
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppShell : Page
    {
        public static AppShell Current = null;
        public MainVM MViewModel => this.DataContext as MainVM;
        public Frame AppFrame => AppShellFrame;
        public AppShell()
        {
            this.InitializeComponent();
            Current = this;
            MenuCList.SelectedIndex = 0;
        }

        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
        }

        private void OnNavigatedToPage(object sender, NavigationEventArgs e)
        {

        }

        public List<NavMenuItem> MenuList { get; } = new List<NavMenuItem>(new[]
        {
            new NavMenuItem()
            {
                Symbol = Symbol.Home,
                Label = "Главная",
                DestPage = typeof(HomePage)
            },
            new NavMenuItem()
            {
                Symbol = Symbol.DockBottom,
                Label = "Операции",
            },

            new NavMenuItem()
            {
                Symbol = Symbol.Setting,
                Label = "Настройки"

            },

        });
        public List<NavMenuItem> ActsList { get; } = new List<NavMenuItem>(new[]
        {
            new NavMenuItem()
            {
                Symbol = Symbol.Add,
                Label = "Добавить операцию",
               // DestPage = typeof(MasterDetailPage),
               // Arguments = typeof(AddFeedView)
            },
            new NavMenuItem()
            {
                Symbol = Symbol.Edit,
                Label = "Отредактировать операцию",
                //DestPage = typeof(MasterDetailPage),
                //Arguments = typeof(EditFeedsView)
            }
        });

        public Rect TogglePaneButtonRect { get; private set; }

        /// <summary>
        /// An event to notify listeners when the hamburger button may occlude other content in the app.
        /// The custom "PageHeader" user control is using this.
        /// </summary>
        public event TypedEventHandler<AppShell, Rect> TogglePaneButtonRectChanged;

        /// <summary>
        /// Check for the conditions where the navigation pane does not occupy the space under the floating
        /// hamburger button and trigger the event.
        /// </summary>
        private void CheckTogglePaneButtonSizeChanged()
        {
            TogglePaneButtonRect =
                RootSplitView.DisplayMode == SplitViewDisplayMode.Inline ||
                RootSplitView.DisplayMode == SplitViewDisplayMode.Overlay
                    ? TogglePaneButton.TransformToVisual(this).TransformBounds(
                        new Rect(0, 0, TogglePaneButton.ActualWidth, TogglePaneButton.ActualHeight))
                    : new Rect();
            TogglePaneButtonRectChanged?.Invoke(this, this.TogglePaneButtonRect);
        }

        /// <summary>
        /// Navigate to the Page for the selected <paramref name="listViewItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="listViewItem"></param>
        private void MenuList_ItemInvoked(object sender, ListViewItem listViewItem)
        {
            ActionsMenuList.SelectedIndex = -1;
            var item = (NavMenuItem)((NavMenuListView)sender).ItemFromContainer(listViewItem);
            if (item != null)
            {
                if (item.DestPage == typeof(HomePage))
                    AppShellFrame.Navigate(typeof(HomePage), null);
            }
        }

        private void ActionsMenuList_OnItemInvoked(object sender, ListViewItem e)
        {
            MenuCList.SelectedIndex = -1;
        }
    }
}
