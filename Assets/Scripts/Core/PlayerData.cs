using System;
using UnityEngine;
using Shop;


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

        public seedType equippedSeed = seedType.tomato;
        public float WaterLevel { get; private set; }

        public int tomatoPlants = 0;
        public int cactusPlants = 0;
        public int cucumberPlants = 0;

        public float EnergyLevel = 1f; 
        public PlayerData(int startingFunds, int startingSeeds)
        {
            Funds = startingFunds;
            tomatoSeeds = startingSeeds;
            cactusSeeds = startingSeeds;
            cucumberSeeds = startingSeeds;
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

        public void SellAllPlants(int earned)
        {
            tomatoPlants = 0;
            cactusPlants = 0;
            cucumberPlants = 0;
            AddFunds(earned);
        }

        public int getEquippedSeedCount(seedType type)
        {
            switch(type)
            {
                case seedType.tomato: return tomatoSeeds;
                case seedType.cactus: return cactusSeeds;
                case seedType.cucumber: return cucumberSeeds;
            }
            return 0;
        }

        public void CycleSeed()
        {
            seedType type = equippedSeed;

            switch(type)
            {
                case seedType.tomato: equippedSeed = seedType.cactus; break;
                case seedType.cactus: equippedSeed = seedType.cucumber; break;
                case seedType.cucumber: equippedSeed = seedType.tomato; break;
            }
            Debug.Log("Seed changed to " + equippedSeed);
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

