using UnityEngine;
using Character;
using TMPro;
using Core;



namespace Farming{
    [RequireComponent(typeof(AnimatedController))]
    public class Farmer : MonoBehaviour
    {
        [SerializeField] private TMP_Text fundsText;
        [SerializeField] private ProgressBar waterBarUI;
        [SerializeField] private float waterPerUse = 0.2f;
        [SerializeField] private Core.DailyQuestManager questManager;
        [SerializeField] private GameManager gameManager;


        [SerializeField] private PlayerEnergy energy;
        [SerializeField] private float tillEnergyCost = 20f;   // tweak
        [SerializeField] private float waterEnergyCost = 10f;  // tweak



        AnimatedController animatedController;

        public ToolType ActiveTool { get; private set; } = ToolType.None;

        public void SetActiveTool(ToolType type)
        {
            ActiveTool = type;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Debug.Assert(waterBarUI, "Farmer requires a waterLevel ProgressBar.");
            Debug.Assert(fundsText, "Farmer needs fundsText");
            animatedController = GetComponent<AnimatedController>();
            waterBarUI.SetText("Water Level");
            waterBarUI.Fill = GameManager.Instance.playerData.WaterLevel;
            fundsText.text = "Funds: $" + GameManager.Instance.playerData.Funds;
        
            if (energy == null)
                energy = GetComponent<PlayerEnergy>();

            Debug.Assert(energy, "Farmer requires PlayerEnergy (add PlayerEnergy to the same GameObject or assign it).");        
        }


        public void RefillWater()
        {
            if (ActiveTool != ToolType.WaterCan)
            {
                Debug.Log("Need to be holding the Watering Can to refill.");
                return;
            }
            GameManager.Instance.playerData.AddWater(1f);
            waterBarUI.Fill = GameManager.Instance.playerData.WaterLevel;
        }

        public void TryFarming(FarmTile tile)
            {
                Debug.Log("Trying to farm");
                if (tile == null) return;

                Plant plant = tile.GetPlant();
                if (plant != null && (plant.IsMature || plant.IsWithered)) { 
                    plant.Harvest(); 
                    tile.ClearPlantReference();
                    return; 
                } 
                Debug.Log("Condition" + tile.GetCondition);

                switch (tile.GetCondition)
                {
                    case FarmTile.Condition.Grass:
                    {
                        if (ActiveTool != ToolType.GardenHoe)
                        {
                            Debug.Log("Need the Garden Hoe to till.");
                            return;
                        }

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
                        if (ActiveTool == ToolType.WaterCan)
                        {
                            Debug.Log("Can't plant while holding the Watering Can.");
                            return;
                        }

                        if (GameManager.Instance.playerData.getEquippedSeedCount(GameManager.Instance.playerData.equippedSeed) > 0)
                        {
                            if (tile.Planting(GameManager.Instance.playerData.equippedSeed))
                            {
                                GameManager.Instance.playerData.UseSeed(GameManager.Instance.playerData.equippedSeed);
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
                            if (ActiveTool != ToolType.GardenHoe)
                            {
                                Debug.Log("Need the Garden Hoe to remove a withered plant.");
                                return;
                            }
                            tile.Interact();
                            animatedController.SetTrigger("Till");
                        }
                        else
                        {
                            if (ActiveTool != ToolType.WaterCan)
                            {
                                Debug.Log("Need the Watering Can to water plants.");
                                return;
                            }

                            if (GameManager.Instance.playerData.WaterLevel < waterPerUse)
                            {
                                Debug.Log("Not enough water.");
                                return;
                            }

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
                        break;
                    }

                    default:
                        break;
                }
            }
    }
}
