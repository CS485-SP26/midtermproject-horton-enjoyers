using UnityEngine;

namespace Core
{
    [CreateAssetMenu(menuName = "Quests/Quest Definition")]
    public class QuestDefinition : ScriptableObject
    {
        public enum QuestType { TillTiles, WaterTiles, KeepWatered }

        [SerializeField] private QuestType questType;
        [SerializeField] private int targetCount = 3;
        [SerializeField] private int rewardFunds = 30;
        [SerializeField] private string displayName;

        public QuestType Type => questType;
        public int Target => targetCount;
        public int Reward => rewardFunds;
        public string DisplayName => displayName;
    }
}
