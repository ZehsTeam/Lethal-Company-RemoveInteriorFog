using UnityEngine;

namespace com.github.zehsteam.RemoveInteriorFog;

internal static class Utils
{
    public static string GetHierarchyPath(Transform transform)
    {
        if (transform == null)
        {
            return string.Empty;
        }

        string path = transform.name;
        Transform currentTransform = transform;

        while (currentTransform.parent != null)
        {
            currentTransform = currentTransform.parent;
            path = $"{currentTransform.name}/{path}";
        }

        return path;
    }
}
