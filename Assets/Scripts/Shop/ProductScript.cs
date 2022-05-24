using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Product",menuName ="ProductforMarket" )]
public class ProductScript : ScriptableObject
{
    public  string nameofProduct;
    public int cost;
    public int attack_plus;
    public int attack_percent;
    public int health_plus;
    public int health_percent;
    public int attackspeed_plus;
    public int attackspeed_percent;
    public int movespeed_plus;
    public int movespeed_percent;
    public Sprite artwork;
    public bool isbought;
    public bool ischoosen;

}
