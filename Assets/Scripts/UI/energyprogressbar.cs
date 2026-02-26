using UnityEngine;

public class energyprogressbar : MonoBehaviour
{
    [SerializeField] private PlayerEnergy energy;
    [SerializeField] private ProgressBar bar;
    [SerializeField] private bool showNumbers = true;

    void Start()
    {
        if (energy == null) energy = FindFirstObjectByType<PlayerEnergy>();
        if (bar == null) bar = GetComponent<ProgressBar>();

        if (energy != null)
            energy.onEnergyChanged.AddListener(OnEnergyChanged);

        // initialize UI
        if (energy != null) OnEnergyChanged(energy.currentEnergy, energy.maxEnergy);
    }

    private void OnEnergyChanged(float current, float max)
    {
        if (bar == null) return;

        bar.Fill = current / max;

        if (showNumbers)
            bar.SetText($"{current:0.0}/{max:0}");    }
}
