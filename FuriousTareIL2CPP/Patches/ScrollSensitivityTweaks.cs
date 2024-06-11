using System.Collections.Generic;
using BepInEx.Configuration;
using FortressOccident;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace FuriousTareIL2CPP.Patches;

// The thought cabinet scrolling sensitivity is way too low when using a scroll wheel.
// The sensitivity is 1. The journal uses 10, and the skills page uses 5.
[HarmonyPatch(
    typeof(FeldController),
    nameof(FeldController.Start)
)]
public class ScrollSensitivityTweaks
{
    private static readonly Dictionary<string, (string objectName, float scrollSensitivity, float originalValue)>
        Changes = new()
        {
            {
                "ThoughtCabinetDescription", ("Thought Cabinet Tooltip/DescriptionText ScrollMask/Scroll View", 10f, 1f)
            }
        };

    public static void LoadConfig(ConfigFile configFile)
    {
        foreach (var (name, entry) in Changes)
        {
            var configEntry = configFile.Bind(
                nameof(ScrollSensitivityTweaks),
                name,
                entry.scrollSensitivity,
                $"Scroll sensitivity for {name}. Original value: {entry.originalValue}"
            );
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (configEntry.Value != entry.scrollSensitivity) // ignore fractional, assume only integers
            {
                Changes[name] = entry with { scrollSensitivity = configEntry.Value };
            }
        }
    }

    public static void Postfix()
    {
        foreach (var (name, (objectName, scrollSensitivity, _)) in Changes)
        {
            if (scrollSensitivity == 0)
            {
                Logger.Log.LogInfo(
                    $"Scroll sensitivity for {name} is 0, skipping"
                );
                continue;
            }

            var gameObject = GameObject.Find(
                objectName
            );
            if (gameObject == null)
            {
                Logger.Log.LogWarning(
                    $"Couldn't find game object called \"{objectName}\", can't patch it"
                );
                continue;
            }

            var component = gameObject.GetComponent<ScrollRect>();
            if (component == null)
            {
                Logger.Log.LogWarning(
                    $"Couldn't find ScrollRect component"
                );
                continue;
            }

            component.scrollSensitivity = scrollSensitivity;
            Logger.Log.LogInfo(
                $"Scroll sensitivity for {name} set to {scrollSensitivity}"
            );
        }
    }
}
