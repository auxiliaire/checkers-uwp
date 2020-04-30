using Checkers.Components;
using Checkers.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Checkers
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Cell SourceCell;
        private Cell TargetCell;

        private readonly MediaPlayer MediaPlayer = new MediaPlayer();

        public static readonly DependencyProperty MoveHistoryProperty =
            DependencyProperty.Register(
                "MoveHistory",
                typeof(ObservableCollection<Move>),
                typeof(MainPage),
                PropertyMetadata.Create(new CreateDefaultValueCallback(() => { return new ObservableCollection<Move>(); }))
            );
        public ObservableCollection<Move> MoveHistory
        {
            get { return (ObservableCollection<Move>)GetValue(MoveHistoryProperty); }
            set { SetValue(MoveHistoryProperty, value); }
        }

        public static readonly DependencyProperty UndoHistoryProperty =
            DependencyProperty.Register(
                "UndoHistory",
                typeof(ObservableCollection<Move>),
                typeof(MainPage),
                PropertyMetadata.Create(new CreateDefaultValueCallback(() => { return new ObservableCollection<Move>(); }))
            );
        public ObservableCollection<Move> UndoHistory
        {
            get { return (ObservableCollection<Move>)GetValue(UndoHistoryProperty); }
            set { SetValue(UndoHistoryProperty, value); }
        }

        public MainPage()
        {
            this.InitializeComponent();

            MoveHistory = new ObservableCollection<Move>();
            UndoHistory = new ObservableCollection<Move>();

            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            TitleBar.Height = coreTitleBar.Height;
            Window.Current.SetTitleBar(MainTitleBar);
            Window.Current.Activated += Current_Activated;
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

            BuildBoard();
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

        private void BallClick(object sender, RoutedEventArgs e)
        {
            if (SourceCell == null)
            {
                SourceCell = (Cell)sender;
                SourceCell.Selected = true;
            }
            else
            {
                TargetCell = (Cell)sender;
                if (BoardGrid.IsValidMove(SourceCell, TargetCell))
                {
                    SourceCell.Filled = false;
                    MakeMove();
                }
                else
                {
                    SourceCell.Selected = false;
                    if (SourceCell.Position == TargetCell.Position)
                    {
                        SourceCell = null;
                    }
                    else
                    {
                        SourceCell = TargetCell;
                        SourceCell.Selected = true;
                    }
                    TargetCell = null;
                }
            }
        }

        private void BallDragEnter(object sender, DragEventArgs e)
        {
            e.DragUIOverride.IsGlyphVisible = false;
            e.DragUIOverride.IsCaptionVisible = false;

            TargetCell = (Cell)sender;
            if (BoardGrid.IsValidMove(SourceCell, TargetCell))
            {
                e.AcceptedOperation = DataPackageOperation.Move;
            }
            else
            {
                e.AcceptedOperation = DataPackageOperation.None;
            }
        }

        private void BallDrop(object sender, DragEventArgs e)
        {
            MakeMove();
        }

        private void MakeMove()
        {
            SaveMove(SourceCell, TargetCell);

            Play("ms-appx:///Assets/Audio/short_pop_sound.mp3");
            TargetCell.Selected = false;
            TargetCell.Filled = true;
            BoardGrid.GetMiddleCell(SourceCell, TargetCell).Filled = false;
            Untrack();
            CheckGameOver();
        }

        private void SaveMove(Cell source, Cell target)
        {
            MoveHistory.Add(new Move(source.Position, target.Position));
            UndoHistory.Clear();
        }

        private void CheckGameOver()
        {
            BoardGrid.CheckGameOver((int countFilled) =>
            {
                Play("ms-appx:///Assets/Audio/finale.mp3");
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(ResultPage), countFilled, new DrillInNavigationTransitionInfo());
            });
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.GoBack(new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Untrack();
            ClearHistory();
            BoardGrid.Children.Clear();
            BuildBoard();
        }

        private void BuildBoard()
        {
            Play("ms-appx:///Assets/Audio/flip.mp3");
            foreach (Logic.Plan.Component planComponent in Engine.Parse(Engine.PLAN))
            {
                Cell cell = planComponent.ToCell();
                cell.Click += BallClick;
                cell.ContentDragStarting += (s, e) => { SourceCell = (Cell)s; };
                cell.ContentDragEnter += BallDragEnter;
                cell.ContentDrop += BallDrop;
                BoardGrid.Children.Add(cell);
            }
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            if (MoveHistory.Count < 1) return;

            Untrack();

            Move lastMove = MoveHistory.Last();

            Cell target = BoardGrid.GetCellAt(lastMove.Target);
            target.Filled = false;
            Cell source = BoardGrid.GetCellAt(lastMove.Source);
            Cell middle = BoardGrid.GetMiddleCell(source, target);
            middle.Selected = false;
            middle.Filled = true;
            source.Selected = false;
            source.Filled = true;

            UndoHistory.Add(lastMove);
            MoveHistory.RemoveAt(MoveHistory.Count - 1);
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            if (UndoHistory.Count < 1) return;

            Untrack();

            Move lastMove = UndoHistory.Last();

            Cell target = BoardGrid.GetCellAt(lastMove.Target);
            target.Selected = false;
            target.Filled = true;
            Cell source = BoardGrid.GetCellAt(lastMove.Source);
            Cell middle = BoardGrid.GetMiddleCell(source, target);
            middle.Filled = false;
            source.Filled = false;

            MoveHistory.Add(lastMove);
            UndoHistory.RemoveAt(UndoHistory.Count - 1);
        }

        private void Untrack()
        {
            SourceCell = null;
            TargetCell = null;
        }

        private void ClearHistory()
        {
            MoveHistory.Clear();
            UndoHistory.Clear();
        }

        private void Play(string fileName)
        {
            MediaPlayer.Pause();
            MediaPlayer.Source = MediaSource.CreateFromUri(new Uri($"{fileName}", UriKind.RelativeOrAbsolute));
            MediaPlayer.Play();
        }
    }
}
