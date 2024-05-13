using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using HarmonyLib;
using FuriousTareIL2CPP.Patches;

namespace FuriousTareIL2CPP;

public class PluginEntryPoint
{
    private static readonly Type[] Patches = new[]
    {
        typeof(DialoguePathFixes),
        typeof(SkipIncorrectVoiceOver),
        typeof(StopWavingThatFlashlight),
        typeof(ThrowAGunLoseAGun),
        typeof(VoiceOverFixAlternatives)
    };

    private readonly Dictionary<Type, bool> _enabledPatches = new Dictionary<Type, bool>();
    
    private void LoadConfig(ConfigFile configFile)
    {
        foreach (var patch in Patches)
        {
            var configEntry = configFile.Bind(
                "Patches",
                patch.Name,
                true
            );
            _enabledPatches[patch] = configEntry.Value;
        }
    }
    
    public PluginEntryPoint(ConfigFile configFile, string pluginName, string pluginGuid)
    {
        LoadConfig(configFile);
        
        var harmony = new Harmony(
            pluginGuid
        );

        // DebugTypeLogger.RegisterPatches(typeof(FlashlightBehaviour));
        
        foreach (var patch in Patches)
        {
            if (_enabledPatches[patch])
            {
                Logger.Log.LogInfo(
                    $"Applying patch: {patch.Name}"
                );
                harmony.PatchAll(
                    patch
                );
            }
            else
            {
                Logger.Log.LogInfo(
                    $"Skipping disabled patch: {patch.Name}"
                );
            }
        }

        Logger.Log.LogInfo(
            $"Plugin \"{pluginName}\" (\"{pluginGuid}\") is loaded!"
        );
    }
}
