using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField]
    private Text slottext;
    [SerializeField]
    private Button slotbutton;
    [SerializeField]
    Image slot;

    public ProductScript productScript;

    public void OnCreate(ProductScript product)
    {
        productScript = product;
        slottext.text = product.nameofProduct.ToString();
 
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

    public void CheckPrice(float current_yellow, float current_blue, float current_red)
    {
        if(productScript.cost_brown <= current_yellow && productScript.cost_blue <= current_blue && productScript.cost_red <= current_red)
        {
            slot.color = new Color(32, 227, 14, 255);
        }
        else
        {
            slot.color = Color.white;
        }
        if (productScript.isbought)
        {
            slotbutton.interactable = false;
        }
    }
    
}
