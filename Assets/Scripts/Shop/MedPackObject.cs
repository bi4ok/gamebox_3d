using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Product", menuName = "ProductForMedPacks")]
public class MedPackObject : ScriptableObject
{
    public string nameOfMedPack;
    public string medpackInfo;
    public int countOfMedpacks;
    public GameObject prefabInGame;
    public GameObject prefabInUI;
    public Sprite imgOfMedPack;
    public int costRed;
    public int costBrown;
    public int costBlue;
    public MedPackObject check;
    public bool isBougth;
}
