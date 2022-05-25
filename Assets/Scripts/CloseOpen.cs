using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseOpen : MonoBehaviour
{
    public void Close(GameObject closegameObject)
    {
        if(closegameObject != null)  closegameObject.SetActive(false);
    }
    public void Open(GameObject opengameObject)
    {
        if (opengameObject != null) opengameObject.SetActive(true);
    }
}
