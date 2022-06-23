using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject SettingsCanvas;

    [SerializeField]
    private GameObject AuthorsCanvas;

    [SerializeField]
    private GameObject MainCanvas;
    [SerializeField]
    private GameObject EducationCanvas;

    [SerializeField]
    private Slider masterVolumeSlider;
    


    private void Awake()
    {
        MainCanvas.SetActive(true);
        AuthorsCanvas.SetActive(false);
        SettingsCanvas.SetActive(false);
    }

    public void StartGame()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShowAuthors()
    {
        MainCanvas.SetActive(false);
        AuthorsCanvas.SetActive(true);
        SettingsCanvas.SetActive(false);
    }

    public void ShowSettings()
    {
        MainCanvas.SetActive(false);
        AuthorsCanvas.SetActive(false);
        SettingsCanvas.SetActive(true);
    }
    public void ShowEducation()
    {
        MainCanvas.SetActive(false);
        EducationCanvas.SetActive(true);
    }

    public void BackToMenu()
    {
        MainCanvas.SetActive(true);
        AuthorsCanvas.SetActive(false);
        SettingsCanvas.SetActive(false);
        EducationCanvas.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
