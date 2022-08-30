using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
namespace CollectionPanel
{
    public class CollectionPanelScript : MonoBehaviour
    {
        public delegate void CollectionDatas(CharCards charCards, GameObject obj);
        public static event CollectionDatas OnCollectionDatasChanged;
        public GameObject cardprefab;
        public GameObject topcardcontent;
        public GameObject unusedcontent;
        public GameObject[] images;
        public CollectionData collectionData = new CollectionData();
        Dictionary<GameObject,int> collectioncardsDic = new Dictionary<GameObject,int>();
        List<GameObject> cardsobject = new List<GameObject>();
        string filepath;
        private static CollectionPanelScript instance;
        public static CollectionPanelScript Instance
        {
            get
            {
                if(instance==null)
                    instance=FindObjectOfType<CollectionPanelScript>(true);
                return instance;
            }
        }

        void Start()
        {
            filepath = "Assets/Resources/CollectionData.json";
            var data = Resources.Load<TextAsset>("CollectionData");
            collectionData = JsonUtility.FromJson<CollectionData>(data.text);
            collectionData.currentusingcards = 0;
            for (int i = 0; i < collectionData.cards.Length; i++)
            {
                if (i < collectionData.totaldeckcards)
                {
                    GameObject temp = Instantiate(cardprefab);
                    temp.transform.parent = topcardcontent.transform;
                    collectionData.cards[i].used = true;
                    temp.name = "Card" + i;
                    collectionData.currentusingcards++;
                    images[i].SetActive(false);
                    cardsobject.Add(temp);
                   
                    collectioncardsDic.Add(temp, collectionData.cards[i].id);
                    OnCollectionDatasChanged(collectionData.cards[i], temp);
                }
                else 
                {
                    GameObject temp = Instantiate(cardprefab);
                    temp.transform.parent = unusedcontent.transform;

                    collectionData.cards[i].used = false;
                    collectioncardsDic.Add(temp, collectionData.cards[i].id);
                    OnCollectionDatasChanged(collectionData.cards[i], temp);
                }
            }
            UpdateData(collectionData);
            ChangingIndex();

        }

        private void ChangingIndex()
        {
            for (int i = 0; i < collectionData.currentusingcards; i++)
            {
                cardsobject[i].transform.SetSiblingIndex(i);
                
            }
            int k = 0;
            for (int i = collectionData.currentusingcards; i < topcardcontent.transform.childCount; i++)
            {
                images[k].transform.SetSiblingIndex(i);
                k++;
            }
        }

        public CharCards CheckCard(GameObject obj)
        {
            for (int i = 0; i < collectionData.cards.Length; i++)
            {
                if (collectionData.cards[i].id == collectioncardsDic[obj])
                {
                    return collectionData.cards[i];
                }
            }
            return null;

        }
        public GameObject GetObjectByID(int id)
        {
            foreach (var item in collectioncardsDic)
            {
                if (item.Value == id)
                    return item.Key;
            }
            return null;
        }
        public CollectionData GetData()
        {
            return collectionData;
        }
        public void UpdateData(CollectionData data)
        {
            collectionData = data;
            File.WriteAllText(filepath, JsonUtility.ToJson(collectionData, true));
            for (int i = 0; i < collectionData.cards.Length; i++)
            {
                GameObject temp = GetObjectByID(collectionData.cards[i].id);
                OnCollectionDatasChanged(collectionData.cards[i], temp);
            }
        }
        public void ChangeParent(GameObject obj)
        {
            CollectionData data = GetData();
            CharCards cards=CheckCard(obj);
           
            if(cards.used==true)
            {
                cards.used = false;
                obj.transform.parent = unusedcontent.transform;
                data.currentusingcards--;
            }
            else if(data.currentusingcards<data.totaldeckcards && cards.used==false)
            {
                cards.used = true;
                obj.transform.parent = topcardcontent.transform;
                data.currentusingcards++;
            }
            UpdateData(data);
            Debug.Log(data.currentusingcards);
            ChangingIndex();
        }

    }
    [System.Serializable]
    public class CollectionData
    {
        public int totaldeckcards;
        public int currentusingcards;
        public CharCards[] cards;
    }
    [System.Serializable]
    public class CharCards
    {
        public int id;
        public string sprite;
        public int upgrades;
        public int leveltext;
        public float currentupgradelevel;
        public float totalupgradelevel;
        public int accuracy;
        public int hitpoints;
        public float damage;
        public bool used;
        public int upgradecost;
        public int upgradecostinc;

    }
}