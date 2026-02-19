using System;

namespace Core
{
    [Serializable]
    public class ActiveQuest
    {
        private QuestDefinition definition;
        private int currentProgress;
        private bool isCompleted;

        public QuestDefinition Definition => definition;
        public int Progress => currentProgress;
        public bool IsCompleted => isCompleted;

        public ActiveQuest(QuestDefinition definition)
        {
            this.definition = definition;
            currentProgress = 0;
            isCompleted = false;
        }

        /// <summary>Increments progress by amount. Returns true the moment the quest completes.</summary>
        public bool AddProgress(int amount)
        {
            if (isCompleted) return false;
            currentProgress += amount;
            if (currentProgress >= definition.Target)
            {
                currentProgress = definition.Target;
                isCompleted = true;
                return true;
            }
            return false;
        }

        /// <summary>Sets progress to an absolute value (used for KeepWatered). Returns true the moment the quest completes.</summary>
        public bool SetProgress(int count)
        {
            if (isCompleted) return false;
            currentProgress = count;
            if (currentProgress >= definition.Target)
            {
                currentProgress = definition.Target;
                isCompleted = true;
                return true;
            }
            return false;
        }
    }
}
