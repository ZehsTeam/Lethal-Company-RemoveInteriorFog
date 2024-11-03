using BepInEx;
using BepInEx.Logging;
using com.github.zehsteam.RemoveInteriorFog.Patches;
using HarmonyLib;

namespace com.github.zehsteam.RemoveInteriorFog;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
internal class Plugin : BaseUnityPlugin
{
    private readonly Harmony _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    internal static Plugin Instance { get; private set; }
    internal static new ManualLogSource Logger { get; private set; }

    internal static ConfigManager ConfigManager { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;

        Logger = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_GUID);
        Logger.LogInfo($"{MyPluginInfo.PLUGIN_NAME} has awoken!");
        
        _harmony.PatchAll(typeof(RoundManagerPatch));

        ConfigManager = new ConfigManager();
    }
}
