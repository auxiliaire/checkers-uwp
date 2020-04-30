using Checkers.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.UI.Notifications;
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
    public sealed partial class Cell : UserControl
    {
        public static readonly DependencyProperty FilledProperty =
            DependencyProperty.Register(
            "Filled", typeof(Boolean),
            typeof(Cell),
            PropertyMetadata.Create(false)
            );
        public bool Filled
        {
            get { return (bool)GetValue(FilledProperty); }
            set { SetValue(FilledProperty, value); }
        }

        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register(
            "Selected", typeof(Boolean),
            typeof(Cell),
            PropertyMetadata.Create(false)
            );
        public bool Selected
        {
            get { return (bool)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        public Position Position { get => new Position((int)GetValue(Grid.RowProperty), (int)GetValue(Grid.ColumnProperty)); }

        public event RoutedEventHandler Click;
        public event TypedEventHandler<UIElement, DragStartingEventArgs> ContentDragStarting;
        public event DragEventHandler ContentDragEnter;
        public event DragEventHandler ContentDrop;

        public Cell()
        {
            this.InitializeComponent();
            CellButton.Click += (s, e) => Click?.Invoke(this, e);
            CellButton.DragEnter += (s, e) => ContentDragEnter?.Invoke(this, e);
            CellButton.Drop += (s, e) => ContentDrop?.Invoke(this, e);
            BallImage.PointerReleased += (s, e) => Click?.Invoke(this, e);
            BallImage.DragStarting += (s, e) => ContentDragStarting?.Invoke(this, e);
        }

        private async void BallDragStartingAsync(UIElement sender, DragStartingEventArgs args)
        {
            var deferral = args.GetDeferral();
            var rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(sender);
            var buffer = await rtb.GetPixelsAsync();
            var bitmap = SoftwareBitmap.CreateCopyFromBuffer(buffer,
                BitmapPixelFormat.Bgra8,
                rtb.PixelWidth,
                rtb.PixelHeight,
                BitmapAlphaMode.Premultiplied);

            args.Data.SetText(((Image)sender).Name);
            args.DragUI.SetContentFromSoftwareBitmap(bitmap);

            Filled = false;

            deferral.Complete();
        }

        private void BallDropCompleted(UIElement sender, DropCompletedEventArgs args)
        {
            if (args.DropResult != DataPackageOperation.Move)
            {
                Filled = true;
            }
        }

        public override string ToString()
        {
            Char content = Filled ? 'O' : '_';
            return $"Cell( {content} @ {Position.Row} : {Position.Col} )";
        }
    }
}
