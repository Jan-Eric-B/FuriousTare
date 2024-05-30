using FortressOccident;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace FuriousTareIL2CPP.Patches;

/**
 * The ability label number and pips overflow when you go over 9. This can happen if you:
 * - Start with Physique 4 (or higher)
 * - Internalise the thought "Revacholian Nationhood", which grants +2 Physique per drink
 * - Drink three different alcohols, giving a boost of +6 Physique, for a total of 10.
 */
[HarmonyPatch(
    typeof(FeldController),
    nameof(FeldController.Start)
)]
public class AbilityLabelOverflow
{
    private const string CommonPrefix =
        "Leveling/Abilities/";

    private const string CommonSuffix = "/Label/";

    private const string NumberSuffix = "Number";

    private const string PipsSuffix = "Pips";

    private static readonly string[] AbilityNames = { "Intellect", "Psyche", "Physique", "Motorics" };

    public static void Postfix()
    {
        Logger.Log.LogInfo(
            $"Adjusting ability labels to handle text overflow"
        );
        foreach (var abilityName in AbilityNames)
        {
            // Update the number text, so it doesn't wrap onto the next line
            var objectName = CommonPrefix + abilityName + CommonSuffix + NumberSuffix;
            var gameObject = GameObject.Find(
                objectName
            );
            if (gameObject == null)
            {
                Logger.Log.LogInfo(
                    $"Could not find game object for ability: {objectName}"
                );
                continue;
            }

            var textMesh = gameObject.GetComponent<TextMeshProUGUI>();
            if (textMesh is null)
            {
                Logger.Log.LogInfo(
                    $"Could not find rect for ability: {objectName}"
                );
                continue;
            }

            textMesh.enableWordWrapping = false;

            var rect = gameObject.GetComponent<RectTransform>();
            if (rect == null)
            {
                Logger.Log.LogInfo(
                    $"Could not find rect for ability: {objectName}"
                );
                continue;
            }

            rect.anchoredPosition = new Vector2(
                rect.anchoredPosition.x - 13,
                rect.anchoredPosition.y
            );
            rect.sizeDelta = new Vector2(
                rect.sizeDelta.x + 26,
                rect.sizeDelta.y
            );

            // Update the pips, so they dynamically shrink. Tweak the vertical positioning slightly.
            objectName = CommonPrefix + abilityName + CommonSuffix + PipsSuffix;
            gameObject = GameObject.Find(
                objectName
            );

            if (gameObject == null)
            {
                Logger.Log.LogInfo(
                    $"Could not find game object for ability: {objectName}"
                );
                continue;
            }

            textMesh = gameObject.GetComponent<TextMeshProUGUI>();
            if (textMesh is null)
            {
                Logger.Log.LogInfo(
                    $"Could not find rect for ability: {objectName}"
                );
                continue;
            }

            textMesh.alignment = TextAlignmentOptions.Center;
            textMesh.enableAutoSizing = true;
            textMesh.enableWordWrapping = false;
            textMesh.fontSizeMin = 10;

            rect = gameObject.GetComponent<RectTransform>();
            if (rect == null)
            {
                Logger.Log.LogInfo(
                    $"Could not find rect for ability: {objectName}"
                );
                continue;
            }

            rect.anchoredPosition = new Vector2(
                rect.anchoredPosition.x,
                rect.anchoredPosition.y + 2
            );
            rect.sizeDelta = new Vector2(
                rect.sizeDelta.x - 90,
                rect.sizeDelta.y - 9
            );
        }
    }
}
