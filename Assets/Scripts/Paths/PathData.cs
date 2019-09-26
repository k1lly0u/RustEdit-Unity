using System.Linq;
using UnityEngine;
using UnityEditor;

public class PathData : MonoBehaviour
{
    public ProtoBuf.PathData serializedData;

    public PathType pathType;

    private static GameObject _node;

    private static GameObject Node
    {
        get
        {
            if (_node == null)
                _node = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Paths/PathNode.prefab");
            return _node;
        }
    }

    internal void Set(ProtoBuf.PathData serializedData)
    {
        this.serializedData = serializedData;

        pathType = GetTypeFromName();

        gameObject.name = this.serializedData.name = string.Concat(pathType, " ", transform.parent.GetDirectChildren().Where(x => x.GetComponent<PathData>().pathType == pathType).Count());
       
        foreach (ProtoBuf.VectorData point in serializedData.nodes)
            AddNode(point, -1);
    }

    public ProtoBuf.PathData GetPathData()
    {
        if (serializedData == null)
            serializedData = PathManager.CreateEmptyPathData(GetTypeFromName());

        serializedData.nodes = new ProtoBuf.VectorData[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            serializedData.nodes[i] = transform.GetChild(i).transform.position;

        return serializedData;
    }

    public GameObject AddNode(Vector3 position, int index = -1)
    {
        GameObject pathNode = Instantiate(Node, position, Quaternion.identity);

        pathNode.AddComponent<PathNode>();

        pathNode.transform.SetParent(gameObject.transform);

        if (index != -1)
            pathNode.transform.SetSiblingIndex(index);
       
        return pathNode;
    }

    public int GetPointCount() => transform.childCount;

    public Vector3 GetVectorAt(int index) => transform.GetChild(index).position;

    private PathType GetTypeFromName()
    {
        if (name.StartsWith("River"))
            return PathType.River;
        if (name.StartsWith("Powerline"))
            return PathType.Powerline;
        return PathType.Road;
    }
}
