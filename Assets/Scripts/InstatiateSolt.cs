using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstatiateSolt : MonoBehaviour
{
    [SerializeField] private GameObject solt;

    private void FixedUpdate()
    {
        Instantiate(solt);   
    }
}
