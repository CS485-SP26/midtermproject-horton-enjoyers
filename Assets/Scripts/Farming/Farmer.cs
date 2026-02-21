using UnityEngine;
using Character;
using TMPro;
using Core;



namespace Farming{
    [RequireComponent(typeof(AnimatedController))]
    public class Farmer : MonoBehaviour
    {
        [SerializeField] private GameObject waterCan;
        [SerializeField] private GameObject gardenHoe;
        [SerializeField] private TMP_Text fundsText;
        [SerializeField] private ProgressBar waterBarUI;
        [SerializeField] private float waterPerUse = 0.2f;
        [SerializeField] private Core.DailyQuestManager questManager;
        [SerializeField] private GameManager gameManager;
        AnimatedController animatedController;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Debug.Assert(waterCan, "Farmer requires a waterCan.");
            Debug.Assert(gardenHoe, "Farmer requires a gardenHoe.");
            Debug.Assert(waterBarUI, "Farmer requires a waterLevel ProgressBar.");
            Debug.Assert(fundsText, "Farmer needs fundsText");
            animatedController = GetComponent<AnimatedController>();
            SetTool("None");
            waterBarUI.SetText("Water Level");
            waterBarUI.Fill = GameManager.Instance.playerData.WaterLevel;
            fundsText.text = "Funds: $" + GameManager.Instance.playerData.Funds;
        }


        public void SetTool(string tool)
            {
                Debug.Log("Recieved" + tool);
                waterCan.SetActive(false);
                gardenHoe.SetActive(false);
                switch (tool)
                {
                    case "GardenHoe": gardenHoe.SetActive(true); break;
                    case "WaterCan": waterCan.SetActive(true); break;
                }
            }

        public void RefillWater()
        {
            GameManager.Instance.playerData.AddWater(1f);
            waterBarUI.Fill = GameManager.Instance.playerData.WaterLevel;
        }

        public void TryFarming(FarmTile tile)
        {
            Debug.Log("Trying to farm");
            if (tile == null) return;
            Debug.Log("Condition" + tile.GetCondition);
            switch (tile.GetCondition)
                {
                    
                    case FarmTile.Condition.Grass:
                    {
                        tile.Interact();
                        animatedController.SetTrigger("Till");
                        questManager?.NotifyTilled();
                        break;
                    }
                    case FarmTile.Condition.Tilled:
                    {
                        if (GameManager.Instance.playerData.WaterLevel >= waterPerUse)
                        {
                            tile.Interact();
                            animatedController.SetTrigger("Water");
                            GameManager.Instance.playerData.UseWater(waterPerUse);
                            waterBarUI.Fill = GameManager.Instance.playerData.WaterLevel;
                            questManager?.NotifyWatered();
                        }
                        break;
                    }
                    default: break;
                }
            
        }
    }
}
