#if TOOLS
using Godot;

namespace BZ.Settings;

[Tool]
public partial class PluginMain : EditorPlugin
{
    // The name that will appear in the Autoload list
    private const string AutoloadName = "SettingsManager";
    
    // The path to your singleton script
    private const string AutoloadPath = "res://addons/BZSettingsSystem/scripts/SettingsManager.cs";

    public override void _EnterTree()
    {
        // This runs when the plugin is enabled
        AddAutoloadSingleton(AutoloadName, AutoloadPath);
        GD.Print("BZ-Settings-System: Plugin Enabled. Autoload registered.");
    }

    public override void _ExitTree()
    {
        // This runs when the plugin is disabled or the editor closes
        RemoveAutoloadSingleton(AutoloadName);
        GD.Print("BZ-Settings-System: Plugin Disabled. Autoload removed.");
    }
}
#endif