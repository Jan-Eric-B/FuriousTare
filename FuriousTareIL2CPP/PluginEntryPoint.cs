using HarmonyLib;
using FuriousTareIL2CPP.Patches;

namespace FuriousTareIL2CPP;

public class PluginEntryPoint
{
    public PluginEntryPoint(string pluginName, string pluginGuid)
    {
        var harmony = new Harmony(
            pluginGuid
        );

        // DebugTypeLogger.RegisterPatches(typeof(FlashlightBehaviour));
        
        foreach (var patch in new[]
                 {
                     typeof(DialoguePathFixes),
                     typeof(SkipIncorrectVoiceOver),
                     typeof(StopWavingThatFlashlight),
                     typeof(VoiceOverFixAlternatives)
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
