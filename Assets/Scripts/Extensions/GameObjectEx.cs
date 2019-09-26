using UnityEngine;

public static class GameObjectEx
{
    public static T GetOrAddComponent<T>(this GameObject child) where T : Component
    {
        T result = child.GetComponent<T>();
        if (result == null)
        {
            result = child.gameObject.AddComponent<T>();
        }
        return result;
    }

    public static void SetLayerRecursive(this GameObject go, int Layer)
    {
        if (go.layer == 18 || go.layer == 28 || go.layer == 29)
        {
            go.SetActive(false);
            return;
        }

        go.layer = Layer;

        for (int i = 0; i < go.transform.childCount; i++)
        {
            go.transform.GetChild(i).gameObject.SetLayerRecursive(Layer);
        }
    }
}
