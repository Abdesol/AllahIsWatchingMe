using System.Reflection;
using System.Runtime.InteropServices;
using AllahIsWatchingMe.ViewModels;
using AllahIsWatchingMe.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Win32;

namespace AllahIsWatchingMe;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        SetAppToStartUp();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    private static void SetAppToStartUp()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // check if the app is already set to start up
            var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            var value = key!.GetValue("Allah Is Watching Me");
            if (value != null) return;
            
            // set the app to start up if it is not set
            var assemblyLocation = Assembly.GetExecutingAssembly().Location.Replace(".dll", ".exe");
            key.SetValue("Allah Is Watching Me", assemblyLocation);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            // TODO: Add macOS support
        }
    }
}