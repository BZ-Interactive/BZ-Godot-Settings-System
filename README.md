# BZ Godot Settings System

[![Godot 4](https://img.shields.io/badge/Godot-4.0+-blue.svg)](https://godotengine.org/)
[![Version](https://img.shields.io/badge/version-0.5-orange.svg)](https://github.com/BZ-Interactive/BZ-Godot-Settings-System/releases)
[![License](https://img.shields.io/github/license/BZ-Interactive/BZ-Godot-Settings-System)](LICENSE)

A robust, **UI-agnostic C# settings framework** for Godot 4+. This plugin provides a complete backend for managing **Display**, **Audio**, and **Accessibility** settings with automatic persistence via `ConfigFile`. Build any UI you want while the framework handles validation, hardware application, and save/load operations.

---

## ✨ Features

### 🖥️ Display Settings
- **Window Modes**: Exclusive Fullscreen, Fullscreen, Windowed
- **Resolution Control**: Custom resolution support
- **VSync Toggle**: Enable/disable vertical synchronization
- **FPS Limiting**: Target framerate configuration
- **Performance Overlay**: Toggle performance metrics display

### 🔊 Audio Settings
- **Master Volume**: Global audio control
- **Music Volume**: Background music level
- **Effects Volume**: Sound effects level

### ♿ Accessibility Settings
- **Colorblind Modes**: Support for Protanopia, Deuteranopia, Tritanopia
- **Shader System**: Framework for colorblind correction shaders (implementation ready)

### 💾 Persistence
- Automatic save/load using Godot's `ConfigFile` system
- Saves to `user://settings.cfg`
- Default values on first launch
- Type-safe configuration handling

---

## 📦 Installation

1. **Download/Clone** this repository
2. Copy the `addons/BZSettingsSystem/` folder into your project's `addons/` directory
3. Open your project in Godot
4. Go to **Project → Project Settings → Plugins**
5. Enable **"BZ Godot Settings System"**

The plugin automatically registers `SettingsManager` as an autoload singleton.

---

## 🚀 Quick Start

### Access Settings from Any Script

```csharp
using Godot;

public partial class MyUI : Control
{
    private SettingsManager _settings;

    public override void _Ready()
    {
        // Get the autoload singleton
        _settings = GetNode<SettingsManager>("/root/SettingsManager");
        
        // Read current settings
        GD.Print($"Current Resolution: {_settings.SettingsData.Resolution}");
        GD.Print($"VSync Enabled: {_settings.SettingsData.VSync}");
    }
}
```

### Change Display Settings

```csharp
// Update resolution and apply immediately
_settings.SettingsData.Resolution = new Vector2I(1920, 1080);
_settings.SettingsData.DisplayMode = DisplayServer.WindowMode.Fullscreen;
_settings.SettingsData.VSync = true;
_settings.SettingsData.TargetFrameRate = 144;

// Apply changes to the engine and save to disk
_settings.SetDisplaySettings();
_settings.SaveSettings();
```

### Change Audio Settings

```csharp
// Adjust volumes (0-100 range recommended)
_settings.SettingsData.MasterVolume = 80f;
_settings.SettingsData.MusicVolume = 60f;
_settings.SettingsData.EffectsVolume = 100f;

// Save changes
_settings.SaveSettings();
```

### Change Accessibility Settings

```csharp
// Set colorblind mode
_settings.SettingsData.ColorBlindMode = ColorBlindMode.Deuteranopia;

// Apply shader (implement SetColorblindModeShader in SettingsManager)
_settings.SetDisplaySettings();
_settings.SaveSettings();
```

### Listen for Settings Changes

```csharp
public override void _Ready()
{
    var settings = GetNode<SettingsManager>("/root/SettingsManager");
    
    // Subscribe to change events
    settings.SettingsChanged += OnSettingsChanged;
}

private void OnSettingsChanged()
{
    GD.Print("Settings were updated!");
    // Update your UI here
}
```

---

## 📋 API Reference

### SettingsManager (Autoload Singleton)

| Property/Method | Description |
|----------------|-------------|
| `Instance` | Static reference to the singleton |
| `SettingsData` | Resource containing all settings values |
| `SaveSettings()` | Saves current settings to `user://settings.cfg` and triggers `SettingsChanged` event |
| `SetDisplaySettings()` | Applies display settings to the engine (window mode, resolution, VSync, FPS, colorblind shader) |
| `SettingsChanged` event | Fires whenever settings are saved |

### SettingsData (Resource)

**Display Settings:**
- `DisplayMode` (DisplayServer.WindowMode) - Window display mode
- `Resolution` (Vector2I) - Screen resolution
- `VSync` (bool) - VSync enabled/disabled
- `TargetFrameRate` (int) - Target FPS limit
- `ShowPerformanceOverlay` (bool) - Performance metrics visibility

**Audio Settings:**
- `MasterVolume` (float) - Master audio level
- `MusicVolume` (float) - Music track level
- `EffectsVolume` (float) - SFX level

**Accessibility:**
- `ColorBlindMode` (ColorBlindMode enum) - Colorblind filter mode

**Methods:**
- `ChangeDisplaySettings(...)` - Batch update display settings
- `ChangeAudioSettings(...)` - Batch update audio settings
- `ChangeAccessibilitySettings(...)` - Batch update accessibility settings
- `ChangeAllSettings(...)` - Update all settings at once

### ColorBlindMode Enum

```csharp
public enum ColorBlindMode
{
    None,
    Protanopia,    // Red-blind
    Deuteranopia,  // Green-blind
    Tritanopia     // Blue-blind
}
```

---

## 🛠️ Customization

### Adding New Settings

1. **Update `SettingsData.cs`**: Add your property with `[Export]`
   ```csharp
   [Export] public bool AutoSave { get; set; } = true;
   ```

2. **Update `ConfigHandler.cs`**: Add getter/setter methods
   ```csharp
   private const string AutoSave = "AutoSave";
   
   public static void SetAutoSave(bool enabled)
   {
       Config.SetValue(General, AutoSave, enabled);
   }
   
   public static bool GetAutoSave()
   {
       if (Config.HasSectionKey(General, AutoSave))
           return (bool)Config.GetValue(General, AutoSave);
       else
       {
           SetAutoSave(true);
           return true;
       }
   }
   ```

3. **Update Load/Save in `SettingsData.cs`**:
   ```csharp
   // In LoadSettings()
   AutoSave = ConfigHandler.GetAutoSave();
   
   // In SaveSettings()
   ConfigHandler.SetAutoSave(AutoSave);
   ```

### Implementing Colorblind Shaders

The `SetColorblindModeShader()` method in `SettingsManager.cs` is a placeholder. Implement it by:

1. Creating shader materials in the `Shaders/` folder for each mode
2. Applying them to a `ColorRect` overlay or viewport shader
3. Switching shaders based on the `ColorBlindMode` parameter

---

## 📂 Project Structure

```
addons/BZSettingsSystem/
├── plugin.cfg               # Plugin metadata
├── PluginMain.cs            # Autoload registration
├── settings_manager.tscn    # Optional scene (currently unused)
├── Scripts/
│   ├── SettingsManager.cs   # Singleton manager
│   ├── Data/
│   │   └── SettingsData.cs  # Settings data resource
│   └── Enums/
│       └── ColorBlindMode.cs
├── Utilities/
│   └── ConfigHandler.cs     # ConfigFile wrapper
├── Shaders/                 # (Empty - for future shaders)
└── Resources/
    └── SettingsData.tres    # Default settings resource
```

---

## 📝 License

This project is licensed under the [MIT License](LICENSE). See the LICENSE file for details.

---

## 🤝 Contributing

Contributions are welcome! Feel free to:
- Report bugs via [Issues](https://github.com/BZ-Interactive/BZ-Godot-Settings-System/issues)
- Submit feature requests
- Create pull requests

---

## 💡 Example Use Cases

- **Pause Menu Settings**: Wire up UI sliders/buttons to modify `SettingsData` properties
- **Options Screen**: Display current values and save changes on "Apply"
- **First-Time Setup**: Detect missing config and show welcome wizard
- **Accessibility Menu**: Provide colorblind mode selection with live preview

---

## ⚠️ Notes

- This plugin is **backend-only** - you must create your own UI
- Audio volume values are stored but **not automatically applied to AudioServer buses** - implement this in your own audio manager
- Colorblind shader implementation is left to the user
- Uses C# - .NET-enabled Godot builds required

---

**Made with ❤️ for the Godot Community**
