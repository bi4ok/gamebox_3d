using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketScript : MonoBehaviour
{
    [SerializeField] private GameObject slot;
    [SerializeField] private GameObject parentslot;
    [SerializeField] private ProductScript[] products;
    [SerializeField] private GameObject marketinfobj;
   [SerializeField] private MarketInfo marketinf;
    private void Awake()
    {
      //  marketinf = marketinfobj.GetComponent<MarketInfo>();
        Slotssort();
    }

    public void Slotssort()
    {
        foreach (ProductScript productscr in products)
        {
            GameObject newproducts = Instantiate(slot, parentslot.transform);
            Slot slotscript = newproducts.GetComponent<Slot>();
            slotscript.OnCreate(productscr);
            Button buttonSlots = newproducts.GetComponentInChildren<Button>();
            marketinf.UpdateInfo(productscr);
            buttonSlots.onClick.AddListener(() => marketinf.UpdateInfo(productscr));

        }

    }
}
