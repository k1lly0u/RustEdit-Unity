using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineEx
{
    public static WaitForEndOfFrame WaitForEndOfFrame;

    private static Dictionary<float, WaitForSeconds> WaitForSecondsBuffer;

    static CoroutineEx()
    {
        CoroutineEx.WaitForEndOfFrame = new WaitForEndOfFrame();
        CoroutineEx.WaitForSecondsBuffer = new Dictionary<float, WaitForSeconds>();
    }

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds WaitForSecond;
        if (!CoroutineEx.WaitForSecondsBuffer.TryGetValue(seconds, out WaitForSecond))
        {
            WaitForSecond = new WaitForSeconds(seconds);
            CoroutineEx.WaitForSecondsBuffer.Add(seconds, WaitForSecond);
        }
        return WaitForSecond;
    }

    public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
    {
        return new WaitForSecondsRealtime(seconds);
    }
}
