using FortressOccident;
using HarmonyLib;
using UnityEngine;

namespace FuriousTareIL2CPP.Patches;

/**
 * In "The Final Cut", the clock's right edge is hard against the day text.
 * In the original version, there was some whitespace between them.
 * We shift the clock left a bit. We also move the hand HUD, so that
 * the space between the hand icons aligns with the colon in the time text.
 * Also, give more space between the hand icons and joystick button icons.
 */
[HarmonyPatch(
    typeof(FeldController),
    nameof(FeldController.Start)
)]
public class TweakHudWhiteSpace
{
    private static readonly (string objectName, float xMinus)[] Changes =
    {
        ("ClockText", 7f), ("HeldItemButtons", 5.5f), ("IconLeft", 7f), ("IconRight", -7f)
    };

    public static void Postfix()
    {
        foreach (var (objectName, xMinus) in Changes)
        {
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

            var component = gameObject.GetComponent<RectTransform>();
            component.anchoredPosition = new Vector2(
                component.anchoredPosition.x - xMinus,
                component.anchoredPosition.y
            );
        }

        // Patched!
        Logger.Log.LogInfo(
            "Clock and hand HUD phase shifted left a wee bit, and joystick icons spaced out."
        );
    }
}
