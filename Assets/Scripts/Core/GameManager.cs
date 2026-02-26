using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Farming;

namespace Core
{
    [System.Serializable]
    public struct TileSave
    {
        public FarmTile.Condition condition;
        public int daysSinceLastInteraction;
        public PlantSave plantData;
    }

    [System.Serializable]
    public struct PlantSave
    {

        public bool hasPlant;
        public string plantName;
        public int growthStage;
        public bool isWatered;
        public bool isWithered;
    }

    [System.Serializable]
    public struct ActiveQuestSave
    {
        public QuestDefinition definition;
        public int progress;
        public bool isCompleted;
    }

    public class GameManager:MonoBehaviour
    {
        static private GameManager instance = null;
        public PlayerData playerData;
        [SerializeField] private int startingFunds = 100;
        [SerializeField] private int startingSeeds = 0;

        // --- Persistent game state ---
        public int savedDay = 1;
        public float savedDayProgress = 0f;
        public List<TileSave> savedTileStates = new List<TileSave>();
        public bool hasSavedTiles = false;
        public List<ActiveQuestSave> savedQuests = null;
        public int savedQuestDay = -1;
        static public GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                    DontDestroyOnLoad(go);
                    Debug.Log("Created a new GameManager");
                }
                return instance;
            }
            // no setter, read only!
        }




        void Awake()
        {
            if (GameManager.instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
                Debug.Log("GameManager set through Awake()");
                playerData = new PlayerData(startingFunds, startingSeeds);
            }
            else
            {
                Debug.Log("Duplicate GameManager attempted. Deleting new attempt");
                Destroy(this);
            }
        }
        public void LoadScenebyName(string name)
        {
            SceneManager.LoadScene(name);
        }
    }
}