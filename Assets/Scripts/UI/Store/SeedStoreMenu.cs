using UnityEngine;
using TMPro;
using Core;

namespace Shop{
    public class SeedStoreMenu : MonoBehaviour
    {
        public int tomatoPrice {get;} = 30;
        public int cactusPrice {get;} = 50;
        public int cucumberPrice {get;} = 100;

        private int cactusQty;
        private int tomatoQty;
        private int cucumberQty;

        public TMP_Text cactusText;
        public TMP_Text tomatoText;
        public TMP_Text cucumberText;
        public TMP_Text totalText;

        void UpdateUI()
        {
            tomatoText.text = tomatoQty.ToString();
            cactusText.text = cactusQty.ToString();
            cucumberText.text = cucumberQty.ToString();
            totalText.text = "Total: $" + calcBuyTotal().ToString();
        }

        public void AddCactus() { cactusQty++; UpdateUI(); }
        public void RemoveCactus() { cactusQty = Mathf.Max(0, cactusQty - 1); UpdateUI(); }

        public void AddTomato() { tomatoQty++; UpdateUI(); }
        public void RemoveTomato() { tomatoQty = Mathf.Max(0, tomatoQty - 1); UpdateUI(); }

        public void AddCucumber() { cucumberQty++; UpdateUI(); }
        public void RemoveCucumber() { cucumberQty = Mathf.Max(0, cucumberQty - 1); UpdateUI(); }

        public void BuySeeds()
        {
            int total = calcBuyTotal();

            if (GameManager.Instance.playerData.SpendFunds(total))
            {
                GameManager.Instance.playerData.AddSeeds(PlayerData.seedType.tomato, tomatoQty);
                GameManager.Instance.playerData.AddSeeds(PlayerData.seedType.cactus, cactusQty);
                GameManager.Instance.playerData.AddSeeds(PlayerData.seedType.cucumber, cucumberQty);

                cactusQty = tomatoQty = cucumberQty = 0;
                Debug.Log("Purchase Successful");
                UpdateUI();
            }
            Debug.Log("Purchase Failed?");
        }

        private int calcBuyTotal()
        {
            int total =
                cactusQty * cactusPrice +
                tomatoQty * tomatoPrice +
                cucumberQty * cucumberPrice;
            return total;
        }
    }
}
