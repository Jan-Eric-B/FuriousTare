using System.Linq;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace FuriousTareIL2CPP.Patches;

// Collage mode adds 2 to 6 seconds loading time.
// If you're not going to use it, you can disable it.
public class DisableCollageMode
{
    private const string CollageMode = "Scenes/CollageMode";

    [HarmonyPatch(
        typeof(FastLoadManager),
        nameof(FastLoadManager.LoadAllScenes)
    )]
    [HarmonyPrefix]
    public static void SkipSceneLoading(FastLoadManager __instance)
    {
        if (!__instance.scenesToSkip.Contains(
                CollageMode
            ))
        {
            __instance.scenesToSkip = __instance.scenesToSkip.AddItem(
                CollageMode
            ).ToArray();
            Logger.Log.LogInfo(
                "Collage mode preload will be skipped"
            );
        }
    }

    [HarmonyPatch(
        typeof(MainMenuList),
        nameof(MainMenuList.Start)
    )]
    [HarmonyPostfix]
    public static void DisableMenuItem()
    {
        var collageObj = GameObject.Find(
            "Menu Rock-in/Content/Collage"
        );
        if (!collageObj)
        {
            Logger.Log.LogWarning(
                "Could not find Collage Mode menu item"
            );
            return;
        }

        // Disable the "main menu button" behaviour
        var buttonComponent = collageObj.GetComponent<MainMenuButton>();
        if (!collageObj)
        {
            Logger.Log.LogWarning(
                "Could not find Collage Mode component: MainMenuButton"
            );
            return;
        }

        buttonComponent.enabled = false;

        // Grey out the menu item   text
        var textObj = GameObject.Find(
            "Menu Rock-in/Content/Collage/Text"
        );
        if (!textObj)
        {
            Logger.Log.LogWarning(
                "Could not find Collage Mode menu item text"
            );
            return;
        }

        var textComponent = textObj.GetComponent<TextMeshProUGUI>();
        if (!textComponent)
        {
            Logger.Log.LogWarning(
                "Could not find Collage Mode component: TextMeshProUGUI"
            );
            return;
        }

        textComponent.faceColor = new Color32(
            127,
            127,
            127,
            127
        );

        Logger.Log.LogInfo(
            "Collage Mode button disabled"
        );
    }
}
