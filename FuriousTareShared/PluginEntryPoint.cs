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

        foreach (var patch in new[]
                 {
                     typeof(DialoguePathFixes), typeof(SkipIncorrectVoiceOver), typeof(VoiceOverFixAlternatives)
                 })
        {
            Logger.Log.LogInfo(
                $"Applying patch: {patch.Name}"
            );
            harmony.PatchAll(
                patch
            );
        }

        Logger.Log.LogInfo(
            $"Plugin \"{pluginName}\" (\"{pluginGuid}\") is loaded!"
        );
    }
}
