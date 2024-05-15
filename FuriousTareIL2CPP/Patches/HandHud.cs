using HarmonyLib;
using UnityEngine;

namespace FuriousTareIL2CPP.Patches;

[HarmonyPatch(
    typeof(ShowTime),
    nameof(ShowTime.OnEnable)
)]
public class HandHud
{
    /**
     * The time HUD element has a Y that's too big, and overlaps the left/right hand icons.
     * We can fix the hand HUD by shrinking the clock.
     */
    public static void Postfix(ShowTime __instance)
    {
        var component = __instance.GetComponent<RectTransform>();
        // Change sizeDelta.y from 130 -> 41
        component.sizeDelta = new Vector2(component.sizeDelta.x, 41);
    }
}
