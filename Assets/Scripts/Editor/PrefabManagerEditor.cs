using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabManager))]
public class PrefabManagerEditor : Editor
{
    private static int Index { get; set; }

    private string[] assets;

    private void OnEnable()
    {
        if (assets == null)
            assets = FileSystem.GetAssetList();
    }

    public override void OnInspectorGUI()
    {
        if (!TerrainManager.HasValidTerrain())
        {
            GUILayout.Label("Create or load a map to continue...");
            return;
        }

        if (assets == null)
        {
            GUILayout.Label("Asset bundles are not currently loaded...");
            return;
        }
        
        PrefabManager instance = (PrefabManager)target;

        GUILayout.Label("Prefab Spawner", EditorStyles.boldLabel);

        GUILayout.Space(5f);

        GUILayout.Label("To spawn a prefab select it from the dropdown and hit 'Spawn'", EditorStyles.wordWrappedMiniLabel);

        GUILayout.Space(5f);

        Index = EditorGUILayout.Popup(Index, assets);

        if (GUILayout.Button("Spawn"))
        {
            RaycastHit rayHit;
            if (Physics.Raycast(SceneView.lastActiveSceneView.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1.0f)), out rayHit, Mathf.Infinity, 1 << 8 | 1 << 9))
            {
                GameObject gameObject = PrefabManager.CreatePrefab(assets[Index], "Decor", rayHit.point, Quaternion.identity, Vector3.one);
                Debug.Log(string.Concat("Spawned ", assets[Index], " at ", rayHit.point));
            }
            else Debug.Log("Look at the terrain to spawn a prefab");
        }
    }
}
