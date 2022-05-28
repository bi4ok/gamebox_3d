using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Product",menuName ="ProductforMarket" )]
public class ProductScript : ScriptableObject
{
    public string type;
    public  string nameofProduct;
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
    public Sprite artwork;
    public bool isbought;
    public bool ismax;
    public int levelsCount;
    public int CurrentLevel;

}