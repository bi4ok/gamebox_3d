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
   
    public bool GunPowder()
    {

        return true;
    }
}
