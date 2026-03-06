using UnityEngine;
using TMPro;

namespace Environment
{
    public class SeasonManager : MonoBehaviour
    {
        public enum DayOfWeek { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }
        public enum Season { Spring, Summer, Fall, Winter }

        private const int DaysPerWeek = 7;
        private const int DaysPerSeason = 28; // 4 weeks per season

        [Header("UI")]
        [SerializeField] private TMP_Text seasonLabel;

        [Header("References")]
        [SerializeField] private DayController dayController;
        [SerializeField] private TMP_Text timeLabel;

        private float lastProgress = -1f;
        private int lastDay = -1;

        public Season CurrentSeason { get; private set; }
        public DayOfWeek CurrentDay { get; private set; }
        public int DayNumber { get; private set; }

        void Update()
        {
            UpdateTimeLabel();
            int day = dayController.CurrentDay;
            if (day == lastDay) return;
            lastDay = day;

            Recalculate(day);
            UpdateLabel();
        }


        private void Recalculate(int day)
        {
            // day 1 = Spring Monday
            int zeroBased = day - 1;
            int seasonIndex = (zeroBased / DaysPerSeason) % 4;
            int dayInWeek = zeroBased % DaysPerWeek;

            CurrentSeason = (Season)seasonIndex;
            CurrentDay = (DayOfWeek)dayInWeek;
            DayNumber = day;
        }

        private void UpdateLabel()
        {
            if (seasonLabel == null) return;

            seasonLabel.SetText(
                CurrentSeason.ToString() + " - " + CurrentDay.ToString() + " Day: {0}",
                DayNumber
            );
        }

        private void UpdateTimeLabel()
        {
            if (timeLabel == null || dayController == null) return;

            float progress = dayController.DayProgressPercent;
            float totalHours = (progress * 24f + 6f) % 24f; // offset: day starts at 6 AM
            int hour = (int)totalHours;
            int minute = (int)((totalHours - hour) * 60f);

            string period = hour >= 12 ? "PM" : "AM";
            int displayHour = hour % 12;
            if (displayHour == 0) displayHour = 12;

            timeLabel.SetText(displayHour + ":" + minute.ToString("D2") + " " + period);
        }


    }
}
