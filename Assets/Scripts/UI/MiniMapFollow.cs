using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float height = 30f;
    [SerializeField] private Vector3 offset = Vector3.zero;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 p = target.position + offset;
        transform.position = new Vector3(p.x, height, p.z);
    }
}
