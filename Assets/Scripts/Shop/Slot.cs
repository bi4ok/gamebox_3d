using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] 
    private Image slotimage;

    public ProductScript productScript;

    public void OnCreate(ProductScript product)
    {
        productScript = product;
        slotimage.sprite = product.artwork;
 
    }

    public bool CanIBuyIt()
    {
        bool slotActive;
        if (productScript.isbought)
        {
            return false;
        }

        if (productScript.check != null)
        {
            slotActive = productScript.check.isbought;
        }
        else
        { 
            slotActive = true;
        }
        return slotActive;
    }

}
