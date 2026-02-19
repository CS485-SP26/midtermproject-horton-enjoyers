using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopTrigger : MonoBehaviour
{
    [SerializeField] private GameObject enterStoreButton;

    private bool isNearShop = false;

    private void Start()
    {
        if (enterStoreButton != null)
            enterStoreButton.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shop"))
        {
            isNearShop = true;
            enterStoreButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shop"))
        {
            isNearShop = false;
            enterStoreButton.SetActive(false);
        }
    }

    public void EnterShop()
    {
        if (isNearShop)
        {
            SceneManager.LoadScene("Scene2-Shop");
        }
    }
}
