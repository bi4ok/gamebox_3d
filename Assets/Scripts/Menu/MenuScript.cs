using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject combatCanvas;

    [SerializeField]
    private GameObject buildCanvas;

    [SerializeField]
    private Text timerText;

    [SerializeField]
    private GameObject settingsMenu;

    [SerializeField]
    private GameObject PauseMenu;

    [SerializeField]
    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        combatCanvas.SetActive(true);
        buildCanvas.SetActive(false);
    }

    public void ChangeToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ChangeToSettings()
    {
        settingsMenu.SetActive(true);
    }

    public void PauseGame()
    {
        PauseMenu.SetActive(true);
    }

    public void ContinueGameButton()
    {
        PauseMenu.SetActive(false);
    }
}
