using UnityEngine;
using TMPro;
using Core;

public class PlayerStatsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fundsText;
    [SerializeField] private TextMeshProUGUI seedCountText;

    [SerializeField] private TextMeshProUGUI plantsText;
    [SerializeField] private TextMeshProUGUI seasonText;

    void Update()
    {
        PlayerData.seedType type = GameManager.Instance.playerData.equippedSeed;
        if (fundsText != null)
            fundsText.text = "Funds: $" + GameManager.Instance.playerData.Funds;

        if (seedCountText != null)
        {
            
            switch(type)
            {
                case PlayerData.seedType.tomato: 
                    seedCountText.text = "Tomato Seeds: " + GameManager.Instance.playerData.tomatoSeeds; 
                    if (seasonText != null) seasonText.text = "All Season";
                    break;
                case PlayerData.seedType.cactus: 
                    seedCountText.text = "Cactus Seeds: " + GameManager.Instance.playerData.cactusSeeds; 
                    if (seasonText != null) seasonText.text = "Summer & Fall";
                    break;
                case PlayerData.seedType.cucumber: 
                    seedCountText.text = "Cucumber Seeds: " + GameManager.Instance.playerData.cucumberSeeds; 
                    if (seasonText != null) seasonText.text = "Spring & Summer";
                    break;
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
