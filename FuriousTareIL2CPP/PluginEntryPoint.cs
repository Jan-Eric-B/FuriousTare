using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using FuriousTareIL2CPP.Patches;
using HarmonyLib;

namespace FuriousTareIL2CPP;

public class PluginEntryPoint
{
    private static readonly Type[] Patches = new[]
    {
        typeof(AbilityLabelOverflow),
        typeof(DialoguePathFixes),
        typeof(HandHud),
        typeof(HandHudReplaceHeldItem),
        typeof(MuzzleKimsBark),
        typeof(RemapVoiceOvers), // should apply after VoiceOverFixAlternatives by using low priority
        typeof(TweakHudWhiteSpace),
        typeof(SkipIncorrectVoiceOver),
        typeof(StopWavingThatFlashlight),
        typeof(TakeASwig),
        typeof(ThoughtCabinetScrolling),
        typeof(ThrowAGunLoseAGun),
        typeof(VoiceOverFixAlternatives),
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
        var configEntryThcScrollSensitivity = configFile.Bind(
            nameof(ThoughtCabinetScrolling),
            nameof(ThoughtCabinetScrolling.ScrollSensitivity),
            10,
            "Change the scroll sensitivity in the thought cabinet description panels. Originally 1, defaults to 10."
        );
        ThoughtCabinetScrolling.ScrollSensitivity = configEntryThcScrollSensitivity.Value;
    }
    
    public PluginEntryPoint(ConfigFile configFile, string pluginName, string pluginGuid)
    {
        LoadConfig(configFile);
        
        var harmony = new Harmony(
            pluginGuid
        );

        // DebugTypeLogger.RegisterPatches(typeof(HudHeldButton));
        // DebugTypeLogger.RegisterPatches(typeof(HudHeldPanelController));
        // DebugTypeLogger.RegisterPatches(typeof(InventoryViewData));
        
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
