using System;
using System.IO;
using AllahIsWatchingMe.Models;
using Avalonia;
using Newtonsoft.Json;

namespace AllahIsWatchingMe.Services;

public class DataBaseService
{
    public static DataBaseService Current { get; } = new();

    private DataBaseService()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var path = Path.Combine(appDataPath, "Allah Is Watching Me"); 
        Directory.CreateDirectory(path);
        
        PrefPath = Path.Combine(path, "pref.json");
        if (File.Exists(PrefPath))
        {
            var json = File.ReadAllText(PrefPath);
            Preferences = JsonConvert.DeserializeObject<PrefModel>(json)!;
            if (Preferences == null)
            {
                File.Delete(PrefPath);
                Preferences = new PrefModel()
                {
                    FontSize = 25, Position = new PixelPoint(15,15)
                };
                UpdatePref();
            }
            else
            {
                Preferences.FontSize ??= 25;
                Preferences.Position ??= new PixelPoint(15,15);
                UpdatePref();
            }
        }
        else
        {
            Preferences = new PrefModel()
            {
                FontSize = 25, Position = new PixelPoint(15,15)
            };
            UpdatePref();
        }
    }
    
    private string PrefPath { get; set; }
    public PrefModel? Preferences { get; set; }
    
    /// <summary>
    /// A preference updater
    /// </summary>
    public void UpdatePref()
    {
        var json = JsonConvert.SerializeObject(Preferences);
        File.WriteAllText(PrefPath, json);
    }
}