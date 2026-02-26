using UnityEngine;
using UnityEngine.Events;
using Core;

public class PlayerEnergy : MonoBehaviour
{
    [Header("Energy")]
    public float maxEnergy = 100f;
    public float currentEnergy = 100f;

    [Header("Regen")]
    public float idleDelay = 2f;        // wait after last energy use
    public float regenPerSecond = 2f;   // slow regen

    [Header("Events")]
    public UnityEvent<float, float> onEnergyChanged;

    private float lastUseTime;

    void Start()
    {
        // Load from PlayerData (0..1) if GameManager exists
        if (GameManager.Instance != null && GameManager.Instance.playerData != null)
        {
            currentEnergy = Mathf.Clamp01(GameManager.Instance.playerData.EnergyLevel) * maxEnergy;
        }
        else
        {
            currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        }

        lastUseTime = Time.time;
        SaveEnergy();
        onEnergyChanged?.Invoke(currentEnergy, maxEnergy);
    }

    void Update()
    {
        if (Time.time - lastUseTime >= idleDelay && currentEnergy < maxEnergy)
        {
            currentEnergy += regenPerSecond * Time.deltaTime;
            currentEnergy = Mathf.Min(currentEnergy, maxEnergy);

            SaveEnergy();
            onEnergyChanged?.Invoke(currentEnergy, maxEnergy);
        }
    }

    public bool Consume(float amount)
    {
        if (currentEnergy < amount) return false;

        currentEnergy -= amount;
        currentEnergy = Mathf.Max(0f, currentEnergy);
        lastUseTime = Time.time;

        SaveEnergy();
        onEnergyChanged?.Invoke(currentEnergy, maxEnergy);
        return true;
    }

    public void RestoreToFull()
    {
        currentEnergy = maxEnergy;

        SaveEnergy();
        onEnergyChanged?.Invoke(currentEnergy, maxEnergy);
    }

    private void SaveEnergy()
    {
        if (GameManager.Instance != null && GameManager.Instance.playerData != null)
        {
            GameManager.Instance.playerData.EnergyLevel = (maxEnergy <= 0f) ? 0f : (currentEnergy / maxEnergy);
        }
    }
}