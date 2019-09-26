using System.IO;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldManager))]
public class WorldManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WorldManager instance = (WorldManager)target;

        GUILayout.Label("Rust Install Directory", EditorStyles.boldLabel);
        if (!string.IsNullOrEmpty(instance.bundlePath))
        {
            GUILayout.Label(instance.bundlePath);
            GUILayout.Label("Valid bundle path selected", EditorStyles.miniBoldLabel);
        }
        
        GUILayout.Space(5f);

        if (GUILayout.Button("Select Rust Directory"))
        {
            AssetBundle.UnloadAllAssetBundles(true);

            string path = EditorUtility.OpenFolderPanel("Rust Install Path", "", "");

            path = Path.Combine(path, "Bundles", "Bundles");

            if (!File.Exists(path))
            {
                Debug.Log("Unable to find the bundle file....");
                return;
            }

            EditorUtility.DisplayProgressBar("Loading Bundles", string.Empty, 0f);

            try
            {
                PlayerPrefs.SetString("RustInstallPath", path);
                instance.Initialize(path);
            }
            catch { }

            EditorUtility.ClearProgressBar();
        }

        if (string.IsNullOrEmpty(instance.bundlePath))        
            return;

        GUILayout.Space(5f);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Space(5f);

        GUILayout.Label("File Controls", EditorStyles.boldLabel);

        GUILayout.Space(5f);

        if (GUILayout.Button("New"))
        {
            NewMapPopupWindow window = ScriptableObject.CreateInstance<NewMapPopupWindow>();

            Rect position = window.position;
            position.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
            position.height = 150;
            window.position = position;

            window.worldManager = instance;
            window.ShowPopup();
        }

        if (GUILayout.Button("Load"))
        {
            string path = EditorUtility.OpenFilePanel("Select a map file...", "", "map");

            if (string.IsNullOrEmpty(path))
                return;

            instance.Load(path);
        }

        if (GUILayout.Button("Save"))
        {
            string path = EditorUtility.SaveFilePanel("Save map...", string.Empty, string.IsNullOrEmpty(instance.mapPath) ? "untitled" : instance.mapPath, "map");

            if (string.IsNullOrEmpty(path))
                return;

            instance.Save(path);
        }

        if (GUILayout.Button("Reset"))
        {       
            if (EditorUtility.DisplayDialog("Reset Scene", "Are you sure you want to reset the scene? All scene objects will be removed", "Reset", "Cancel"))
                Unity.EditorCoroutines.Editor.EditorCoroutineUtility.StartCoroutine(instance.Cleanup(), instance);
        }
    }
}




