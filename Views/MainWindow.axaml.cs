using System.Globalization;
using AllahIsWatchingMe.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace AllahIsWatchingMe.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainText.FontSize = DataBaseService.Current.Preferences!.FontSize ?? 25;
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
            }
            else
            {
                if (MainText.FontSize <= 25) return;
                MainText.FontSize -= 1;
            }
            DataBaseService.Current.Preferences!.FontSize = int.Parse(MainText.FontSize.ToString(CultureInfo.CurrentCulture));
            DataBaseService.Current!.UpdatePref();
        });
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