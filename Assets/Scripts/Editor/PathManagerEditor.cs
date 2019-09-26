using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathManager))]
public class PathManagerEditor : Editor
{
    private static PathType Type { get; set; }

    public override void OnInspectorGUI()
    {        
        if (!TerrainManager.HasValidTerrain())
        {
            GUILayout.Label("Create or load a map to continue...");
            return;
        }

        PathManager instance = (PathManager)target;

        GUILayout.Label("Path Creator", EditorStyles.boldLabel);

        GUILayout.Space(5f);

        Type = (PathType)EditorGUILayout.EnumPopup("Path Type", Type);

        if (GUILayout.Button("Create"))
        {
            RaycastHit rayHit;
            if (Physics.Raycast(SceneView.lastActiveSceneView.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1.0f)), out rayHit, Mathf.Infinity, 1 << 8))
            {
                PathManager.CreatePath(Type, rayHit.point);
                Debug.Log(string.Concat("Create a new ", Type, " path at ", rayHit.point));
            }
            else Debug.Log("Look at the terrain to create a path");
        }
    }
}
