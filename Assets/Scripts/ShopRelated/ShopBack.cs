using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopBackButton : MonoBehaviour
{
    [SerializeField] private string farmingSceneName = "Scene1-FarmingSim";

    public void BackToFarm()
    {
        SceneManager.LoadScene(farmingSceneName);
    }
}
