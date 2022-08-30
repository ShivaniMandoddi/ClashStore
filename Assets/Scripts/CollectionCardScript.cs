using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CollectionPanel;
using UnityEngine.EventSystems;

public class CollectionCardScript : MonoBehaviour
{
    public delegate void InfoButtons(GameObject obj);
    public static event InfoButtons OnInfoButtons;
    public Image cardSprite;
    public Text upgradetext;
    public Text leveltext;
    public Slider levelSlider;
    public Text levelslidertext;
    public GameObject Buttons;
    CharCards swappingcard;
    GameObject swappinggameobject;
    Vector3 position;
    public Text removetext;
    
    private void OnEnable()
    {
        OnInfoButtons += InfoSetFalse;
        CollectionPanelScript.OnCollectionDatasChanged += DisplayUI;
    }
    private void OnDisable()
    {
        OnInfoButtons -= InfoSetFalse;
        CollectionPanelScript.OnCollectionDatasChanged += DisplayUI;
    }
    public void DisplayUI(CharCards carddata,GameObject obj)
    {
        if(obj==this.gameObject)
        {
            // Debug.Log(SpriteManager.Instance.GettingSprite(carddata.sprite));
            cardSprite.sprite = SpriteManager.Instance.GettingSprite(carddata.sprite);
        
            upgradetext.text = carddata.upgrades.ToString();
            leveltext.text = "Level " + carddata.leveltext;
            
            levelSlider.value = carddata.currentupgradelevel / carddata.totalupgradelevel;
            levelslidertext.text = carddata.currentupgradelevel + "/" + carddata.totalupgradelevel;
            if(carddata.used==false)
            {
                removetext.text = "Use";
            }
            else
            {
                removetext.text = "Remove";
            }
        }
    }
   
    public void InfoPanel(GameObject obj)
    {

        Buttons.SetActive(false);
        
        InfoPanelScript.Instance.InfoPanel(obj);
    }
    public void ClickonObject(GameObject obj)
    {
        OnInfoButtons(obj);
       

    }
    public void OnPointingDown(GameObject obj)
    {
        position = obj.transform.position;
    }
    public void OnPointingDrag(GameObject obj)
    {
        Vector3 pos = Input.mousePosition;
        obj.transform.position = pos;
        
    }
    public void OnPointExit(GameObject obj)
    {
        var hit = new List<RaycastResult>();
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        EventSystem.current.RaycastAll(eventData, hit);
        swappingcard = null;
        for (int i = 0; i < hit.Count; i++)
        {
            if (hit[i].gameObject.layer == 6 && hit[i].gameObject != obj)
            {
                swappingcard = CollectionPanelScript.Instance.CheckCard(hit[i].gameObject);
                
                swappinggameobject = hit[i].gameObject;
                break;
              
            }
        }
        CharCards presentcard=CollectionPanelScript.Instance.CheckCard(obj);
        CollectionData collectionData = CollectionPanelScript.Instance.GetData();
        int k=-1;
        for (int i = 0; i < collectionData.cards.Length; i++)
        {
            if(presentcard==collectionData.cards[i])
            {
                k = i;
            }
        }
        if (swappingcard!=null && k!=-1 && swappingcard.used==true && presentcard.used==true)
        {
            for (int i = 0; i < collectionData.cards.Length; i++)
            {
                if (swappingcard == collectionData.cards[i])
                {
                    obj.transform.SetSiblingIndex(i);
                    swappinggameobject.transform.SetSiblingIndex(k);
                    (collectionData.cards[i], collectionData.cards[k]) = (collectionData.cards[k], collectionData.cards[i]);
                    break;
                }
            }
        }
        else
        {
            obj.transform.position = position;
        }
        CollectionPanelScript.Instance.UpdateData(collectionData);
    }
    public void Remove(GameObject obj)
    {
        Buttons.SetActive(false);
        CollectionPanelScript.Instance.ChangeParent(obj);
    }
    public void InfoSetFalse(GameObject obj)
    {
        if (Buttons == obj)
        {
            if (Buttons.activeInHierarchy == false)
                Buttons.SetActive(true);
            else
                Buttons.SetActive(false);
            
        }
        else
            Buttons.SetActive(false);
    }

}
