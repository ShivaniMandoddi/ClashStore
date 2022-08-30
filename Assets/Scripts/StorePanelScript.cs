using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
namespace StorePanel
{
    public class StorePanelScript : MonoBehaviour
    {
        public delegate void StoreDatas(Cards carddata, GameObject obj,StoreData storeData);
        public static event StoreDatas OnStoreDatas;
        public GameObject sectionprefab;
        public GameObject cardprefab;
        public GameObject sectioncontent;
        public Text cointext;
        public Text moneyText;
        public StoreData storedata = new StoreData();
       
        string filepath;
        Dictionary<GameObject,int> cardobjects = new Dictionary<GameObject,int>();
        private static StorePanelScript instance;
        public static StorePanelScript Instance
        {
            get
            {
                if(instance==null)
                    instance=FindObjectOfType<StorePanelScript>(true);
                return instance;
            }
        }
        void Awake()
        {
            filepath = "Assets/Resources/StoreData.json";
            var data = Resources.Load<TextAsset>("StoreData");
            storedata = JsonUtility.FromJson<StoreData>(data.text);
            cointext.text = storedata.coins + " ";
            moneyText.text = storedata.money + " ";
            for (int i = 0; i < storedata.sections.Length; i++)
            {
                GameObject temp = Instantiate(sectionprefab);
                temp.transform.parent = sectioncontent.transform;
                for (int j = 0; j < storedata.sections[i].cards.Length; j++)
                {
                    GameObject cardtemp = Instantiate(cardprefab);
                    cardtemp.transform.parent = temp.transform.GetChild(1).transform;
                    cardobjects.Add(cardtemp,storedata.sections[i].cards[j].id);
                    OnStoreDatas(storedata.sections[i].cards[j], cardtemp,storedata);
                }
            }
        }
        public void CardCheck(GameObject obj)
        {
            storedata = GetData();
            if (cardobjects.ContainsKey(obj))
            {
                for (int i = 0; i < storedata.sections.Length; i++)
                {
                    for (int j = 0; j < storedata.sections[i].cards.Length; j++)
                    {
                        if (storedata.sections[i].cards[j].id == cardobjects[obj])
                        {
                            if (storedata.sections[i].cards[j].cointype == CoinType.GOLD)
                                storedata.coins = storedata.coins - storedata.sections[i].cards[j].cost;
                            else
                                storedata.money = storedata.money - storedata.sections[i].cards[j].cost;
                        }
                    }
                }
            }
            UpdateData(storedata);
        }
        public void UpdateData()
        {
            File.WriteAllText(filepath, JsonUtility.ToJson(storedata,true));
            cointext.text = storedata.coins + " ";
            moneyText.text = storedata.money + " ";
            for (int i = 0; i < storedata.sections.Length; i++)
            {
                for (int j = 0; j < storedata.sections[i].cards.Length; j++)
                {
                    GameObject temp=null;
                    foreach (var item in cardobjects)
                    {
                        if(item.Value== storedata.sections[i].cards[j].id)
                        {
                            temp = item.Key;
                        }
                    }

                    OnStoreDatas(storedata.sections[i].cards[j], temp, storedata);
                }
                   
             }
        }
        public void UpdateData(StoreData storeData)
        {
            storedata = storeData;
            UpdateData();
        }
        public StoreData GetData()
        {
            return storedata;
        }
     }

            
      
    

        
    
    [System.Serializable]
    public class StoreData
    {
        public int coins;
        public int money;
        public Sections[] sections;
    }
    [System.Serializable]
    public class Sections
    {
        public string sectionname;
        public string sectiondesc;
        public Cards[] cards;
    }
    [System.Serializable]
    public class Cards
    {
        public int id;
        public string name;
        public int cost;
        public string sprite;
        public int availabletouse;
        public int updatevalue;
        public CoinType cointype;

    }
}


