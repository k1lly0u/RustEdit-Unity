using UnityEngine;
using UnityEditor;

public class NewMapPopupWindow : EditorWindow
{
    public WorldManager worldManager;
    private int mapSize = 1000;
    private int splat = 3;
    private int biome = 0;

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Create New Map", EditorStyles.boldLabel);

        GUILayout.Space(10f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Map Size");
        mapSize = Mathf.Clamp(EditorGUILayout.IntField(mapSize), 1000, 6000);
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        splat = EditorGUILayout.IntPopup("Splat", splat, splats, splatInts);
        GUILayout.Space(5f);

        biome = EditorGUILayout.IntPopup("Biome", biome, biomes, biomeInts);
        GUILayout.Space(10f);

        if (GUILayout.Button("Create"))
        {
            worldManager.Create(mapSize, splat, biome);
            this.Close();
            return;
        }
        if (GUILayout.Button("Cancel"))
            this.Close();
    }

    private string[] splats = new string[] { "Dirt", "Snow", "Sand", "Rock", "Grass", "Forest", "Stones", "Gravel" };
    private int[] splatInts = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
    private string[] biomes = new string[] { "Arid", "Temperate", "Tundra", "Arctic" };
    private int[] biomeInts = new int[] { 0, 1, 2, 3 };
}