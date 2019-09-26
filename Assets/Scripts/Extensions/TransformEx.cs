using UnityEngine;

public static class TransformEx
{
    public static Transform[] GetDirectChildren(this Transform transform)
    {
        Transform[] array = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            array[i] = transform.GetChild(i);
        }

        return array;
    }
}