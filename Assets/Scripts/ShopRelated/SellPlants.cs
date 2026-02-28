using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellPlants : MonoBehaviour
{
    [SerializeField] private int pricePerPlant = 100;
    [SerializeField] private Button sellButton;
    [SerializeField] private TMP_Text plantCountText;

    private void Start()
    {
        sellButton.onClick.AddListener(OnSell);
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (sellButton != null)
            sellButton.onClick.RemoveListener(OnSell);
    }

    public void OnSell()
    {
        var data = GameManager.Instance.playerData;
        if (data.Plants <= 0) return;

        data.SellAllPlants(pricePerPlant);
        UpdateUI();
    }

    private void UpdateUI()
    {
        var data = GameManager.Instance.playerData;
        bool hasPlants = data.Plants > 0;
        sellButton.gameObject.SetActive(hasPlants);

        if (plantCountText != null)
            plantCountText.text = $"Plants: {data.Plants}  (+${data.Plants * pricePerPlant})";
    }
}
