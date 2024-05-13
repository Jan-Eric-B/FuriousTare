using FortressOccident;
using HarmonyLib;
using Sunshine.Metric;
using UnityEngine;

namespace FuriousTareIL2CPP.Patches;

[HarmonyPatch]
public class StopWavingThatFlashlight
{
    private static bool _isFlashlightIKEnabled = false;

    [HarmonyPatch(
        typeof(Character),
        nameof(Character.ToggleFlashlightIK)
    )]
    [HarmonyPrefix]
    public static void OnCharacterToggleFlashlightIK(ref bool __runOriginal, Character __instance, bool isEnabled)
    {
        Logger.Log.LogDebug(
            $"Toggling character flashlight IK: {isEnabled}. (Skipping original method)"
        );
        _isFlashlightIKEnabled = isEnabled;
        if (InventoryViewData.Singleton.IsEquipped("flashlight"))
        {
            try
            {
                FlashlightBehaviour componentInChildren = __instance.GetComponentInChildren<FlashlightBehaviour>();
                CrossPlatformInputManager.mCPIM.AnalogueCharPos.isFlashlightActive = isEnabled;
                if (isEnabled)
                {
                    CrossPlatformInputManager.mCPIM.AnalogueCharPos.resetFlashlightPos();
                }
            }
            catch
            {
                Debug.Log("Unable to toggle flashlight IK because flashlight is hidden");
            }
        }
        
        __runOriginal = false;
    }

    [HarmonyPatch(
        typeof(FlashlightTargetBehaviour),
        nameof(FlashlightTargetBehaviour.Update)
    )]
    [HarmonyPrefix]
    public static void OnFlashlightTargetBehaviourUpdate(ref bool __runOriginal)
    {
        if (!_isFlashlightIKEnabled)
        {
            // Too chatty, don't log this.
            // Logger.Log.LogDebug(
            //     $"Flashlight IK was disabled, so don't update the IK target"
            // );
            __runOriginal = false;
        }
    }
}
