using FortressOccident;
using HarmonyLib;
using Sunshine.Dialogue;

namespace FuriousTareIL2CPP.Patches;

/**
 * I didn't trace the exact root cause, but it seems that there is an "idle" animation
 * layer which has a "weight" of 1 when standing still, but 0 when moving or going to
 * the inventory screen. So the idle animation has priority over any "substance used"
 * animation.
 *
 * We can work around it by reducing the weight of the idle animation layer to 0
 * every time we consume a substance.
 */
public class TakeASwig
{
    private static float _weight = 0f;
    private const int IdleLayerIndex = 3;

    [HarmonyPatch(
        typeof(HudHeldPanelController),
        nameof(HudHeldPanelController.OnSubstanceUse)
    )]
    [HarmonyPrefix]
    public static void Prefix()
    {
        var animator = Character.Main.AnimatorComponent;
        _weight = animator.GetLayerWeight(
            IdleLayerIndex
        );
        if (_weight > 0)
        {
            Logger.Log.LogInfo(
                $"Setting idle layer weight to 0"
            );
            animator.SetLayerWeight(
                IdleLayerIndex,
                0
            );
        }
    }

    [HarmonyPatch(
        typeof(Announcements),
        nameof(Announcements.SubstanceUsed)
    )]
    [HarmonyPrefix]
    private static void RestoreLayerWeight()
    {
        // I wanted to use a pass-through IEnumerator, but was getting an error:
        // Unhandled exception. Il2CppInterop.Runtime.ObjectCollectedException: Object was garbage collected in IL2CPP domain
        // I also wasn't able to invoke my own coroutine.
        // So I'm just going to listen for the announcement.

        if (_weight > 0)
        {
            Logger.Log.LogInfo(
                $"Restoring idle layer weight to {_weight}"
            );
            var animator = Character.Main.AnimatorComponent;
            animator.SetLayerWeight(
                IdleLayerIndex,
                _weight
            );
        }
    }
}
