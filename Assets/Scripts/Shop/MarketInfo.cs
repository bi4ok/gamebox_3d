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
        "\n" + products.cost_red.ToString() +
        "\n" + products.cost_blue.ToString() +
        "\n" + products.cost_brown.ToString();
        mainimage.sprite = products.artwork;
        buttontext.text = products.isbought == true ? products.ismax == true ? "Max" : "Upgrade" : "Buy";
        currentproduct = products;
        buttonbuy.interactable = true;
    }
    public void Buy()
    {
        
        currentproduct.isbought = true;
        //�������� ������
        buttonbuy.onClick.AddListener(() => Choose());
        UpdateInfo(currentproduct);
        
    }
    public void Choose()
    {
        item = new Item(currentproduct.damagePlus, currentproduct.damagePercent ,
         currentproduct.attackSpeedPlus , currentproduct.attackSpeedPercent ,
         currentproduct.attackRangePlus , currentproduct.attackRangePercent ,
                   currentproduct.healthPlus , currentproduct.healthPercent ,
         currentproduct.movementSpeedPlus , currentproduct.movementSpeedPercent );
       
        if(currentproduct.CurrentLevel != currentproduct.levelsCount)
        {
            //�������� ������
            playercontroller.EquipProduct(item);
            currentproduct.CurrentLevel++;
        }
        else
        {
            buttonbuy.interactable = false;
            currentproduct.ismax = true;
        }
        UpdateInfo(currentproduct);
    }
}
