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
        nametext.text = products.name;
        chtext.text = products.health_plus.ToString() + "\n" + products.attack_plus.ToString() + "\n" + products.attackspeed_plus.ToString() + "\n" + products.movespeed_plus.ToString() + "\n" + products.cost.ToString();
        mainimage.sprite = products.artwork;
        buttontext.text = products.isbought == true ? products.ischoosen == true ? "Choosen" : "Choose" : "Buy";
        currentproduct = products;
       
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
        
        if(currentproduct.ischoosen == true)
        {
            //�������� ����� ��� �������� �������
            currentproduct.ischoosen = false;
        }
        else
        {
            //���������� �����
            currentproduct.ischoosen = true;
        }
        UpdateInfo(currentproduct);
    }
}
