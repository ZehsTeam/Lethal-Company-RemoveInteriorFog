using BepInEx.Configuration;

namespace com.github.zehsteam.RemoveInteriorFog;

internal class ConfigManager
{
    // General
    public ConfigEntry<bool> DisableVanillaInteriorFog { get; private set; }
    public ConfigEntry<bool> DisableOtherInteriorFog { get; private set; }

    public ConfigManager()
    {
        BindConfigs();
    }

    private void BindConfigs()
    {
        ConfigHelper.SkipAutoGen();

        // General
        DisableVanillaInteriorFog = ConfigHelper.Bind("General", "DisableVanillaInteriorFog", defaultValue: true,  requiresRestart: false, "If enabled, the vanilla interior fog added in v67 will be removed.");
        DisableOtherInteriorFog =   ConfigHelper.Bind("General", "DisableOtherInteriorFog",   defaultValue: false, requiresRestart: false, "If enabled, other interior fog will be removed. Works with modded interior fog.");
    }
}
