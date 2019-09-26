using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainManager))]
public class TerrainManagerEditor : Editor
{
    private PaintType paintType;
    private TerrainTopology.Enum topologyType;

    private void OnEnable()
    {
        TerrainManager instance = (TerrainManager)target;

        paintType = instance.PaintMode;
        topologyType = instance.SelectedTopology;
    }

    public override void OnInspectorGUI()
    {
        TerrainManager instance = (TerrainManager)target;

        if (!TerrainManager.HasValidTerrain())
        {
            GUILayout.Label("Create or load a map to continue...");
            return;
        }

        GUILayout.Label("Paint Controls", EditorStyles.boldLabel);

        GUILayout.Space(5f);

        paintType = (PaintType)EditorGUILayout.EnumPopup("Paint Mode", paintType);
        if (paintType != instance.PaintMode)
        {
            instance.ChangePaintType(paintType);
            return;
        }

        if (paintType == PaintType.Topology)
        {
            topologyType = (TerrainTopology.Enum)EditorGUILayout.EnumPopup("Topology Layer", topologyType);
            if (topologyType != instance.SelectedTopology)
                instance.ChangeTopologyLayer(topologyType);
        }
    }
}
