using UnityEngine;
using Character;



namespace Farming{
    [RequireComponent(typeof(AnimatedController))]
    public class Farmer : MonoBehaviour
    {
        [SerializeField] private GameObject waterCan;
        [SerializeField] private GameObject gardenHoe;
        [SerializeField] private ProgressBar waterBarUI;
        [SerializeField] private float waterLevel = 1f;
        [SerializeField] private float waterPerUse = 0.2f;
        AnimatedController animatedController;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Debug.Assert(waterCan, "Farmer requires a waterCan.");
            Debug.Assert(gardenHoe, "Farmer requires a gardenHoe.");
            Debug.Assert(waterBarUI, "Farmer requires a waterLevel ProgressBar.");
            animatedController = GetComponent<AnimatedController>();
            SetTool("None");
            waterBarUI.SetText("Water Level");
            waterBarUI.Fill = waterLevel;
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
                        break;
                    }
                    case FarmTile.Condition.Tilled: 
                    {
                        if (waterLevel >= waterPerUse)
                        {
                            tile.Interact();
                            animatedController.SetTrigger("Water");
                            waterLevel -= waterPerUse;
                            waterBarUI.Fill = waterLevel;
                        } 
                        break;
                    }
                    default: break;
                }
            
        }
    }
}
