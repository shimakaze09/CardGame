using UnityEngine;

public class DropZone : MonoBehaviour
{
    public bool IsWithinBounds(Vector3 position)
    {
        // Check if the position is within the bounds of the drop zone
        var bounds = GetComponent<Collider>().bounds;
        return bounds.Contains(position);
    }
}