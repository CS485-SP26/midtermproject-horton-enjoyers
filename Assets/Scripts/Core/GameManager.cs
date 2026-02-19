using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager:MonoBehaviour
    {
        static private GameManager instance = null;
        private PlayerData playerData;
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

        void Awake()
        {
            if (GameManager.instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
                Debug.Log("GameManager set through Awake()");
                playerData = new PlayerData(0);
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