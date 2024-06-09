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
public class ThoughtCabinetScrolling
{
#pragma warning disable CA2211
    public static int ScrollSensitivity = 10;
#pragma warning restore CA2211
    
    // UI/Global UI Canvas/Global UI Fitter/FeldMaster/LeftMidRight/RIGHT/Tooltips/Thought Cabinet Tooltip/DescriptionText ScrollMask/Scroll View
    public static void Postfix()
    {
        const string objectName = "Thought Cabinet Tooltip/DescriptionText ScrollMask/Scroll View";
        var gameObject = GameObject.Find(
            objectName
        );
        if (gameObject == null)
        {
            Logger.Log.LogWarning(
                $"Couldn't find game object called \"{objectName}\", can't patch it"
            );
            return;
        }

        var component = gameObject.GetComponent<ScrollRect>();
        if (component == null)
        {
            Logger.Log.LogWarning(
                $"Couldn't find ScrollRect component"
            );
            return;
        }
        component.scrollSensitivity = ScrollSensitivity;
        Logger.Log.LogInfo($"Thought Cabinet scroll sensitivity set to {ScrollSensitivity}");
    }
}
