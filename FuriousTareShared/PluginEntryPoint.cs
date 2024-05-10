using HarmonyLib;
using FuriousTareShared.Patches;

namespace FuriousTareShared;

public class PluginEntryPoint
{
    public PluginEntryPoint(string pluginName, string pluginGuid)
    {
        var harmony = new Harmony(
            pluginGuid
        );
        harmony.PatchAll(typeof(SkipIncorrectVoiceOver));
        harmony.PatchAll(typeof(VoiceOverFixAlternatives));

        Logger.Log.LogInfo(
            $"Plugin \"{pluginName}\" (\"{pluginGuid}\") is loaded!"
        );
    }
}
