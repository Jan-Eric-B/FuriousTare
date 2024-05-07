using BepInEx;
using BepInEx.Unity.IL2CPP;

namespace DiscoElysiumModPlugin;

[BepInPlugin(
    MyPluginInfo.PLUGIN_GUID,
    MyPluginInfo.PLUGIN_NAME,
    MyPluginInfo.PLUGIN_VERSION
)]
[BepInProcess(
    "disco.exe"
)]
public class Plugin : BasePlugin
{
    public Plugin()
    {
        Logger.Log = Log;
    }

    public override void Load()
    {
        // Plugin startup logic
        Log.LogInfo(
            $"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!"
        );

        VoiceOverFixAlternatives.RegisterPatches();
    }
}
