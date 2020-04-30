using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Checkers.Components
{
    public sealed partial class Rating : UserControl
    {
        public static readonly DependencyProperty StarsProperty =
            DependencyProperty.Register(
            "Stars", typeof(int),
            typeof(Rating),
            PropertyMetadata.Create(0)
            );
        public int Stars
        {
            get { return (int)GetValue(StarsProperty); }
            set { SetValue(StarsProperty, value); }
        }

        public Rating()
        {
            this.InitializeComponent();
        }
    }
}
