using Godot;

[GlobalClass]
public partial class SettingsManager : Node
{
    public static SettingsManager Instance { get; private set; }
    [Export] public SettingsData SettingsData { get; private set; }

    public delegate void SettingsChangedEventHandler();
    public event SettingsChangedEventHandler SettingsChanged;

    public override void _Ready()
    {
        Instance = this;
        SettingsData = GD.Load<SettingsData>("res://addons/BZSettingsSystem/Resources/SettingsData.tres");
        SettingsData.LoadSettings();
        SetDisplaySettings();
        base._Ready();
    }
    
    public void SaveSettings()
    {   
        SettingsData.SaveSettings();
        // can never be null
        SettingsChanged.Invoke();
    }

    /// <summary>
    /// Applies the display settings from SettingsData to the engine.
    /// </summary>
    public void SetDisplaySettings()
    {
        DisplayServer.WindowSetMode(SettingsData.DisplayMode);
        DisplayServer.WindowSetSize(SettingsData.Resolution);
        DisplayServer.WindowSetVsyncMode(SettingsData.VSync ? DisplayServer.VSyncMode.Enabled : DisplayServer.VSyncMode.Disabled);
        // TargetFrameRate + 1 to account for 59.98 like offsets.
        Engine.MaxFps = SettingsData.TargetFrameRate + 1;
        SetColorblindModeShader(SettingsData.ColorBlindMode);
    }

    public static string GetIndexFromWindowMode(DisplayServer.WindowMode mode)
    {
        return mode switch
        {
            DisplayServer.WindowMode.ExclusiveFullscreen => "0",
            DisplayServer.WindowMode.Fullscreen => "1",
            DisplayServer.WindowMode.Windowed => "2",
            _ => "0",
        };
    }

    public static DisplayServer.WindowMode GetWindowModeFromIndex(string index)
    {
        return index switch
        {
            "0" => DisplayServer.WindowMode.ExclusiveFullscreen,
            "1" => DisplayServer.WindowMode.Fullscreen,
            "2" => DisplayServer.WindowMode.Windowed,
            _ => DisplayServer.WindowMode.ExclusiveFullscreen,
        };
    }

    public void SetColorblindModeShader(ColorBlindMode mode)
    {
        // choose shader based on mode
    }
}
