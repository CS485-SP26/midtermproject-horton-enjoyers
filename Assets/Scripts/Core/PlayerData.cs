using UnityEngine;


namespace Core
{
    [System.Serializable]
    public class PlayerData
    {

        public int Funds { get; private set; }
        public int Seeds { get; private set; }
        public float WaterLevel { get; private set; }

        public int Plants; // I think this works??

        public float EnergyLevel = 1f; 
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

        public int SellAllPlants(int pricePerPlant)
        {
            int earned = Plants * pricePerPlant;
            Plants = 0;
            AddFunds(earned);
            return earned;
        }



        public bool UseSeed()
        {   
            if (Seeds >=1)
            {
                Seeds -= 1;
                return true;
            }
            else
                return false;
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

