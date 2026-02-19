using UnityEngine;
using Farming;
using Character;

public class Well : MonoBehaviour, IInteractable
{
    [SerializeField] private Farmer farmer;

    public void Interact()
    {
        farmer.RefillWater();
        Debug.Log("Well: watering can refilled.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerController>()?.SetNearbyInteractable(this);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerController>()?.SetNearbyInteractable(null);
    }
}
