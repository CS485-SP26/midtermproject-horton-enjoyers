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
        int plantCount = data.tomatoPlants + data.cactusPlants + data.cucumberPlants;
        if (plantCount <= 0) return;

        data.SellAllPlants(sellTotal());
        UpdateUI();
    }

    private void UpdateUI()
    {
        var data = GameManager.Instance.playerData;
        int plantCount = data.tomatoPlants + data.cactusPlants + data.cucumberPlants;
        bool hasPlants = plantCount > 0;
        sellButton.gameObject.SetActive(hasPlants);

        if (plantCountText != null)
            plantCountText.text = $"Plants: {plantCount}  (+${sellTotal()})";
    }

    public int sellTotal()
    {
        var data = GameManager.Instance.playerData;
        int total = 
            data.tomatoPlants * GameBalance.TomatoPlantValue +
            data.cactusPlants * GameBalance.CactusPlantValue + 
            data.cucumberPlants * GameBalance.CucumberPlantValue;
        return total;
    }
}
