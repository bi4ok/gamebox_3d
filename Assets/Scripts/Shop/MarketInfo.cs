using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketInfo : MonoBehaviour
{
    [SerializeField]  private Text buttontext;
    [SerializeField] private Text nametext;
    [SerializeField] private Text chtext;
    [SerializeField] private Image mainimage;
    [SerializeField] private Button buttonbuy;
    private ProductScript currentproduct;
    [SerializeField] private PlayerController playercontroller;
    private Item item;
    public void UpdateInfo(ProductScript products)
    {
        nametext.text = products.nameofProduct;
        chtext.text = products.healthPlus.ToString() + "," + "+"+ products.healthPercent.ToString() + "%" +
        "\n" + products.damagePlus.ToString()        + "," + "+" + products.damagePercent.ToString() + "%" +
        "\n" + products.attackSpeedPlus.ToString()   + "," + "+" + products.attackSpeedPercent.ToString() + "%" +
        "\n" + products.movementSpeedPlus.ToString() + "," + "+" + products.movementSpeedPercent.ToString() + "%" +
        "\n" + products.cost.ToString();
        mainimage.sprite = products.artwork;
        buttontext.text = products.isbought == true ? products.ischoosen == true ? "Choosen" : "Choose" : "Buy";
        currentproduct = products;
       
    }
    public void Buy()
    {
        
        currentproduct.isbought = true;
        //Вычитаем монеты
        buttonbuy.onClick.AddListener(() => Choose());
        UpdateInfo(currentproduct);
    }
    public void Choose()
    {
        item = new Item(currentproduct.damagePlus, currentproduct.damagePercent = 0,
         currentproduct.attackSpeedPlus = 0, currentproduct.attackSpeedPercent = 0,
         currentproduct.attackRangePlus = 0, currentproduct.attackRangePercent = 0,
                   currentproduct.healthPlus = 0, currentproduct.healthPercent = 0,
         currentproduct.movementSpeedPlus = 0, currentproduct.movementSpeedPercent = 0);
       
        if(currentproduct.ischoosen == true)
        {
            playercontroller.UnequipProduct(item);
            currentproduct.ischoosen = false;
        }
        else
        {
            playercontroller.EquipProduct(item);
            currentproduct.ischoosen = true;
        }
        UpdateInfo(currentproduct);
    }
}
