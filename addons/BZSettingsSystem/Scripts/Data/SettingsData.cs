using Godot;
using BZ.Utilities.ConfigHandler;

public partial class SettingsData : Resource
{
    [ExportCategory("Display Settings")]
    [Export] public DisplayServer.WindowMode DisplayMode { get; set; } = DisplayServer.WindowMode.ExclusiveFullscreen;
    [Export] public Vector2I Resolution { get; set; } = new Vector2I(1280, 720);
    [Export] public bool VSync { get; set; } = true;
    [Export] public int TargetFrameRate { get; set; } = 60;
    [Export] public bool ShowPerformanceOverlay { get; set; } = true;

    [ExportCategory("Audio Settings")]
    [Export] public float MasterVolume { get; set; } = 50f;
    [Export] public float MusicVolume { get; set; } = 100f;
    [Export] public float EffectsVolume { get; set; } = 100f;

    [ExportCategory("Accessibility Settings")]
    [Export] public ColorBlindMode ColorBlindMode { get; set; } = ColorBlindMode.None;

    /// <summary>
    /// Parameterless Constructor for godot initialization.
    /// </summary>
    public SettingsData() { }

    public void ChangeDisplaySettings(DisplayServer.WindowMode displayMode, Vector2I resolution, bool vSync, int targetFrameRate, bool showPerformanceOverlay)
    {
        DisplayMode = displayMode;
        Resolution = resolution;
        VSync = vSync;
        TargetFrameRate = targetFrameRate;
        ShowPerformanceOverlay = showPerformanceOverlay;
    }

    public void ChangeAudioSettings(float masterVolume, float musicVolume, float effectsVolume)
    {
        MasterVolume = masterVolume;
        MusicVolume = musicVolume;
        EffectsVolume = effectsVolume;
    }

    public void ChangeAccessibilitySettings(ColorBlindMode colorBlindMode)
    {
        ColorBlindMode = colorBlindMode;
    }

    public void ChangeAllSettings(DisplayServer.WindowMode displayMode, Vector2I resolution, bool vSync, int targetFrameRate, bool showPerformanceOverlay,
        float masterVolume, float musicVolume, float effectsVolume, ColorBlindMode colorBlindMode)
    {
        ChangeDisplaySettings(displayMode, resolution, vSync, targetFrameRate, showPerformanceOverlay);
        ChangeAudioSettings(masterVolume, musicVolume, effectsVolume);
        ChangeAccessibilitySettings(colorBlindMode);
    }

    public void LoadSettings()
    {
        #if DEBUG
        GD.Print("Loading Settings.");
        #endif

        ConfigHandler.LoadConfigFile();

        // === Display Settings ===
        DisplayMode = ConfigHandler.GetDisplayMode();
        Resolution = ConfigHandler.GetResolution();
        VSync = ConfigHandler.GetVSync();
        TargetFrameRate = ConfigHandler.GetTargetFrameRate();
        ShowPerformanceOverlay = ConfigHandler.GetShowPerformanceOverlay();

        // === Audio Settings ===
        MasterVolume = ConfigHandler.GetMasterVolume();
        MusicVolume = ConfigHandler.GetMusicVolume();
        EffectsVolume = ConfigHandler.GetEffectsVolume();

        // === Accessibility Settings ===
        ColorBlindMode = ConfigHandler.GetColorblindMode();
    }

    public void SaveSettings()
    {
        #if DEBUG
        GD.Print("Saving Settings.");
        #endif

        // === Display Settings ===
        ConfigHandler.SetResolution(Resolution);
        ConfigHandler.SetDisplayMode(DisplayMode);
        ConfigHandler.SetVSync(VSync);
        ConfigHandler.SetTargetFrameRate(TargetFrameRate);
        ConfigHandler.SetShowPerformanceOverlay(ShowPerformanceOverlay);

        // === Audio Settings ===
        ConfigHandler.SetMasterVolume(MasterVolume);
        ConfigHandler.SetMusicVolume(MusicVolume);
        ConfigHandler.SetEffectsVolume(EffectsVolume);

        // === Accessibility Settings ===
        ConfigHandler.SetColorblindMode(ColorBlindMode);

        ConfigHandler.SaveConfig();
    }
}
