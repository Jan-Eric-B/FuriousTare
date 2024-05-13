using PixelCrushers.DialogueSystem;

namespace FuriousTareIL2CPP;

public static class DialogueEntryExtensions
{
    public static string ArticyID(this DialogueEntry entry)
    {
        return Field.LookupValue(
            entry.fields,
            "Articy Id"
        );
    }
}
