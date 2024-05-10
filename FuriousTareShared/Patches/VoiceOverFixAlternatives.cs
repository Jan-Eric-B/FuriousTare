using HarmonyLib;
using PixelCrushers.DialogueSystem;

namespace FuriousTareShared.Patches;

/**
 * The wrong voice over is played when a dialogue entry contains a script that
 * sets a Lua variable, and the "alternative" conditions read from that same variable.
 * (Text is not affected, because the text is determined _before_ a dialogue entry is
 * presented, but the voice over clip is determined _afterwards_.)
 *
 * We fix it by:
 * - Hooking into `JanusNode.HandleEntry()`, querying the "alternative ID" and storing it
 * - Hooking into `JanusNode.GetIDOfConditionMet()`, returning the stored "alternative ID"
 */
[HarmonyPatch(
    typeof(JanusNode)
)]
public class VoiceOverFixAlternatives
{
    private static int? _lastDialogueEntryId;
    private static int? _lastAlternativeId;

    [HarmonyPostfix]
    [HarmonyPatch(
        nameof(JanusNode.GetIDOfConditionMet)
    )]
    // ReSharper disable twice InconsistentNaming
    public static void GetIDOfConditionMetHook(ref int __result, DialogueEntry entry)
    {
        Logger.Log.LogDebug(
            $"JanusNode.GetIDOfConditionMet() called for {entry.id}. Original alternative ID: {__result}. Last entry ID: {_lastDialogueEntryId}. Last alternative: {_lastAlternativeId}"
        );
        if (_lastDialogueEntryId == null)
        {
            return;
        }

        if (_lastDialogueEntryId == entry.id && _lastAlternativeId != null && __result != _lastAlternativeId)
        {
            Logger.Log.LogInfo(
                $"JanusNode.GetIDOfConditionMet() intercepted for dialogue \"{Field.LookupValue(entry.fields, "Title")}\". Entry ID {entry.id}. Original alternative: {__result}, new alternative: {_lastAlternativeId}"
            );
            __result = _lastAlternativeId.Value;
        }

        // Always reset state afterwards, so we don't accidentally match some other text/voice over pair.
        ResetState();
    }

    [HarmonyPostfix]
    [HarmonyPatch(
        nameof(JanusNode.HandleEntry),
        new[] { typeof(DialogueEntry) }
    )]
    public static void HandleEntryHook(DialogueEntry entry)
    {
        ResetState(); // required before SetState, so we don't ignore the newly calculated alternative ID
        SetState(
            entry
        );
        Logger.Log.LogDebug(
            $"JanusNode.HandleEntry() intercepted. Title: {Field.LookupValue(entry.fields, "Title")}, ID: {_lastDialogueEntryId}, Articy ID: {Field.LookupValue(entry.fields, "Articy Id")}, dialogue alternative ID: {_lastAlternativeId}"
        );
    }

    private static void ResetState()
    {
        _lastAlternativeId = null;
        _lastDialogueEntryId = null;
    }

    private static void SetState(DialogueEntry entry)
    {
        _lastAlternativeId = JanusNode.GetIDOfConditionMet(
            entry
        );
        _lastDialogueEntryId = entry.id;
    }
}
