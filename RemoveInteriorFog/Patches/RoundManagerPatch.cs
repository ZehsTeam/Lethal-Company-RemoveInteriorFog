using DunGen;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace com.github.zehsteam.RemoveInteriorFog.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal static class RoundManagerPatch
{
    [HarmonyPatch(nameof(RoundManager.RefreshEnemiesList))]
    [HarmonyPostfix]
    [HarmonyPriority(Priority.Last)]
    private static void RefreshEnemiesListPatch()
    {
        if (!Plugin.ConfigManager.DisableVanillaInteriorFog.Value)
        {
            return;
        }

        RemoveVanillaInteriorFog();
    }

    [HarmonyPatch(nameof(RoundManager.FinishGeneratingLevel))]
    [HarmonyPostfix]
    private static void FinishGeneratingLevelPatch()
    {
        if (!Plugin.ConfigManager.DisableOtherInteriorFog.Value)
        {
            return;
        }

        RemoveOtherInteriorFog();
    }

    public static void RemoveVanillaInteriorFog()
    {
        if (RoundManager.Instance == null)
        {
            return;
        }

        if (RoundManager.Instance.indoorFog == null)
        {
            return;
        }

        DisableLocalVolumetricFog(RoundManager.Instance.indoorFog, "Vanilla interior fog has been disabled.");
    }

    public static void RemoveOtherInteriorFog()
    {
        DisableFogOnDungeonTiles();
        DisableFogBelowMoonSurface();
    }

    private static void DisableFogOnDungeonTiles()
    {
        foreach (var dungeonTile in Object.FindObjectsByType<Tile>(FindObjectsSortMode.None))
        {
            foreach (var localVolumetricFog in dungeonTile.GetComponentsInChildren<LocalVolumetricFog>())
            {
                DisableLocalVolumetricFog(localVolumetricFog);
            }
        }
    }

    private static void DisableFogBelowMoonSurface()
    {
        if (RoundManager.Instance == null)
        {
            return;
        }

        if (RoundManager.Instance.dungeonGenerator == null)
        {
            return;
        }

        if (RoundManager.Instance.dungeonGenerator.Root == null)
        {
            return;
        }

        float minY = RoundManager.Instance.dungeonGenerator.Root.transform.position.y + 10f;

        foreach (var localVolumetricFog in Object.FindObjectsByType<LocalVolumetricFog>(FindObjectsSortMode.None))
        {
            if (localVolumetricFog == RoundManager.Instance.indoorFog)
            {
                continue;
            }

            Vector3 bottomPosition = GetBottomPosition(localVolumetricFog);

            if (bottomPosition.y <= minY)
            {
                DisableLocalVolumetricFog(localVolumetricFog);
            }
        }
    }

    private static Vector3 GetBottomPosition(LocalVolumetricFog localVolumetricFog)
    {
        if (localVolumetricFog == null)
        {
            return Vector3.zero;
        }

        Vector3 position = localVolumetricFog.transform.position;
        position.y -= localVolumetricFog.parameters.size.y / 2f;

        return position;
    }

    private static void DisableLocalVolumetricFog(LocalVolumetricFog localVolumetricFog, string message = "")
    {
        if (localVolumetricFog == null)
        {
            return;
        }

        localVolumetricFog.gameObject.SetActive(false);

        if (!string.IsNullOrWhiteSpace(message))
        {
            Plugin.Logger.LogInfo(message);
            return;
        }

        Plugin.Logger.LogInfo($"Disabled LocalVolumetricFog \"{Utils.GetHierarchyPath(localVolumetricFog.transform)}\".");
    }
}
