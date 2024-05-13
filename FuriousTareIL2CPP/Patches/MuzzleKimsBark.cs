using HarmonyLib;
using Il2CppSystem;

namespace FuriousTareIL2CPP.Patches;

[HarmonyPatch(
    typeof(LockedDoorSwitcher),
    nameof(LockedDoorSwitcher.KimBarkOtherWayIn)
)]
public class MuzzleKimsBark
{
    private static DateTime? _lastCallTime;

    public static bool Prefix()
    {
        var now = DateTime.UtcNow;
        if (_lastCallTime == null)
        {
            _lastCallTime = now;
            return true;
        }

        var diff = (now - _lastCallTime).Value.TotalSeconds;
        var shouldPlay = diff > 1;
        _lastCallTime = now;
        if (!shouldPlay)
        {
            Logger.Log.LogInfo(
                $"Muzzling Kim's bark - too soon since the last one. ({diff} seconds)"
            );
        }

        return shouldPlay;
    }
}
