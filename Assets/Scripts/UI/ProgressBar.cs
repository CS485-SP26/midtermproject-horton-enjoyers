using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private TextMeshProUGUI fillText;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        public float Fill { set { fillImage.fillAmount = value; } }

        public void SetText(string text)
        {
            fillText.text = text;
        }
        
    }

