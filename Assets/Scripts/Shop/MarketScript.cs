using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketScript : MonoBehaviour
{
    [Serializable]
    public class BarChanger
    {
        public GameObject bar;
        public Vector3 shift;
    }

    [SerializeField] 
    private GameObject slot;
    [SerializeField] 
    private GameObject parentslot;
    [SerializeField] 
    private ProductScript[] products;
    [SerializeField] 
    private MarketInfo marketinf;

    [SerializeField]
    private BarChanger[] barsToMove;
    [SerializeField]
    GameHandler gameHandler;
    [SerializeField]
    private GameObject[] slots;
    private int i;
    private void Awake()
    {
        //marketinf = marketinfobj.GetComponent<MarketInfo>();
        Slotssort();
        
        print("MARKET START");
    }
    private void Start()
    {
        CheckPrices();
    }
    public void MoveHPBarsRight()
    {
        print("����� ������");
        foreach (BarChanger bar in barsToMove)
        {
            print($"{bar} -- { bar.bar.transform.position} -- {bar.shift}");
            bar.bar.transform.position += bar.shift;
        }
    }
    public void MoveHPBarsLeft()
    {
        foreach (BarChanger bar in barsToMove)
        {
            bar.bar.transform.position -= bar.shift;
        }
    }

    public void Slotssort()
    {
        foreach (ProductScript productscr in products)
        {
            GameObject newproducts = Instantiate(slot, parentslot.transform);
            Slot slotscript = newproducts.GetComponent<Slot>();
            slotscript.OnCreate(productscr);
            Button buttonSlots = newproducts.GetComponentInChildren<Button>();
            //marketinf.UpdateInfo(productscr);
            buttonSlots.onClick.AddListener(() => marketinf.UpdateInfo(productscr, slotscript));
            slots[i] = newproducts;
            i++;
        }

    }
    public void CheckPrices()
    {
        foreach(GameObject game in slots)
        {
            Slot slotscript = game.GetComponent<Slot>();
            slotscript.CheckPrice(gameHandler.CheckScrap("yellow"), gameHandler.CheckScrap("blue"), gameHandler.CheckScrap("red"));
        }
    }
}
