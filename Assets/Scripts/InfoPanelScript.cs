using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CollectionPanel;
using StorePanel;
public class InfoPanelScript : MonoBehaviour
{
    public Image cardSprite;
    public Text accuracy;
    public Text hitpoints;
    public Text damage;
    public Button Upgrade;
    public Text upgradetext;
    int id;
   
    private static InfoPanelScript instance;
    public static InfoPanelScript Instance
    {
        get
        {
            if(instance == null)
                instance=FindObjectOfType<InfoPanelScript>(true);
            return instance;
        }
    }
    void Start()
    {
        
    }
    
    public void InfoPanel(GameObject obj)
    {
        
        this.gameObject.SetActive(true);
        CharCards data = CollectionPanelScript.Instance.CheckCard(obj);
        cardSprite.sprite = SpriteManager.Instance.GettingSprite(data.sprite);
        id = data.id;
        accuracy.text = "Accuracy : "+data.accuracy;
        hitpoints.text = "HitPoints : " + data.hitpoints;
        damage.text = "Damage : " + data.damage;
        upgradetext.text = "Upgrade  " + data.upgradecost;
        StoreData storeData = StorePanelScript.Instance.GetData();
        if(data.upgradecost>storeData.coins)
        {
            Upgrade.interactable = false;
        }
        else
        {
            Upgrade.interactable = true;
        }
       
    }
    public void UpgradeData()
    {
        CollectionData data = CollectionPanelScript.Instance.GetData();
        StoreData storeData = StorePanelScript.Instance.GetData();
        for (int i = 0; i < data.cards.Length; i++)
        {
            if (data.cards[i].id==id)
            {
                storeData.coins -= data.cards[i].upgradecost;
                data.cards[i].accuracy += 2;
                data.cards[i].hitpoints += 3;
                data.cards[i].damage += 5;
                data.cards[i].upgradecost += data.cards[i].upgradecostinc;
                data.cards[i].currentupgradelevel += 1;
                GameObject temp=CollectionPanelScript.Instance.GetObjectByID(data.cards[i].id);
                if (temp != null)
                {
                    InfoPanel(temp);
                }
                break;
            }
        }
        CollectionPanelScript.Instance.UpdateData(data);
        StorePanelScript.Instance.UpdateData(storeData);
    }
}
