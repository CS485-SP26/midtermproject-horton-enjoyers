using UnityEngine;
using UnityEngine.UI;
using Core;

public class PurchaseSeeds : MonoBehaviour
{
    [SerializeField] private int cost = 50;
    [SerializeField] private int seedsPerPurchase = 1;
    [SerializeField] private Button purchaseButton;

    void Start()
    {
        purchaseButton.onClick.AddListener(OnPurchase);
        UpdateUI();
    }

    void OnDestroy()
    {
        if (purchaseButton != null)
            purchaseButton.onClick.RemoveListener(OnPurchase);
    }

    public void OnPurchase()
    {
        if (GameManager.Instance.SpendFunds(cost))
        {
            GameManager.Instance.AddSeeds(seedsPerPurchase);
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        bool canAfford = GameManager.Instance.GetFunds() >= cost;
        purchaseButton.gameObject.SetActive(canAfford);
    }
}
