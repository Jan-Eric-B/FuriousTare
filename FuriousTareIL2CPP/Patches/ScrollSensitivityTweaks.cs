using System.Collections.Generic;
using BepInEx.Configuration;
using FortressOccident;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace FuriousTareIL2CPP.Patches;

// The thought cabinet scrolling sensitivity is way too low when using a scroll wheel.
// The sensitivity is 1. The journal uses 10, and the skills page uses 5.
// Scrolling is pretty slow in general, so we allow tweaking a few different scroll panels.
[HarmonyPatch(
    typeof(FeldController),
    nameof(FeldController.Start)
)]
public class ScrollSensitivityTweaks
{
    private static readonly Dictionary<string, (string objectName, float scrollSensitivity, float originalValue)>
        Changes = new()
        {
            // Dialogue
            // UI/Global UI Canvas/Global UI Fitter/FeldMaster/LeftMidRight/RIGHT/Dialogue Panel/DialoguePanel Mask/Scrolling Panel
            // Initial: 20
            // Recommended: 60
            { "Dialogue", ("Dialogue Panel/DialoguePanel Mask/Scrolling Panel", 60f, 10f) },

            // Inv item description
            // UI/Global UI Canvas/Global UI Fitter/FeldMaster/LeftMidRight/RIGHT/Tooltips/Inventory Tooltip/Item Description Mask/Scroll View
            // Initial: 10
            { "InventoryItemDescription", ("Inventory Tooltip/Item Description Mask/Scroll View", 20f, 10f) },

            // Journal tasks
            // UI/Global UI Canvas/Global UI Fitter/FeldMaster/FullScreen/FULL/Journal/TasksTab/TaskList/Scroll View
            // Initial: 10
            // Snaps to steps
            // Change VerticalStepScrollViewJournalTasks -> VerticalStepScrollView.stepSize
            // Set both to 80 for snap scrolling
            // Or, only set the stepSize, and get smooth scrolling
            { "JournalTasksList", ("Journal/TasksTab/TaskList/Scroll View", 80f, 10f) },

            // Journal white checks
            // UI/Global UI Canvas/Global UI Fitter/FeldMaster/FullScreen/FULL/Journal/ChecksTab/ChecksList/Scroll View
            // Initial: 10
            // Also step size set to 5, so has annoying rebound.
            // 20/40 seems good.
            { "JournalWhiteChecks", ("Journal/ChecksTab/ChecksList/Scroll View", 40f, 5f) },

            // Save Game file scroll list
            // UI/Global UI Canvas/Global UI Fitter/FeldMaster/MainMenu/SubView/Save View/Content/FileListPanel/FileListScrollView
            // Initial: 12/30
            { "SaveLoadFileList", ("Save Load Screen/Template/Content/FileListPanel/FileListScrollView", 80f, 30f) },

            // Skill description
            // UI/Global UI Canvas/Global UI Fitter/FeldMaster/FullScreen/FULL/Charsheet/Character Sheet Info Panel/PortraitMask/Scalable Text/InfoPanel
            // Initial: 5
            { "SkillDescription", ("Character Sheet Info Panel/PortraitMask/Scalable Text/InfoPanel", 20f, 5f) },

            // Thought Cabinet Description
            // UI/Global UI Canvas/Global UI Fitter/FeldMaster/LeftMidRight/RIGHT/Tooltips/Thought Cabinet Tooltip/DescriptionText ScrollMask/Scroll View
            // Initial: 1
            {
                "ThoughtCabinetDescription", ("Thought Cabinet Tooltip/DescriptionText ScrollMask/Scroll View", 10f, 1f)
            },

            // Thought cabinet list
            // UI/Global UI Canvas/Global UI Fitter/FeldMaster/LeftMidRight/MID/Thoughts Container/Thoughts List Scroll View
            // Initial: 20/20
            { "ThoughtCabinetList", ("Thoughts Container/Thoughts List Scroll View", 40f, 20f) },

            // Thought internalized popup
            // UI/Global UI Canvas/Global UI Fitter/FeldMaster/FullScreen/FULL/ThoughtsSplashScreen/Content/Description Area/Details Mask/Description Panel
            // Initial: 20
            {
                "ThoughtInternalised",
                ("ThoughtsSplashScreen/Content/Description Area/Details Mask/Description Panel", 40f, 20f)
            },
        };

    public static void LoadConfig(ConfigFile configFile)
    {
        foreach (var (name, entry) in Changes)
        {
            var configEntry = configFile.Bind(
                nameof(ScrollSensitivityTweaks),
                name,
                entry.scrollSensitivity,
                $"Scroll sensitivity for {name}. Original value: {entry.originalValue}"
            );
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (configEntry.Value != entry.scrollSensitivity) // ignore fractional, assume only integers
            {
                Changes[name] = entry with { scrollSensitivity = configEntry.Value };
            }
        }
    }

    public static void Postfix()
    {
        var gameObjects = FindGameObjects();
        foreach (var (name, gameObject) in gameObjects)
        {
            var (_, scrollSensitivity, _) = Changes[name];
            if (scrollSensitivity == 0)
            {
                Logger.Log.LogInfo(
                    $"Scroll sensitivity for {name} is 0, skipping (will use the original value)"
                );
                continue;
            }

            // Prefer setting step size, if component exists, since it overrides the scroll rect
            var componentVerticalStep = gameObject.GetComponent<VerticalStepScrollView>();
            var originalValue = 0f;
            if (componentVerticalStep != null)
            {
                originalValue = componentVerticalStep.stepSize;
                componentVerticalStep.stepSize = scrollSensitivity;
                // Also set the scroll rect sensitivity to the lowest possible value. This gives us smooth scrolling,
                //  and avoids issues if we set the step size to be lower than the scroll sensitivity (for some reason)
                var component = gameObject.GetComponent<ScrollRect>();
                if (component != null)
                {
                    component.scrollSensitivity = 1f;
                }
            }
            else
            {
                var component = gameObject.GetComponent<ScrollRect>();
                if (component == null)
                {
                    Logger.Log.LogWarning(
                        $"Couldn't find ScrollRect component"
                    );
                    continue;
                }

                originalValue = component.scrollSensitivity;
                component.scrollSensitivity = scrollSensitivity;
            }

            Logger.Log.LogInfo(
                $"Scroll sensitivity for {name} set from {originalValue} -> {scrollSensitivity}"
            );
        }
    }

    /**
     * Not all game objects are active yet, so "Find()" doesn't work.
     * This iterates and filters components we're potentially interested in.
     */
    private static Dictionary<string, GameObject> FindGameObjects()
    {
        var baseObject = Object.FindObjectOfType<GlobalUIFitter>(
            true
        );
        if (baseObject == null)
        {
            Logger.Log.LogWarning(
                $"Couldn't find base UI game object"
            );
            return null;
        }

        var output = new Dictionary<string, GameObject>();
        var components = baseObject.gameObject.GetComponentsInChildren<ScrollRect>(
            true
        );
        // Iterate through components. Get full path of gameObject, check if it's list of known names.
        // Add the gameObject to the dict.
        foreach (var component in components)
        {
            var fullPath = FuriousTareUtils.GetFullPath(
                component.gameObject
            );

            foreach (var (name, (objectName, _, _)) in Changes)
            {
                if (fullPath.Contains(
                        objectName
                    ))
                {
                    output[name] = component.gameObject;
                }
            }
        }

        return output;
    }
}
