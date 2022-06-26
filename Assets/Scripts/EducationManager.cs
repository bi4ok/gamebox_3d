using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EducationManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Education_panels;
    private int i = 0;
    private bool is_skip = false;
    private void Awake()
    {
        CloseEducation();
        NextEducationPanel();
    }
    public void NextEducationPanel()
    {
        print(is_skip);
        if (!is_skip)
        {
            CloseEducation();
            Education_panels[i].SetActive(true);
            i++;
            
            print("ѕытаюсь включить обучение ");
        }
    }

    public void NextEducationPanelFromShop()
    {
        print(i);
        if (!is_skip && i == 10)
        {
            CloseEducation();
            Education_panels[i].SetActive(true);
            i++;

            print("ѕытаюсь включить обучение ");
        }
    }

    public void SkipEducation()
    {
        is_skip = true;
    }
    public void CloseEducation()
    {
        if(i != 0)
        Education_panels[i - 1].SetActive(false);
    }
    public bool isEndEducation()
    {
        if (i == Education_panels.Length)
            return true;
        else
            return false;
    }
}
