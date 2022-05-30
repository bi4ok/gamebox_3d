using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicManager : MonoBehaviour
{
    [SerializeField] private GameObject pudofsoltgmObj;
    [SerializeField] private GameObject invisiblehat;
    [SerializeField] private GameObject gunpowder;
    
    
   
    public void PudOfSalt()
    {
        pudofsoltgmObj.SetActive(true);
    }
   public void GunPowder()
    {
        PlayerPrefs.SetInt("gunpowder", 1);
    }
   
}
