using System.Collections.Generic;
using UnityEngine;
using Core;

public class QuestPanelUI : MonoBehaviour
{
    [SerializeField] private QuestRowUI[] questRows;

    public void Refresh(List<ActiveQuest> quests)
    {
        if (questRows == null) return;

        for (int i = 0; i < questRows.Length; i++)
        {
            if (questRows[i] == null) continue;

            if (i < quests.Count)
            {
                questRows[i].gameObject.SetActive(true);
                questRows[i].Display(quests[i]);
            }
            else
            {
                questRows[i].gameObject.SetActive(false);
            }
        }
    }
}
