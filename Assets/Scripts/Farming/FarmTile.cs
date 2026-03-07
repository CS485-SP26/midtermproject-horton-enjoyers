using System.Collections.Generic;
using UnityEngine;
using Environment;
using Farming;
using Core;
using System.Linq;

namespace Farming 
{
    public class FarmTile : MonoBehaviour
    {
        public enum Condition { Grass, Tilled, Watered, Planted_Dry, Planted_Wet }

        [SerializeField] private Condition tileCondition = Condition.Grass; 

        [Header("Visuals")]
        [SerializeField] private Material grassMaterial;
        [SerializeField] private Material tilledMaterial;
        [SerializeField] private Material wateredMaterial;
        MeshRenderer tileRenderer;

        [Header("Planting")]
        [SerializeField] private Plant plantPrefab;
        private Plant currentPlant;
        [SerializeField] private PlantData tomatoData;
        [SerializeField] private PlantData cactusData;
        [SerializeField] private PlantData cucumberData;


        [Header("Audio")]
        [SerializeField] private AudioSource stepAudio;
        [SerializeField] private AudioSource tillAudio;
        [SerializeField] private AudioSource waterAudio;

        List<Material> materials = new List<Material>();

        private int daysSinceLastInteraction = 0;
        public FarmTile.Condition GetCondition { get { return tileCondition; } }

        public int DaysSinceLastInteraction => daysSinceLastInteraction;
        public bool HasPlant => currentPlant != null;
        private SeasonManager seasonManager;

        void Awake()
        {
            tileRenderer = GetComponent<MeshRenderer>();
        }

        void Start()
        {
            seasonManager = FindAnyObjectByType<SeasonManager>();
            Debug.Assert(seasonManager, "FarmTile needs access to seasonManager");
            Debug.Assert(tileRenderer, "FarmTile requires a MeshRenderer");

            foreach (Transform edge in transform)
            {
                materials.Add(edge.gameObject.GetComponent<MeshRenderer>().material);
            }
        }

        public void Interact()
        {
            switch(tileCondition)
            {
                case FarmTile.Condition.Grass: Till(); break;
                case FarmTile.Condition.Tilled: break;
                //case FarmTile.Condition.Watered: Debug.Log("Ready for planting"); break;
                case FarmTile.Condition.Planted_Dry:
                {
                    if (currentPlant != null && currentPlant.IsWithered)
                    {
                        RemovePlant();
                        Till();
                    }
                    else
                    {
                        Water();
                    }
                    
                    break;
                }
            }
            daysSinceLastInteraction = 0;
        }

        public void Till()
        {
            tileCondition = FarmTile.Condition.Tilled;
            UpdateVisual();
            tillAudio?.Play();
        }

        public void ClearPlantReference()
        {
            currentPlant = null;
            tileCondition = FarmTile.Condition.Grass;
            UpdateVisual();
            // set condition back to Tilled ?
        }
        public void Water()
        {
            if (tileCondition == Condition.Planted_Dry && currentPlant != null)
            {
                currentPlant.ReceiveWater();
                tileCondition = FarmTile.Condition.Planted_Wet;
                UpdateVisual();
                waterAudio?.Play();
                return;
            }
            /*
            tileCondition = FarmTile.Condition.Watered;
            UpdateVisual();
            waterAudio?.Play();
            */
        }

        private void UpdateVisual()
        {
            if(tileRenderer == null) return;
            switch(tileCondition)
            {
                case FarmTile.Condition.Grass: tileRenderer.material = grassMaterial; break;
                case FarmTile.Condition.Tilled: tileRenderer.material = tilledMaterial; break;
                case FarmTile.Condition.Watered: tileRenderer.material = wateredMaterial; break;
                case FarmTile.Condition.Planted_Dry: tileRenderer.material = tilledMaterial; break;
                case FarmTile.Condition.Planted_Wet: tileRenderer.material = wateredMaterial; break;
            }
            Debug.Log("Condition = " +tileCondition + "tilematerial = " + tileRenderer.material);
        }

        public void SetHighlight(bool active)
        {
            foreach (Material m in materials)
            {
                if (active)
                {
                    m.EnableKeyword("_EMISSION");
                } 
                else 
                {
                    m.DisableKeyword("_EMISSION");
                }
            }
            if (active) stepAudio.Play();
        }



        public void OnDayPassed()
        {
            daysSinceLastInteraction++;
            if (currentPlant != null)
            {
                currentPlant.OnDayPassed();

                // Soil dries after 1 day
                if (tileCondition == FarmTile.Condition.Planted_Wet  && !currentPlant.IsMature)
                {
                    tileCondition = FarmTile.Condition.Planted_Dry;
                    UpdateVisual();
                }
                return;
            }
            else if(daysSinceLastInteraction >= 2)
            {
                if(tileCondition == FarmTile.Condition.Watered) tileCondition = FarmTile.Condition.Tilled;
                else if(tileCondition == FarmTile.Condition.Tilled) tileCondition = FarmTile.Condition.Grass;
            }
            UpdateVisual();
        }

        public Plant GetPlant()
        {
            return currentPlant;
        }

        public bool Planting(PlayerData.seedType seedType)
        {
            if (tileCondition != Condition.Tilled || currentPlant != null)
                return false;

            if (plantPrefab == null)
                return false;
            
            bool cactusCheck = cactusData.growSeasons.Contains(seasonManager.CurrentSeason);
            bool cucumberCheck = cucumberData.growSeasons.Contains(seasonManager.CurrentSeason);
            if (seedType == PlayerData.seedType.cactus && !cactusCheck)
            {
                Debug.Log("Cacti can only be planted in the summer or fall");
                return false;
            }
            if (seedType == PlayerData.seedType.cucumber && !cucumberCheck)
            {
                Debug.Log("Cucumber can only be planted in the spring or summer");
                return false;
            }
            
            currentPlant = Instantiate(plantPrefab, transform);
            currentPlant.transform.localPosition = new Vector3(0, -5f, 0);
            currentPlant.transform.localRotation = Quaternion.identity;
            currentPlant.transform.localScale = new Vector3(1f, 20f, 1f);  // fixes weird inherited scaling from farmtile

            switch(seedType)
            {
                case PlayerData.seedType.tomato: currentPlant.SetPlantType(tomatoData); break;
                case PlayerData.seedType.cactus: currentPlant.SetPlantType(cactusData); break;
                case PlayerData.seedType.cucumber: currentPlant.SetPlantType(cucumberData); break;
            }
            currentPlant.InitializeModels();
            currentPlant.SetState(Plant.PlantState.Planted);
            tileCondition = Condition.Planted_Dry;

            return true;
        }

        private void RemovePlant()
        {
            if (currentPlant != null)
            {
                Destroy(currentPlant.gameObject);
                currentPlant = null;
                tileCondition = FarmTile.Condition.Tilled;
                UpdateVisual();
            }
        }

        public TileSave GetSaveData()
        {
            TileSave save = new TileSave();

            save.condition = tileCondition;
            save.daysSinceLastInteraction = daysSinceLastInteraction;

            if (currentPlant != null)
            {
                save.plantData = currentPlant.GetSaveData();
                save.plantData.hasPlant = true;
            }
            else
            {
                save.plantData = new PlantSave { hasPlant = false };
            }

            return save;
        }

        

        public void RestoreState(TileSave save)
        {
            tileCondition = save.condition;
            daysSinceLastInteraction = save.daysSinceLastInteraction;

            if (save.plantData.hasPlant)
            {
                PlantData dataToLoad;
                switch (save.plantData.plantName)
                {
                    case "Tomato":
                        dataToLoad = tomatoData;
                        break;
                    case "Cactus":
                        dataToLoad = cactusData;
                        break;
                    case "Cucumber":
                        dataToLoad = cucumberData;
                        break;
                    default:
                        Debug.Log("Unknown plant: " + save.plantData.plantName);
                        return;
                }

                currentPlant = Instantiate(plantPrefab, transform);

                currentPlant.transform.localPosition = new Vector3(0, -5f, 0);
                currentPlant.transform.localRotation = Quaternion.identity;
                currentPlant.transform.localScale = new Vector3(1f, 20f, 1f);

                // Restore the plant's internal state
                currentPlant.LoadFromSaveData(save.plantData, dataToLoad);
            }

            UpdateVisual();
        }
    }
}