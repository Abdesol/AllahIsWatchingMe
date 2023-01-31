using Avalonia;
using Newtonsoft.Json;

namespace AllahIsWatchingMe.Models;

public class PrefModel
{
    /// <summary>
    /// Size of the window
    /// </summary>
    [JsonProperty]
    public double? FontSize { get; set; }
    
    /// <summary>
    /// Position of the window
    /// </summary>
    [JsonProperty]
    public PixelPoint? Position { get; set; }
}