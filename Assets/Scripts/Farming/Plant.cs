using UnityEngine;

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

        private PlantState currentState = PlantState.Planted;

        private int daysGrowing = 0;
        private int daysWithoutWater = 0;
        private int daysWatered = 0;

        private GameObject plantedModel;
        private GameObject growingModel;
        private GameObject matureModel;
        private GameObject witheredModel;

        public bool IsWithered => currentState == PlantState.Withered;
        public bool IsMature => currentState == PlantState.Mature;

        private void Start()
        {
            InitializeModels();
            SetState(PlantState.Planted);
        }

        private void InitializeModels()
        {
            if (plantData == null)
            {
                Debug.LogError("PlantData missing on Plant!");
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

            daysWithoutWater = 0;
            daysWatered++;
        }

        public void OnDayPassed()
        {
            if (IsWithered || IsMature)
                return;

            if (daysWatered > 0)
            {
                daysWatered--;

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
            }
            else
            {
                daysWithoutWater++;

                if (daysWithoutWater >= 2)
                {
                    SetState(PlantState.Withered);
                }
            }
        }

        private void SetState(PlantState newState)
        {
            currentState = newState;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (plantedModel == null) return;

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
        /*
        public int Harvest()
        {
            if (currentState != PlantState.Mature)
                return 0;

            int value = plantData.sellValue;
            Destroy(gameObject);
            return value;
        }
        */
    }
}