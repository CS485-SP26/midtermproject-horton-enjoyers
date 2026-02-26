using UnityEngine;
using UnityEngine.UI;
using Core;

public class PurchaseEnergyDrink : MonoBehaviour
{
    [SerializeField] private int cost = 25;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private PlayerEnergy playerEnergy; // assign in Inspector (recommended)

    void Start()
    {
        purchaseButton.onClick.AddListener(OnPurchase);

        if (playerEnergy == null)
            playerEnergy = FindFirstObjectByType<PlayerEnergy>();

        UpdateUI();
    }

    void OnDestroy()
    {
        if (purchaseButton != null)
            purchaseButton.onClick.RemoveListener(OnPurchase);
    }

    public void OnPurchase()
    {
        if (GameManager.Instance.playerData.SpendFunds(cost))
        {
            if (playerEnergy == null)
                playerEnergy = FindFirstObjectByType<PlayerEnergy>();

            if (playerEnergy != null)
                playerEnergy.RestoreToFull();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        bool canAfford = GameManager.Instance.playerData.Funds >= cost;
        purchaseButton.gameObject.SetActive(canAfford);
    }
}