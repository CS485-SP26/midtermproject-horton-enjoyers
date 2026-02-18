using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager:MonoBehaviour
    {
        static private GameManager instance = null;

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

        // TODO Refactor to struct or data class
        int funds = 20;

        public void AddFunds(int value)
        {
            this.funds += value;
        }

        public int GetFunds()
        {
            return this.funds;
        }

        void Awake()
        {
            if (GameManager.instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
                Debug.Log("GameManager set through Awake()");
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