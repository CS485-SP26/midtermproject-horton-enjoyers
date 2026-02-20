using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager:MonoBehaviour
    {
        static private GameManager instance = null;
        private PlayerData playerData;
        [SerializeField] private int startingFunds = 100;
        [SerializeField] private int startingSeeds = 0;
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


        public void AddFunds(int value)
        {
            this.playerData.AddFunds(value);
        }

        public int GetFunds()
        {
            return this.playerData.Funds;
        }

        public bool SpendFunds(int amount)
        {
            return this.playerData.SpendFunds(amount);
        }

        public int GetSeeds()
        {
            return this.playerData.Seeds;
        }

        public void AddSeeds(int amount)
        {
            this.playerData.AddSeeds(amount);
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