using BepInEx;
using BepInEx.Unity.IL2CPP;

namespace FuriousTareIL2CPP;

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
        var pluginEntryPoint = new PluginEntryPoint(Config, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_GUID);
    }
}
