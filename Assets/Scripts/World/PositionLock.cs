using UnityEngine;

[ExecuteInEditMode]
public class PositionLock : MonoBehaviour
{
    private Transform Transform;
    private Vector3 Position;

    private void OnEnable()
    {
        Transform = transform;
        Position = Transform.position;
    }

    private void Update()
    {
        if (Transform.position != Position)
            Transform.position = Position;
    }
}
