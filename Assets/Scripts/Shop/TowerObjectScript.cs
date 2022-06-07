using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Product", menuName = "ProductForTower")]
public class TowerObjectScript : ScriptableObject
{
    public string nameOfTower;
    public string towerInfo;
    public GameObject prefabInGame;
    public GameObject prefabInUI;
    public Sprite imgOfTower;
    public int costRed;
    public int costBrown;
    public int costBlue;
    public TowerObjectScript check;
    public bool isBougth;

}
