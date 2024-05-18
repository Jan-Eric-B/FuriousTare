using System;
using System.Collections.Generic;
using System.Reflection;
using FuriousTareIL2CPP.Patches;
using HarmonyLib;

namespace FuriousTareIL2CPP;

/**
 * Use this to intercept every method on a specific type.
 * Useful for reverse engineering.
 */
public class DebugTypeLogger
{
    private static readonly Dictionary<string, int> MethodInvocationCountDict = new Dictionary<string, int>();
    private const int SuppressionThreshold = 200;

    private static void DoHook(object __instance, MethodBase __originalMethod, object[] __args)
    {
        MethodInvocationCountDict.TryGetValue(
            __originalMethod.Name,
            out var invocationCount
        );
        if (invocationCount >= SuppressionThreshold)
        {
            return;
        }

        var argsString = string.Join(
            ", ",
            __args
        );
        Logger.Log.LogInfo(
            $"{__instance}.${__originalMethod.Name}({argsString})"
        );
        invocationCount++;
        if (invocationCount >= SuppressionThreshold)
        {
            Logger.Log.LogInfo(
                $"Hit invocation threshold, suppressing further logging for ${__originalMethod.Name}"
            );
        }

        MethodInvocationCountDict[__originalMethod.Name] = invocationCount;
    }

    public static void RegisterPatches(Type type)
    {
        var harmony = new Harmony(
            "FuriousTare"
        );

        var patchMethod = new HarmonyMethod(
            AccessTools.Method(
                typeof(DebugTypeLogger),
                nameof(DoHook)
            )
        );
        foreach (var method in AccessTools.GetDeclaredMethods(
                     type
                 ))
        {
            if (!method.IsGenericMethod && !method.IsAbstract && !method.Name.StartsWith("get_") && !method.Name.StartsWith("set_"))
            {
                harmony.Patch(
                    method,
                    prefix: patchMethod
                );
            }
        }
    }
}
