using FortressOccident;
using HarmonyLib;
using Il2CppSystem;
using Sunshine.Metric;
using UnityEngine;

namespace FuriousTareIL2CPP.Patches;

[HarmonyPatch]
public class StopWavingThatFlashlight
{
    private static readonly AnimationCurve FlashlightUnsnapRamp = AnimationCurve.EaseInOut(1f, 0.1f, 2, 5f);
    private static bool _isFlashlightIKEnabled = false;
    private static DateTime? _lastToggleOnTime;

    [HarmonyPatch(
        typeof(Character),
        nameof(Character.ToggleFlashlightIK)
    )]
    [HarmonyPrefix]
    public static void OnCharacterToggleFlashlightIK(ref bool __runOriginal, Character __instance, bool isEnabled)
    {
        Logger.Log.LogDebug(
            $"Toggling character flashlight IK: {isEnabled}. (Replacing original method)"
        );
        if (!_isFlashlightIKEnabled && isEnabled)
        {
            _lastToggleOnTime = DateTime.UtcNow;
        }

        _isFlashlightIKEnabled = isEnabled;
        if (InventoryViewData.Singleton.IsEquipped(
                "flashlight"
            ))
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
                Debug.Log(
                    "Unable to toggle flashlight IK because flashlight is hidden"
                );
            }
        }

        __runOriginal = false;
    }

    [HarmonyPatch(
        typeof(FlashlightTargetBehaviour),
        nameof(FlashlightTargetBehaviour.Update)
    )]
    [HarmonyPrefix]
    public static void OnFlashlightTargetBehaviourUpdate(ref bool __runOriginal, FlashlightTargetBehaviour __instance)
    {
        if (!_isFlashlightIKEnabled)
        {
            // Too chatty, don't log this.
            // Logger.Log.LogDebug(
            //     $"Flashlight IK was disabled, so don't update the IK target"
            // );
            __runOriginal = false;
        }
        else if (_lastToggleOnTime != null)
        {
            // Ease the IK back into the world for a little bit. Avoids a quick "snap" back to the mouse cursor.
            var lastTime = _lastToggleOnTime.Value;
            var now = DateTime.UtcNow;
            var diff = (now - lastTime).TotalSeconds;
            if (diff <= 1)
            {
                __instance.maxSpeed = 0.1f;
            }
            else if (diff <= 2)
            {
                __instance.maxSpeed = FlashlightUnsnapRamp.Evaluate((float)diff);
            }
            else
            {
                // Logger.Log.LogInfo(
                //     "Resetting Flashlight IK max speed"
                // );
                __instance.maxSpeed = 10f;
                _lastToggleOnTime = null;
            }
        }
    }
}
