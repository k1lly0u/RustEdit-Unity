using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathNode))]
public class PathNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (!TerrainManager.HasValidTerrain())
        {
            GUILayout.Label("Create or load a map to continue...");
            return;
        }

        PathNode instance = (PathNode)target;       

        GUILayout.Label(string.Concat("Node Index : ", instance.transform.GetSiblingIndex()), EditorStyles.boldLabel);

        GUILayout.Space(5f);

        if (GUILayout.Button("Select Path Object"))
        {
            GameObject parent = instance.transform.parent?.gameObject;
            if (parent == null)
            {
                Debug.LogError("This path node does not have a parent path object. All nodes should be parented to a GameObject with a PathData component");
                return;
            }

            Selection.activeGameObject = parent;

            Debug.Log("Path object selected");
        }
    }
}
