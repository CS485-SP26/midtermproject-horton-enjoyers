using UnityEngine;
using UnityEngine.UI;

public class energyprogressbar : MonoBehaviour
{
    [SerializeField] private PlayerEnergy energy;
    [SerializeField] private ProgressBar bar;
    [SerializeField] private Image fillImage;   // <-- ADD THIS
    [SerializeField] private bool showNumbers = true;

    [SerializeField] private Color greenColor = new Color(0.2f, 0.8f, 0.2f);
    [SerializeField] private Color yellowColor = new Color(1f, 0.8f, 0.2f);
    [SerializeField] private Color redColor = new Color(0.9f, 0.2f, 0.2f);

    void Start()
    {
        if (energy == null) energy = FindFirstObjectByType<PlayerEnergy>();
        if (bar == null) bar = GetComponent<ProgressBar>();

        if (energy != null)
            energy.onEnergyChanged.AddListener(OnEnergyChanged);

        if (energy != null) OnEnergyChanged(energy.currentEnergy, energy.maxEnergy);
    }

    private void OnEnergyChanged(float current, float max)
    {
        if (bar == null) return;

        float percent = current / max;
        bar.Fill = percent;

        if (showNumbers)
            bar.SetText($"{current:0.0}/{max:0}");

        // 🔥 ADD COLOR LOGIC HERE
        if (fillImage != null)
        {
            if (percent > 0.7f)
                fillImage.color = greenColor;
            else if (percent > 0.3f)
                fillImage.color = yellowColor;
            else
                fillImage.color = redColor;
        }
    }
}