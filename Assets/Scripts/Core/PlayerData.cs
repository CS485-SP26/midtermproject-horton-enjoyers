using UnityEngine;


namespace Core
{
    [System.Serializable]
    public class PlayerData
    {
        
        public int Funds {get; private set;}

        public PlayerData(int startingFunds)
        {
            Funds = startingFunds;
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
    }
}

