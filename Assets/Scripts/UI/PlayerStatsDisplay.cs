using UnityEngine;
using TMPro;
using Core;

public class PlayerStatsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fundsText;
    [SerializeField] private TextMeshProUGUI seedCountText;

    void Update()
    {
        if (fundsText != null)
            fundsText.text = "Funds: " + GameManager.Instance.playerData.Funds;

        if (seedCountText != null)
            seedCountText.text = "Seeds: " + GameManager.Instance.playerData.Seeds;
    }
}
