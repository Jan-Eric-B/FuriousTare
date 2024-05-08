using BepInEx;
using BepInEx.Unity.Mono;
using FuriousTareShared;

namespace FuriousTareGOG;

[BepInPlugin(
    MyPluginInfo.PLUGIN_GUID,
    MyPluginInfo.PLUGIN_NAME,
    MyPluginInfo.PLUGIN_VERSION
)]
[BepInProcess(
    "Disco Elysium.exe"
)]
#pragma warning disable BepInEx002
public class Plugin : BaseUnityPlugin
#pragma warning restore BepInEx002
{
    Plugin()
    {
        FuriousTareShared.Logger.Log = Logger;
    }
    
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo(
            $"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!"
        );
        
        VoiceOverFixAlternatives.RegisterPatches();
    }
}
