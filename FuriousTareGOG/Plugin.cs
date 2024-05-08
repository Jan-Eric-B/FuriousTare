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
    public Plugin()
    {
        FuriousTareShared.Logger.Log = Logger;
    }
    
    private void Awake()
    {
        var pluginEntryPoint = new PluginEntryPoint(MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_GUID);
    }
}
