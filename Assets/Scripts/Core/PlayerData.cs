using UnityEngine;


namespace Core
{
    [System.Serializable]
    public class PlayerData
    {

        public int Funds { get; private set; }
        public int Seeds { get; private set; }
        public float WaterLevel { get; private set; }
        public PlayerData(int startingFunds, int startingSeeds)
        {
            Funds = startingFunds;
            Seeds = startingSeeds;
            WaterLevel = 1f;
        }

        public void AddFunds(int amount)
        {
            Funds += amount;
        }

        public bool SpendFunds(int amount)
        {

            // if not enough funds to complete purchase return false
            if (Funds < amount)
                return false;

            // if enough funds for purchase subtract amount and return true
            Funds -= amount;
            return true;
        }

        public void AddSeeds(int amount)
        {
            Seeds += amount;
        }

        public void AddWater(float amount)
        {
            WaterLevel = Mathf.Clamp01(WaterLevel + amount);
        }

        public void UseWater(float amount)
        {
            WaterLevel -= amount;
        }
    }
}

