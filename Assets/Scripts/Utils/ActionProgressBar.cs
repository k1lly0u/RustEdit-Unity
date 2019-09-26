using UnityEditor;

public static class ActionProgressBar
{    
    public static void UpdateProgress(string message, float progress)
    {
        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayProgressBar(message, string.Empty, progress);
    }

    public static void Close()
    {
        EditorUtility.ClearProgressBar();        
    }
}
