using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Product",menuName ="ProductforMarket/Armor" )]
public class ProductScript : ScriptableObject
{
    public string type = "Armor";
    public  string nameofProduct;
    public int cost;
    public int damagePlus;
    public int damagePercent;
    public int attackSpeedPlus;
    public int attackSpeedPercent;
    public int attackRangePlus;
    public int attackRangePercent;
    public int healthPlus;
    public int healthPercent;
    public int movementSpeedPlus;
    public int movementSpeedPercent;
    public Sprite artwork;
    public bool isbought;
    public bool ischoosen;

}
