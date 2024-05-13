using HarmonyLib;
using PixelCrushers.DialogueSystem;

namespace FuriousTareIL2CPP.Patches;

[HarmonyPriority(Priority.Low)]
[HarmonyPatch(
    typeof(JanusNode),
    nameof(JanusNode.GetIDOfConditionMet)
)]
public class RemapVoiceOvers
{
    public static void Postfix(ref int __result, DialogueEntry entry)
    {
        // Explicit remapping to fix some voice-overs
        if (entry.ArticyID() == "0x0100005A00011DBC" && __result == 0) // "Ola, Tequila Sunset, grand boiadeiro!"
        {
            Logger.Log.LogInfo(
                $"JanusNode.GetIDOfConditionMet() remapped VO for dialogue: \"{entry.Title}\"."
            );
            __result = -1;
        }
    }
}
