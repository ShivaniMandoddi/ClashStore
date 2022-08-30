using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CollectionPanel;
public class UIButtonsScript : MonoBehaviour
{
    public GameObject infopanel;
    public GameObject storepanel;
    public GameObject collectionpanel;
    public GameObject[] collectiondisplaypanels;
    public Image[] buttons;
    public Sprite image1;
    public Sprite image2;
    private void Start()
    {
        SetAllPanelFalse();
        collectionpanel.SetActive(true);
    }
    public void StorePanel()
    {
        SetAllPanelFalse();
        storepanel.SetActive(true);
    }
    public void CollectionPanel()
    {
        SetAllPanelFalse();
        collectionpanel.SetActive(true);
    }
    public void MenuPanel(GameObject obj)
    {
        obj.SetActive(true);
    }
    public void MenuPanelClose(GameObject obj)
    {
        obj.SetActive(false);
    }
    public void RemoveButton()
    {
        CollectionData data = CollectionPanelScript.Instance.GetData();
        for (int i = 0; i < data.cards.Length; i++)
        {
            if (data.cards[i].used==true)
            {
                CollectionPanelScript.Instance.ChangeParent(CollectionPanelScript.Instance.GetObjectByID(data.cards[i].id));
            }
        }
    }
    public void MagicButton()
    {
        CollectionData data = CollectionPanelScript.Instance.GetData();
        int k = data.totaldeckcards - data.currentusingcards;
        int j = 0;
        for (int i = 0; i < data.cards.Length; i++)
        {
            if(j<k && data.cards[i].used==false)
            {
                CollectionPanelScript.Instance.ChangeParent(CollectionPanelScript.Instance.GetObjectByID(data.cards[i].id));
                k++;
            }

        }
    }
    private void SetAllPanelFalse()
    {
       storepanel.SetActive(false);
        collectionpanel.SetActive(false);

    }

    public void InfoPanel()
    {
        infopanel.SetActive(true);
    }
    public void CloseInfo()
    {
        //Debug.Log("CLose");
        infopanel.SetActive(false);
    }
    public void Display1(int i)
    {
        SettingButtonsFalse();
       collectiondisplaypanels[i].SetActive(true);
        buttons[i].sprite = image2;
       
    }
    public void SettingButtonsFalse()
    {
        for (int i = 0; i <collectiondisplaypanels.Length; i++)
        {
            collectiondisplaypanels[i].SetActive(false);
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].sprite = image1;
        }
    }
    
}
