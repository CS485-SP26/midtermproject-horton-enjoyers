using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI fillText;
    [SerializeField] private float smoothSpeed = 8f;

    private float targetFill = 1f;

    public float Fill
    {
        set { targetFill = Mathf.Clamp01(value); }
    }

    private void Update()
    {
        if (fillImage == null) return;
        fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFill, Time.deltaTime * smoothSpeed);
    }

    public void SetText(string text)
    {
        if (fillText == null) return;
        fillText.text = text;
    }
}