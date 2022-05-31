using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstatiateSolt : MonoBehaviour
{
    [SerializeField] private GameObject solt;
    [SerializeField] private float seconds;

    private void FixedUpdate()
    {
        SoltInst();
    }
    public void SoltInst()
    {
        GameObject newsolt = Instantiate(solt);
        Solt soltScript = newsolt.GetComponent<Solt>();
        StartCoroutine(soltScript.Lifetime(seconds));
    }
}
