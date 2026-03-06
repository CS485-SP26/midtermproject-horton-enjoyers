using UnityEngine;
using TMPro;
using Core;

public class PlayerStatsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fundsText;
    [SerializeField] private TextMeshProUGUI seedCountText;

    [SerializeField] private TextMeshProUGUI plantsText;

    void Update()
    {
        PlayerData.seedType type = GameManager.Instance.playerData.equippedSeed;
        if (fundsText != null)
            fundsText.text = "Funds: $" + GameManager.Instance.playerData.Funds;

        if (seedCountText != null)
        {
            
            switch(type)
            {
                case PlayerData.seedType.tomato: seedCountText.text = "Tomato Seeds: " + GameManager.Instance.playerData.tomatoSeeds; break;
                case PlayerData.seedType.cactus: seedCountText.text = "Cactus Seeds: " + GameManager.Instance.playerData.cactusSeeds; break;
                case PlayerData.seedType.cucumber: seedCountText.text = "Cucumber Seeds: " + GameManager.Instance.playerData.cucumberSeeds; break;
            }
        }

        if (plantsText != null)
        {
            switch(type)
            {
                case PlayerData.seedType.tomato: plantsText.text = "Tomato plants: " + GameManager.Instance.playerData.tomatoPlants; break;
                case PlayerData.seedType.cactus: plantsText.text = "Cactus plants: " + GameManager.Instance.playerData.cactusPlants; break;
                case PlayerData.seedType.cucumber: plantsText.text = "Cucumber plants: " + GameManager.Instance.playerData.cucumberPlants; break;
            }
        }
    }
}
