using UnityEngine;

public class SeedStoreUI : MonoBehaviour
{
    public GameObject seedStorePanel;

    public void OpenStore()
    {
        seedStorePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseStore()
    {
        seedStorePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
