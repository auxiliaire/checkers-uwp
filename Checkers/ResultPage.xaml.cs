using Checkers.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Checkers
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ResultPage : Page
    {
        public static readonly DependencyProperty StarsProperty =
             DependencyProperty.Register(
             "Stars", typeof(int),
             typeof(ResultPage),
             PropertyMetadata.Create(0)
             );
        public int Stars
        {
            get { return (int)GetValue(StarsProperty); }
            set { SetValue(StarsProperty, value); }
        }

        public static readonly DependencyProperty RemainedProperty =
             DependencyProperty.Register(
             "Remained", typeof(int),
             typeof(ResultPage),
             PropertyMetadata.Create(0)
             );
        public int Remained
        {
            get { return (int)GetValue(RemainedProperty); }
            set { SetValue(RemainedProperty, value); }
        }

        public ResultPage()
        {
            this.InitializeComponent();

            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            TitleBar.Height = coreTitleBar.Height;
            Window.Current.SetTitleBar(MainTitleBar);
            Window.Current.Activated += Current_Activated;
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Remained = (int)e.Parameter;
            Stars = 4 - Remained;
            Message.Text = Engine.GetMessage(Remained);
        }

        private void Current_Activated(object sender, WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState != CoreWindowActivationState.Deactivated)
            {
                BackButtonGrid.Visibility = Visibility.Visible;
                MainTitleBar.Opacity = 1;
            }
            else
            {
                BackButtonGrid.Visibility = Visibility.Collapsed;
                MainTitleBar.Opacity = 0.5;
            }
        }

        void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar titleBar, object args)
        {
            TitleBar.Visibility = titleBar.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBar.Height = sender.Height;
            RightMask.Width = sender.SystemOverlayRightInset;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.GoBack(new DrillInNavigationTransitionInfo());
        }
    }
}
