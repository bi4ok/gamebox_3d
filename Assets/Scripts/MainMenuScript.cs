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
    private Slider masterVolumeSlider_music;
    [SerializeField]
    private Slider masterVolumeSlider_sounds;
    [SerializeField]
    private Slider masterVolumeSlider_dilogs;


    private void Awake()
    {
        MainCanvas.SetActive(true);
        AuthorsCanvas.SetActive(false);
        SettingsCanvas.SetActive(false);
        masterVolumeSlider_dilogs.value = PlayerPrefs.GetFloat("MasterVolume_Dilogs", 1);
        masterVolumeSlider_sounds.value = PlayerPrefs.GetFloat("MasterVolume_Sounds", 1);
        masterVolumeSlider_music.value = PlayerPrefs.GetFloat("MasterVolume_Music", 1);
    }

    public void StartGame()
    {
        PlayerPrefs.SetFloat("MasterVolume_Music", masterVolumeSlider_music.value);
        PlayerPrefs.SetFloat("MasterVolume_Sounds", masterVolumeSlider_sounds.value);
        PlayerPrefs.SetFloat("MasterVolume_Dilogs", masterVolumeSlider_dilogs.value);
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
