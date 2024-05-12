using HarmonyLib;

namespace FuriousTareShared.Patches;

[HarmonyPatch(
    typeof(FlashlightBehaviour)
)]
public class StopWavingThatFlashlight
{
    private static bool isLightEnabled = false;
    
    [HarmonyPatch(
        nameof(FlashlightBehaviour.EnableLights)
    )]
    [HarmonyPostfix]
    public static void EnableLightsPostfix(bool setEnable)
    {
        Logger.Log.LogInfo(
            $"FlashlightBehaviour EnableLights: {setEnable}."
        );
        isLightEnabled = setEnable;
    }
    
    [HarmonyPatch(
        nameof(FlashlightBehaviour.OnIdleAnimFinished)
    )]
    [HarmonyPrefix]
    public static bool OnIdleAnimFinishedPrefix()
    {
        if (isLightEnabled)
        {
            Logger.Log.LogDebug(
                $"FlashlightBehaviour.OnIdleAnimFinished(): lights are already enabled, so idle anim probably wasn't started. Skipping execution."
            );
            return false;
        }

        return true;
    }
}
