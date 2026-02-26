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


        [SerializeField] private PlayerEnergy energy;
        [SerializeField] private float tillEnergyCost = 20f;   // tweak
        [SerializeField] private float waterEnergyCost = 10f;  // tweak



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
        
            if (energy == null)
                energy = GetComponent<PlayerEnergy>();

            Debug.Assert(energy, "Farmer requires PlayerEnergy (add PlayerEnergy to the same GameObject or assign it).");        
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
                        // Energy check for tilling
                        if (!energy.Consume(tillEnergyCost))
                        {
                            Debug.Log("Not enough energy to till.");
                            return;
                        }

                        tile.Interact();
                        animatedController.SetTrigger("Till");
                        questManager?.NotifyTilled();
                        break;
                    }

                    case FarmTile.Condition.Tilled:
                    {

                        // Must have enough water first
                        if (GameManager.Instance.playerData.WaterLevel >= waterPerUse)
                        if (GameManager.Instance.playerData.Seeds > 0)
                        {
                            if (tile.Planting())
                            {
                                GameManager.Instance.playerData.UseSeed();
                                // Optional planting animation later
                                // animatedController.SetTrigger("Plant");
                            }
                        }
                        else
                        {
                            Debug.Log("No seeds available.");
                        }

                        break;
                    }
                    case FarmTile.Condition.Planted_Dry:
                    {
                        if (tile.GetPlant().IsWithered)
                        {
                            tile.Interact();
                            animatedController.SetTrigger("Till");
                        } 
                        else if (GameManager.Instance.playerData.WaterLevel >= waterPerUse)
                        {
                            // Energy check for watering
                            if (!energy.Consume(waterEnergyCost))
                            {
                                Debug.Log("Not enough energy to water.");
                                return;
                            }

                            tile.Interact();
                            animatedController.SetTrigger("Water");
                            GameManager.Instance.playerData.UseWater(waterPerUse);
                            waterBarUI.Fill = GameManager.Instance.playerData.WaterLevel;
                            questManager?.NotifyWatered();
                        }
                        else
                        {
                            Debug.Log("Not enough water to water.");
                        }

                        break;
                     }
                            default:
                            break;
                }
            }
    }
}
