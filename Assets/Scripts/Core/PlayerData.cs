using System;
using UnityEngine;


namespace Core
{
    [System.Serializable]
    public class PlayerData
    {

        public int Funds { get; private set; }
        public int tomatoSeeds { get; private set; }
        public int cactusSeeds { get; private set; }
        public int cucumberSeeds { get; private set; }
        public enum seedType {tomato, cactus, cucumber}
        public float WaterLevel { get; private set; }

        public int Plants; // I think this works??

        public float EnergyLevel = 1f; 
        public PlayerData(int startingFunds, int startingSeeds)
        {
            Funds = startingFunds;
            tomatoSeeds = startingSeeds;
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

        public void AddSeeds(Enum seed, int amount)
        {
            switch(seed)
            {
                case seedType.tomato: tomatoSeeds += amount; break;
                case seedType.cactus: cactusSeeds += amount; break;
                case seedType.cucumber: cucumberSeeds += amount; break;
            }

        }

        public int SellAllPlants(int pricePerPlant)
        {
            int earned = Plants * pricePerPlant;
            Plants = 0;
            AddFunds(earned);
            return earned;
        }



        public bool UseSeed(Enum seed)
        {   
            switch(seed)
            {
                case seedType.tomato: 
                    if (tomatoSeeds >=1)
                    {
                        tomatoSeeds -= 1;
                        return true;
                    }
                    else
                        return false;
                case seedType.cactus: 
                    if (cactusSeeds >=1)
                    {
                        cactusSeeds -= 1;
                        return true;
                    }
                    else
                        return false;
                case seedType.cucumber: 
                    if (cucumberSeeds >=1)
                    {
                        cucumberSeeds -= 1;
                        return true;
                    }
                    else
                        return false;
            }
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

