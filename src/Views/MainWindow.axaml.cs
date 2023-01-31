using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using AllahIsWatchingMe.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform;
using Avalonia.Threading;
using Microsoft.Win32;
using ReactiveUI;

namespace AllahIsWatchingMe.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainText.FontSize = DataBaseService.Current.Preferences!.FontSize ?? 25;
        MainImage.Height = DataBaseService.Current.Preferences!.FontSize + 30 ?? 55;
        MainImage.Width = DataBaseService.Current.Preferences!.FontSize + 30 ?? 55;
        Position = (PixelPoint)DataBaseService.Current.Preferences.Position!;
        
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
            while (i <= 60)
            {
                Position = i switch
                {
                    <= 15 => new PixelPoint(Position.X - 1, Position.Y + 1),
                    <= 30 => new PixelPoint(Position.X - 1, Position.Y - 1),
                    <= 45 => new PixelPoint(Position.X + 1, Position.Y - 1),
                    _ => new PixelPoint(Position.X + 1, Position.Y + 1)
                };
                Thread.Sleep(TimeSpan.FromMilliseconds(50));
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