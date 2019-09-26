using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathData))]
public class PathDataEditor : Editor
{
    public override void OnInspectorGUI()
    {        
        if (!TerrainManager.HasValidTerrain())
        {
            GUILayout.Label("Create or load a map to continue...");
            return;
        }

        PathData instance = (PathData)target;

        GUILayout.Label(string.Concat("Path Type : ", instance.pathType), EditorStyles.boldLabel);

        GUILayout.Space(5f);

        if (instance.pathType == PathType.Road)
        {
            GUILayout.Label("Road Options", EditorStyles.boldLabel);

            instance.serializedData.start = GUILayout.Toggle(instance.serializedData.start, "Cap Road Start");
            instance.serializedData.end = GUILayout.Toggle(instance.serializedData.end, "Cap Road End");
            GUILayout.Space(5f);
        }

        GUILayout.Label("Mesh Options", EditorStyles.boldLabel);

        instance.serializedData.width = EditorGUILayout.FloatField("Path Width", instance.serializedData.width);

        GUILayout.Label("Terrain Options", EditorStyles.boldLabel);

        instance.serializedData.innerPadding = EditorGUILayout.FloatField("Inner Padding", instance.serializedData.innerPadding);
        instance.serializedData.outerPadding = EditorGUILayout.FloatField("Outer Padding", instance.serializedData.outerPadding);
        instance.serializedData.innerFade = EditorGUILayout.FloatField("Inner Fade", instance.serializedData.innerFade);
        instance.serializedData.outerFade = EditorGUILayout.FloatField("Outer Fade", instance.serializedData.outerFade);

        GUILayout.Space(5f);

        if (GUILayout.Button("Add Node At Start"))
        {
            Vector3 position = Quaternion.Euler(instance.GetVectorAt(1) - instance.GetVectorAt(0)) * Vector3.forward;

            instance.AddNode(position, 0);

            Debug.Log("Added a path node at the start of the path");
        }

        if (GUILayout.Button("Add Node At End"))
        {
            int count = instance.GetPointCount() - 1;

            Vector3 position = Quaternion.Euler(instance.GetVectorAt(count) - instance.GetVectorAt(count - 1)) * Vector3.forward;

            instance.AddNode(position, 0);

            Debug.Log("Added a path node at the end of the path");
        }        
    }
}
