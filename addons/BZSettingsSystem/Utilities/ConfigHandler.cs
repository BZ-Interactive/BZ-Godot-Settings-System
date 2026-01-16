using Godot;

namespace BZ.Utilities.ConfigHandler;
public static class ConfigHandler
{
    private static readonly ConfigFile Config = new();
    private const string userPrefix = "user://";
    private const string configFileName = "settings.cfg";

    #region Sections
    private const string Display = "Display";
    private const string Audio = "Audio";
    private const string Gameplay = "Gameplay";
    private const string Accessibility = "Accessibility";
    private const string General = "General";
    #endregion

    #region Keys
    // Display Settings
    private const string Resolution = "Resolution";
    private const string DisplayMode = "DisplayMode";
    private const string VSync = "VSync";
    private const string TargetFrameRate = "TargetFrameRate";
    private const string ShowPerformanceOverlay = "ShowPerformanceOverlay";

    // Audio Settings
    private const string MasterVolume = "MasterVolume";
    private const string MusicVolume = "MusicVolume";
    private const string EffectsVolume = "EffectsVolume";

    // Accessibility Settings
    private const string ColorblindMode = "ColorblindMode";
    #endregion

    public static void LoadConfigFile()
    {
        if (Config.Load(userPrefix + configFileName) != Error.Ok)
        {
            #if DEBUG
            GD.Print("No Config Found, Creating New One With Default Settings.");
            #endif
            
            SettingsData settings = SettingsManager.Instance.SettingsData;

            // === Display Methods ===
            SetResolution(settings.Resolution);
            SetDisplayMode(settings.DisplayMode);
            SetVSync(settings.VSync);
            SetTargetFrameRate(settings.TargetFrameRate);
            SetShowPerformanceOverlay(settings.ShowPerformanceOverlay);

            // === Audio Methods ===
            SetMasterVolume(settings.MasterVolume);
            SetMusicVolume(settings.MusicVolume);
            SetEffectsVolume(settings.EffectsVolume);

            // === Accessibility Methods ===
            SetColorblindMode(settings.ColorBlindMode);
            SaveConfig();
        }
        else
            LoadConfig();
    }

    public static void SaveConfig()
    {
        Config.Save(userPrefix + configFileName);
        #if DEBUG
        GD.Print("Saving Config To: " + OS.GetUserDataDir() + "/" + configFileName);
        #endif
    }

    private static void LoadConfig()
    {
        #if DEBUG
        GD.Print("Loading Data From Here: " + OS.GetUserDataDir() + "/" + configFileName);
        #endif
                
        // === Display Methods ===
        SettingsManager.Instance.SettingsData.Resolution = GetResolution();
        SettingsManager.Instance.SettingsData.DisplayMode = GetDisplayMode();
        SettingsManager.Instance.SettingsData.VSync = GetVSync();
        SettingsManager.Instance.SettingsData.TargetFrameRate = GetTargetFrameRate();
        SettingsManager.Instance.SettingsData.ShowPerformanceOverlay = GetShowPerformanceOverlay();
        //SettingsManager.Instance.SettingsData.Brightness = GetBrightness();

        // === Audio Methods ===
        SettingsManager.Instance.SettingsData.MasterVolume = GetMasterVolume();
        SettingsManager.Instance.SettingsData.MusicVolume = GetMusicVolume();
        SettingsManager.Instance.SettingsData.EffectsVolume = GetEffectsVolume();

        // === Accessibility Methods ===
        SettingsManager.Instance.SettingsData.ColorBlindMode = GetColorblindMode();
    }

    #region Display Methods
    public static void SetResolution(Vector2I resolution)
    {
        Config.SetValue(Display, Resolution, resolution);
    }

    public static Vector2I GetResolution()
    {
        if (Config.HasSectionKey(Display, Resolution))
            return (Vector2I)Config.GetValue(Display, Resolution);
        else
        {
            SetResolution(new Vector2I(1920, 1080));
            return new Vector2I(1920, 1080);
        }
    }

    public static void SetDisplayMode(DisplayServer.WindowMode mode)
    {
        Config.SetValue(Display, DisplayMode, SettingsManager.GetIndexFromWindowMode(mode));
    }

    public static DisplayServer.WindowMode GetDisplayMode()
    {
        if (Config.HasSectionKey(Display, DisplayMode))
            return SettingsManager.GetWindowModeFromIndex(Config.GetValue(Display, DisplayMode).AsString());
        else
        {
            SetDisplayMode(DisplayServer.WindowMode.ExclusiveFullscreen);
            return DisplayServer.WindowMode.ExclusiveFullscreen;
        }
    }

    public static void SetVSync(bool enabled)
    {
        Config.SetValue(Display, VSync, enabled);
    }

    public static bool GetVSync()
    {
        if (Config.HasSectionKey(Display, VSync))
            return (bool)Config.GetValue(Display, VSync);
        else
        {
            SetVSync(true);
            return true;
        }
    }

    public static void SetTargetFrameRate(int fps)
    {
        Config.SetValue(Display, TargetFrameRate, fps);
    }

    public static int GetTargetFrameRate()
    {
        if (Config.HasSectionKey(Display, TargetFrameRate))
            return (int)Config.GetValue(Display, TargetFrameRate);
        else
        {
            SetTargetFrameRate(60);
            return 60;
        }
    }

    public static void SetShowPerformanceOverlay(bool show)
    {
        Config.SetValue(Display, ShowPerformanceOverlay, show);
    }

    public static bool GetShowPerformanceOverlay()
    {
        if (Config.HasSectionKey(Display, ShowPerformanceOverlay))
            return (bool)Config.GetValue(Display, ShowPerformanceOverlay);
        else
        {
            SetShowPerformanceOverlay(false);
            return false;
        }
    }
    #endregion

    #region Audio Methods
    public static void SetMasterVolume(float volume)
    {
        Config.SetValue(Audio, MasterVolume, volume);
    }

    public static float GetMasterVolume()
    {
        if (Config.HasSectionKey(Audio, MasterVolume))
            return (float)Config.GetValue(Audio, MasterVolume);
        else
        {
            SetMasterVolume(1.0f);
            return 1.0f;
        }
    }

    public static void SetMusicVolume(float volume)
    {
        Config.SetValue(Audio, MusicVolume, volume);
    }

    public static float GetMusicVolume()
    {
        if (Config.HasSectionKey(Audio, MusicVolume))
            return (float)Config.GetValue(Audio, MusicVolume);
        else
        {
            SetMusicVolume(1.0f);
            return 1.0f;
        }
    }

    public static void SetEffectsVolume(float volume)
    {
        Config.SetValue(Audio, EffectsVolume, volume);
    }

    public static float GetEffectsVolume()
    {
        if (Config.HasSectionKey(Audio, EffectsVolume))
            return (float)Config.GetValue(Audio, EffectsVolume);
        else
        {
            SetEffectsVolume(1.0f);
            return 1.0f;
        }
    }
    #endregion


    #region Accessibility Methods
    public static void SetColorblindMode(ColorBlindMode mode)
    {
        Config.SetValue(Accessibility, ColorblindMode, (int)mode);
    }

    public static ColorBlindMode GetColorblindMode()
    {
        if (Config.HasSectionKey(Accessibility, ColorblindMode))
            return (ColorBlindMode)(int)Config.GetValue(Accessibility, ColorblindMode);
        else
        {
            SetColorblindMode(ColorBlindMode.None);
            return ColorBlindMode.None;
        }
    }
    #endregion
}