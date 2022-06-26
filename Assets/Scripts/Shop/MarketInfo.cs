using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketInfo : MonoBehaviour
{
    [SerializeField] 
    private Text buttontext;
    [SerializeField] 
    private Text nametext;
    [SerializeField] 
    private Text chtext;
    [SerializeField] 
    private Image mainimage;
    [SerializeField]
    private Text blue_pricetext;
    [SerializeField]
    private Text green_pricetext;
    [SerializeField]
    private Text red_pricetext;
    [SerializeField] 
    private Button buttonbuy;
    [SerializeField]
    private PlayerController playercontroller;
    [SerializeField]
    private GameHandler gameHandler;
    [SerializeField]
    AudioManager audioManager;

    private ProductScript currentproduct;
    private Slot currentSlot;

    private Item item;
    public void UpdateInfo(ProductScript products, Slot slotScript)
    {
        print(this.name);
        nametext.text = products.nameofProduct;
        //chtext.text = products.healthPlus.ToString() + "," + "+"+ products.healthPercent.ToString() + "%" +
        //"\n" + products.damagePlus.ToString()        + "," + "+" + products.damagePercent.ToString() + "%" +
        //"\n" + products.attackSpeedPlus.ToString()   + "," + "+" + products.attackSpeedPercent.ToString() + "%" +
        //"\n" + products.movementSpeedPlus.ToString() + "," + "+" + products.movementSpeedPercent.ToString() + "%" +
        //"\n" + products.cost_red.ToString() +
        //"\n" + products.cost_blue.ToString() +
        //"\n" + products.cost_brown.ToString();
        chtext.text = GetProductInfo(products);

       // mainimage.sprite = products.artwork;
        currentproduct = products;
        currentSlot = slotScript;
        buttonbuy.interactable = currentSlot.CanIBuyIt();
        print(currentproduct + "UPDATE");
    }

    private string GetProductInfo(ProductScript products)
    {
        string productInfo = "Описание товара:" +
            "\n" + products.productInfo;
            //"\n" + "Характеристики:" +
            //"\n\n" + "Здоровье:" +
            //"+" + products.healthPlus.ToString() + "," + "+" + products.healthPercent.ToString() + "%" +
            //"\n\n" + "Урон:" +
            //"+" + products.damagePlus.ToString() + "," + "+" + products.damagePercent.ToString() + "%" +
            //"\n\n" + "Скорость бега:" +
            //"+" + products.movementSpeedPlus.ToString() + "," + "+" + products.movementSpeedPercent.ToString() + "%" 
        ;
        blue_pricetext.text = products.cost_blue.ToString();
        green_pricetext.text = products.cost_brown.ToString();
        red_pricetext.text = products.cost_red.ToString();
        
        return productInfo;
    }

    public void Buy()
    {
        audioManager.PlaySounds("Kassa");
        item = new Item(damagePlus: currentproduct.damagePlus,
            damagePercent: currentproduct.damagePercent,
            attackSpeedPlus: currentproduct.attackSpeedPlus,
            attackSpeedPercent: currentproduct.attackSpeedPercent,
            attackRangePlus: currentproduct.attackRangePlus,
            attackRangePercent: currentproduct.attackRangePercent,
            healthPlus: currentproduct.healthPlus,
            healthPercent: currentproduct.healthPercent,
            energyPlus: 0, energyPercent: 0,
            movementSpeedPlus: currentproduct.movementSpeedPlus,
            movementSpeedPercent: currentproduct.movementSpeedPercent);

        var cost = new Dictionary<string, float>()
        {
            {"red", currentproduct.cost_red },
            {"blue", currentproduct.cost_blue },
            {"yellow", currentproduct.cost_brown }
        };

        if (gameHandler.PlayerTryWasteScrap(cost))
        {
            //Вычитаем монеты
            playercontroller.EquipProduct(item, currentproduct.type, currentproduct.level);
            currentproduct.isbought = true;
        }
        UpdateInfo(currentproduct, currentSlot);
        
    }
    public void Choose()
    {

    }
}

