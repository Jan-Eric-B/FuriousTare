using FortressOccident;
using HarmonyLib;

namespace FuriousTareIL2CPP.Patches;

[HarmonyPatch]
public class StopWavingThatFlashlight
{
    private static bool _isFlashlightIKEnabled = false;

    [HarmonyPatch(
        typeof(Character),
        nameof(Character.ToggleFlashlightIK)
    )]
    [HarmonyPostfix]
    public static void OnCharacterToggleFlashlightIK(bool isEnabled)
    {
        Logger.Log.LogDebug(
            $"Toggling character flashlight IK: {isEnabled}."
        );
        _isFlashlightIKEnabled = isEnabled;
    }

    [HarmonyPatch(
        typeof(FlashlightTargetBehaviour),
        nameof(FlashlightTargetBehaviour.Update)
    )]
    [HarmonyPrefix]
    public static bool OnFlashlightTargetBehaviourUpdate()
    {
        if (!_isFlashlightIKEnabled)
        {
            // Too chatty, don't log this.
            // Logger.Log.LogDebug(
            //     $"Flashlight IK was disabled, so don't update the IK target"
            // );
            return false;
        }

        return true;
    }
}
