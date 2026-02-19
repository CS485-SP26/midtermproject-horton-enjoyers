using TMPro;
using UnityEngine;
using Core;

public class QuestRowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questNameText;
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private GameObject completedBadge;

    public void Display(ActiveQuest quest)
    {
        if (questNameText != null)
            questNameText.text = quest.Definition.DisplayName;

        if (progressBar != null)
        {
            float fill = quest.Definition.Target > 0
                ? (float)quest.Progress / quest.Definition.Target
                : 0f;
            progressBar.Fill = Mathf.Clamp01(fill);
            progressBar.SetText($"{quest.Progress}/{quest.Definition.Target}");
        }

        if (completedBadge != null)
            completedBadge.SetActive(quest.IsCompleted);
    }
}
