using System;
using HarmonyLib;
using Sunshine.Metric;

namespace FuriousTareIL2CPP.Patches;

/**
 * Equip() and Hold() attempt to unequip any existing items.
 * It happens asynchronously, after those methods have finished.
 * This means the newly equipped items are immediately unequipped.
 * This particular signature of Unequip() is only called by those methods.
 * We can work around the problem by checking if the currently equipped item
 * is the one we're trying to unequip, and if not, once Unequip() has finished,
 * we can restore the original item and refresh the hand HUD (for the 3rd time).
 */
[HarmonyPatch(
    typeof(InventoryViewData),
    nameof(InventoryViewData.Unequip),
    new Type[] { typeof(string), typeof(EquipmentSlotType), typeof(bool) }
)]
public class HandHudReplaceHeldItem
{
    public static void Prefix(out string __state, InventoryViewData __instance, string itemName,
        EquipmentSlotType convertedType)
    {
        var equippedItem = __instance.equipment[convertedType];
        __state = itemName != equippedItem ? equippedItem : null;
    }

    public static void Postfix(string __state, InventoryViewData __instance, EquipmentSlotType convertedType)
    {
        if (__state == null)
        {
            return;
        }

        Logger.Log.LogInfo(
            $"Re-equipping item \"{__state}\" and refreshing HUD"
        );
        __instance.equipment[convertedType] = __state;
        HudHeldPanelController.Current.UpdateHeldPanel();
    }
}
