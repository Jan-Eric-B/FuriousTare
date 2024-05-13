using HarmonyLib;
using PixelCrushers.DialogueSystem;
using VOTool;

namespace FuriousTareIL2CPP.Patches;

[HarmonyPatch(typeof(VoiceOverClipsPlayer), nameof(VoiceOverClipsPlayer.PlayVoiceClip))]
public class SkipIncorrectVoiceOver
{
    public static bool Prefix(DialogueEntry entry)
    {
        var articyId = entry.ArticyID();
        // 0x0100005800001E34 = "Cindy the SKULL: \"She nods disdainfully toward the wo...\""
        var shouldSkipVoiceOver = articyId == "0x0100005800001E34";
        if (shouldSkipVoiceOver)
        {
            Logger.Log.LogInfo($"Skipping incorrect voiceover: \"{entry.Title}\". Articy ID: \"{articyId}\"");
            return false;
        }

        return true;
    }
}
