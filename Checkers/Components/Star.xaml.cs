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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Checkers.Components
{
    public sealed partial class Star : UserControl
    {
        public static readonly DependencyProperty FilledProperty =
            DependencyProperty.Register(
            "Filled", typeof(Boolean),
            typeof(Star),
            PropertyMetadata.Create(false)
            );
        public bool Filled
        {
            get { return (bool)GetValue(FilledProperty); }
            set { SetValue(FilledProperty, value); }
        }

        public Star()
        {
            this.InitializeComponent();
        }
    }
}
