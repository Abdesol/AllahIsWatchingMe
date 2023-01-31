using System;
using System.Globalization;
using System.Threading;
using AllahIsWatchingMe.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;

namespace AllahIsWatchingMe.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainText.FontSize = DataBaseService.Current.Preferences!.FontSize ?? 25;
        MainImage.Height = DataBaseService.Current.Preferences!.FontSize + 30 ?? 55;
        MainImage.Width = DataBaseService.Current.Preferences!.FontSize + 30 ?? 55;
        if (DataBaseService.Current.Preferences.Position == null)
        {
            var primaryMonitor = Screens.Primary;
            Position = new PixelPoint(primaryMonitor!.Bounds.Width - 15, primaryMonitor.Bounds.Height - 15);
            DataBaseService.Current.Preferences.Position = Position;
            DataBaseService.Current!.UpdatePref();
        }
        else
        {
            Position = (PixelPoint)DataBaseService.Current.Preferences.Position;
        }
        
        AddHandler(PointerWheelChangedEvent, (_, e) =>
        {
            if (e.KeyModifiers != KeyModifiers.Control) return;
            if (e.Delta.Y > 0)
            {
                if (MainText.FontSize >= 150) return;
                MainText.FontSize += 1;
                MainImage.Height += 2;
                MainImage.Width += 2;
            }
            else
            {
                if (MainText.FontSize <= 25) return;
                MainText.FontSize -= 1;
                if (MainImage.Height > 57)
                {
                    MainImage.Height -= 2;
                    MainImage.Width -= 2;
                }
            }

            DataBaseService.Current.Preferences!.FontSize = MainText.FontSize;
            DataBaseService.Current!.UpdatePref();
        });
        
        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromHours(1)
        };
        timer.Tick += (_, _) =>
        {
            var i = 0;
            while (i <= 30)
            {
                Position = i <= 15 ? new PixelPoint(Position.X - 1, Position.Y) : new PixelPoint(Position.X + 1, Position.Y);
                Thread.Sleep(TimeSpan.FromSeconds(0.1));
                i++;
            }
        };
        timer.Start();

    }
    private void ChangeWindowPosition(object sender, PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        BeginMoveDrag(e);
        OnWindowPositionChanged();
    }
    private void OnWindowPositionChanged()
    {
        DataBaseService.Current.Preferences!.Position = new PixelPoint(Position.X, Position.Y);
        DataBaseService.Current!.UpdatePref();
    }
}