using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EducationManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Education_panels;
    private int i = 0;
    private void Awake()
    {
        foreach(GameObject g in Education_panels)
        {
            g.SetActive(false);
        }
        
    }
    public void NextEducationPanel()
    {
        Education_panels[i].SetActive(true);
        i++;
    }
}
