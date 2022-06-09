using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Product",menuName ="Product/forMarket" )]
public class ProductScript : ScriptableObject
{
    public string type;
    public int level;
    public string nameofProduct;
    public string productInfo;
    public int cost_red;
    public int cost_brown;
    public int cost_blue;
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
    public bool isbought;
    public ProductScript check;

}
