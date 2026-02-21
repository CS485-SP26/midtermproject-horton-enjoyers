using System.Collections.Generic;
using UnityEngine;
using Environment;
using Farming;

namespace Core
{
    public class DailyQuestManager : MonoBehaviour
    {
        [SerializeField] private QuestDefinition[] questPool;
        [SerializeField] private int questsPerDay = 2;
        [SerializeField] private DayController dayController;
        [SerializeField] private FarmTileManager farmTileManager;
        [SerializeField] private QuestPanelUI questPanelUI;
        [SerializeField] private TMPro.TMP_Text fundsText;

        [SerializeField] private List<ActiveQuest> activeQuests = new List<ActiveQuest>();

        void OnEnable()
        {
            if (dayController != null)
            {
                dayController.dayEvaluationEvent.AddListener(OnDayEvaluation);
                dayController.dayPassedEvent.AddListener(OnDayPassed);
            }
        }

        void OnDisable()
        {
            if (dayController != null)
            {
                dayController.dayEvaluationEvent.RemoveListener(OnDayEvaluation);
                dayController.dayPassedEvent.RemoveListener(OnDayPassed);
            }
        }

        void Start()
        {
            Debug.Assert(dayController, "DailyQuestManager requires a DayController");
            Debug.Assert(farmTileManager, "DailyQuestManager requires a FarmTileManager");
            Debug.Assert(questPanelUI, "DailyQuestManager requires a QuestPanelUI");
            Debug.Assert(questPool != null && questPool.Length > 0, "DailyQuestManager requires at least one QuestDefinition in the pool");

            RollNewQuests();
        }

        /// <summary>Called by Farmer after successfully tilling a tile.</summary>
        public void NotifyTilled()
        {
            foreach (ActiveQuest quest in activeQuests)
            {
                if (quest.Definition.Type == QuestDefinition.QuestType.TillTiles)
                {
                    bool justCompleted = quest.AddProgress(1);
                    if (justCompleted) AwardQuestReward(quest);
                }
            }
            questPanelUI?.Refresh(activeQuests);
        }

        /// <summary>Called by Farmer after successfully watering a tile.</summary>
        public void NotifyWatered()
        {
            foreach (ActiveQuest quest in activeQuests)
            {
                if (quest.Definition.Type == QuestDefinition.QuestType.WaterTiles)
                {
                    bool justCompleted = quest.AddProgress(1);
                    if (justCompleted) AwardQuestReward(quest);
                }
            }
            questPanelUI?.Refresh(activeQuests);
        }

        /// <summary>Fires before tile degradation — evaluate KeepWatered quests here.</summary>
        private void OnDayEvaluation()
        {
            int wateredCount = farmTileManager != null ? farmTileManager.CountWateredTiles() : 0;
            Debug.Log($"DailyQuestManager: Day evaluation — {wateredCount} tiles watered.");

            foreach (ActiveQuest quest in activeQuests)
            {
                if (quest.Definition.Type == QuestDefinition.QuestType.KeepWatered)
                {
                    bool justCompleted = quest.SetProgress(wateredCount);
                    if (justCompleted) AwardQuestReward(quest);
                }
            }
            questPanelUI?.Refresh(activeQuests);
        }

        /// <summary>Fires after tile degradation — roll fresh quests for the new day.</summary>
        private void OnDayPassed()
        {
            RollNewQuests();
        }

        private void RollNewQuests()
        {
            activeQuests.Clear();

            if (questPool == null || questPool.Length == 0) return;

            // Fisher-Yates shuffle on a copy of the pool indices
            int[] indices = new int[questPool.Length];
            for (int i = 0; i < indices.Length; i++) indices[i] = i;

            for (int i = indices.Length - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                int tmp = indices[i];
                indices[i] = indices[j];
                indices[j] = tmp;
            }

            int count = Mathf.Min(questsPerDay, questPool.Length);
            for (int i = 0; i < count; i++)
            {
                activeQuests.Add(new ActiveQuest(questPool[indices[i]]));
            }

            Debug.Log($"DailyQuestManager: Rolled {activeQuests.Count} new quest(s).");
            questPanelUI?.Refresh(activeQuests);
        }

        private void AwardQuestReward(ActiveQuest quest)
        {
            GameManager.Instance.playerData.AddFunds(quest.Definition.Reward);
            if (fundsText != null)
                fundsText.SetText("Funds: ${0}", GameManager.Instance.playerData.Funds);
            Debug.Log($"DailyQuestManager: Quest '{quest.Definition.DisplayName}' complete! Awarded ${quest.Definition.Reward}.");
        }
    }
}
