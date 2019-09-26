using UnityEngine;

[SelectionBase]
public class PrefabData : MonoBehaviour
{
    internal string category;

    internal ProtoBuf.PrefabData GetPrefabData()
    {
        ProtoBuf.PrefabData data = new ProtoBuf.PrefabData();
        data.id = StringPool.Get(gameObject.name);
        
        data.category = string.IsNullOrEmpty(category) ? "Decor" : category;

        Vector3 position = gameObject.transform.position;
        Vector3 rotation = gameObject.transform.rotation.eulerAngles;
        Vector3 scale = gameObject.transform.localScale;

        data.position = new ProtoBuf.VectorData() { x = position.x, y = position.y, z = position.z };
        data.rotation = new ProtoBuf.VectorData() { x = rotation.x, y = rotation.y, z = rotation.z };
        data.scale = new ProtoBuf.VectorData() { x = scale.x, y = scale.y, z = scale.z };

        return data;
    }
}
