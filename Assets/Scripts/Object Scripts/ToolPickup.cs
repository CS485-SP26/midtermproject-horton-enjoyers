using UnityEngine;
using Character;
using Farming;

/// <summary>
/// Attach to any world-space tool object (watering can, garden hoe, etc.).
/// Set toolType in the Inspector. The player picks it up with F and drops it with F.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ToolPickup : MonoBehaviour
{
    [SerializeField] public ToolType toolType;

    [Header("Hand Alignment")]
    [SerializeField] private Vector3 pickupPositionOffset = Vector3.zero;
    [SerializeField] private Vector3 pickupRotationOffset = Vector3.zero;

    private Rigidbody rb;
    private bool isPickedUp = false;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPickedUp) return;
        PlayerController pc = other.GetComponentInParent<PlayerController>();
        if (pc != null) pc.SetNearbyTool(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPickedUp) return;
        PlayerController pc = other.GetComponentInParent<PlayerController>();
        if (pc != null) pc.SetNearbyTool(null);
    }

    public void Pickup(Transform hand, Farmer farmer)
    {
        isPickedUp = true;
        rb.isKinematic = true;
        rb.detectCollisions = false;

        transform.SetParent(hand);
        transform.localPosition = pickupPositionOffset;
        transform.localRotation = Quaternion.Euler(pickupRotationOffset);

        farmer.SetActiveTool(toolType);
    }

    public void Drop(Vector3 dropPosition, Farmer farmer)
    {
        isPickedUp = false;
        transform.SetParent(null);
        transform.position = dropPosition;
        transform.rotation = Quaternion.identity;

        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.up * 2f;

        farmer.SetActiveTool(ToolType.None);
    }

    // Parameterless overload for legacy callers (e.g. HW2)
    public void Drop()
    {
        isPickedUp = false;
        transform.SetParent(null);
        transform.rotation = Quaternion.identity;

        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.up * 2f;
    }
}
