using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StorePanel;
public enum CoinType
{
    GOLD, RUPEE
}
public class CardScript : MonoBehaviour
{
    public Text cardname;
    public Image cardSprite;
    public Text upgradecost;
    public GameObject costSprite;
    public Button upgradebutton;
    int id;
    void Start()
    {
        
    }
    private void OnEnable()
    {
       StorePanel.StorePanelScript.OnStoreDatas += DisplayUI;
    }

   
    public void DisplayUI(Cards data,GameObject obj,StoreData storedata)
    {
        if(obj==this.gameObject)
        {
            id = data.id;
            cardname.text = data.name;
            cardSprite.sprite = SpriteManager.Instance.GettingSprite(data.sprite);
            
            if (data.cointype == CoinType.GOLD)
            {
                costSprite.SetActive(true);
                upgradecost.text = data.cost.ToString();
                if (data.cost >= storedata.coins)
                    upgradebutton.interactable = false;
                else
                    upgradebutton.interactable = true;

            }
            else
            {
                costSprite.SetActive(false);
                upgradecost.text = data.cost+" $";
                if (data.cost >= storedata.money)
                    upgradebutton.interactable = false;
                else
                    upgradebutton.interactable = true;
            }
            
        }
    }
    public void UpgradeButton(GameObject obj)
    {
       StorePanelScript.Instance.CardCheck(obj);
    }
}
