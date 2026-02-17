using UnityEngine;

public class WateringCan : MonoBehaviour
{
    [SerializeField] private Transform handTransform;
    [SerializeField] private Transform playerTransform;
    private bool isPickedUp = false;
    [SerializeField] private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Assert(rb, "This wateringcan requires a Rigidbody.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPickedUp)
        {
            Debug.Log("Picked up");
            isPickedUp =true;
            rb.isKinematic = true;
            rb.detectCollisions = false;


            transform.SetParent(handTransform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            
        }
    }

    public void Drop()
    {
        if (isPickedUp)
        {
            isPickedUp = false;
            transform.SetParent(null);

            transform.position = playerTransform.position + playerTransform.forward * 1.5f;

            transform.rotation = Quaternion.identity;
            rb.isKinematic = false;
            rb.detectCollisions = true;
            rb.useGravity = true;
        };

    }
}
