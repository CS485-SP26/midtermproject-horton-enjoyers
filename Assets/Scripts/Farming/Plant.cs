using UnityEngine;
using Core;
using Environment;

namespace Farming
{
    public class Plant : MonoBehaviour
    {
        public enum PlantState
        {
            Planted,
            Growing,
            Mature,
            Withered
        }

        [Header("Plant Data")]
        [SerializeField] private PlantData plantData;

        private PlantState currentState;

        private int daysGrowing = 0;
        private float witherProgress = 0f;
        private int daysWatered = 0;

        private GameObject plantedModel;
        private GameObject growingModel;
        private GameObject matureModel;
        private GameObject witheredModel;

        public PlantState CurrentState => currentState; //can be useful in other scripts, like for quest evaluation
        public bool IsWithered => currentState == PlantState.Withered;
        public bool IsMature => currentState == PlantState.Mature;
        private SeasonManager seasonManager;

        private void Start()
        {
            seasonManager = FindAnyObjectByType<SeasonManager>();
            Debug.Assert(seasonManager, "Plant needs access to seasonManager");
        }

  
        public void SetPlantType(PlantData data)
        {
            plantData = data;
        }

        public void InitializeModels()
        {
            
            if (plantData == null)
            {
                Debug.LogError("PlantData missing on Plant! (ex. TomatoData)");
                return;
            }

            plantedModel = Instantiate(plantData.plantedModel, transform);
            growingModel = Instantiate(plantData.growingModel, transform);
            matureModel = Instantiate(plantData.matureModel, transform);
            witheredModel = Instantiate(plantData.witheredModel, transform);

            plantedModel.SetActive(false);
            growingModel.SetActive(false);
            matureModel.SetActive(false);
            witheredModel.SetActive(false);
        }

        public void ReceiveWater()
        {
            if (IsWithered || IsMature)
                return;

            witherProgress = 0f;
            daysWatered++;
        }

        public void OnDayPassed()
        {
            if (plantData == null) 
                return;

            if (IsWithered || IsMature)
                return;

            if (daysWatered > 0)
            {
                if (currentState == PlantState.Planted)
                    SetState(PlantState.Growing);

                if (daysGrowing < plantData.daysToMature)
                {
                    daysGrowing++;
                }

                if (daysGrowing >= plantData.daysToMature)
                {
                    SetState(PlantState.Mature);
                }

                daysWatered--;
            }
            else
            {
                switch(seasonManager.CurrentSeason)
                {
                    case SeasonManager.Season.Spring:
                    case SeasonManager.Season.Fall: 
                        witherProgress +=1f;
                        Debug.Log("Spring or Fall Withering"); 
                        break;
                    case SeasonManager.Season.Summer: witherProgress +=1.5f; break;
                    case SeasonManager.Season.Winter: witherProgress +=1.25f; break;
                }


                if (witherProgress >= plantData.daysBeforeWither)
                {
                    SetState(PlantState.Withered);
                }
            }
        }

        public void SetState(PlantState newState)
        {
            Debug.Log("Plant state changing from " + currentState + " to " + newState);
            currentState = newState;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (plantedModel == null) return;

            Debug.Log("UpdateVisual called, state = " + currentState);
            Debug.Log("matureModel active: " + matureModel + " | growingModel active: " + growingModel);
            
            plantedModel.SetActive(false);
            growingModel.SetActive(false);
            matureModel.SetActive(false);
            witheredModel.SetActive(false);

            switch (currentState)
            {
                case PlantState.Planted:
                    plantedModel.SetActive(true);
                    break;

                case PlantState.Growing:
                    growingModel.SetActive(true);
                    break;

                case PlantState.Mature:
                    matureModel.SetActive(true);
                    break;

                case PlantState.Withered:
                    witheredModel.SetActive(true);
                    break;
            }
        }

        public int Harvest()
        {
            // If it's withered → allow harvesting BUT give nothing
            if (currentState == PlantState.Withered)
            {
                Destroy(gameObject);
                return 0;
            }

            // Only give reward if Mature
            if (currentState != PlantState.Mature)
                return 0;

            // Add to NEW currency: Plants
            int amount = 1; // you can scale this later (e.g., plantData.yield)
            switch(plantData.plantName)
            {
                case "Tomato": GameManager.Instance.playerData.tomatoPlants += amount; break;
                case "Cactus": GameManager.Instance.playerData.cactusPlants += amount; break;
                case "Cucumber": GameManager.Instance.playerData.cucumberPlants += amount; break;
            }
            

            Destroy(gameObject);
            return amount;
        }

        public PlantSave GetSaveData()
        {
            PlantSave save = new PlantSave();

            save.hasPlant = true;
            save.plantName = plantData.plantName;
            save.growthStage = daysGrowing;   
            save.isWatered = daysWatered > 0;   
            save.isWithered = IsWithered;   
    

            return save;
        }

        public void LoadFromSaveData(PlantSave save, PlantData loadedPlantData)
        {
            if (!save.hasPlant)
                return;

            plantData = loadedPlantData;

            daysGrowing = save.growthStage;
            daysWatered = save.isWatered ? 1 : 0;
            witherProgress = save.isWatered ? 0 : witherProgress;

            InitializeModels();


            if (save.isWithered)
                SetState(PlantState.Withered);
            else if (daysGrowing >= plantData.daysToMature)
                SetState(PlantState.Mature);
            else if (daysGrowing > 0)
                SetState(PlantState.Growing);
            else
                SetState(PlantState.Planted);

        }
    }
}