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
    private GameObject gameOverCanvas;

    [SerializeField]
    private Text timerText;

    [SerializeField]
    private GameObject settingsMenu;

    [SerializeField]
    private GameObject PauseMenu;

    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private BonusController bonusHandler;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private InputField inputField;

    private bool _nameEntered = false;

    private void Awake()
    {
        Time.timeScale = 1; 
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        combatCanvas.SetActive(true);
        buildCanvas.SetActive(false);
    }
    public void FixedUpdate()
    {
        PauseGame();
    }

    public void ChangeToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ChangeToSettings()
    {
        settingsMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void PauseGame()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ContinueGameButton()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        
    }

    public void ChangeStateToFight()
    {
        combatCanvas.SetActive(true);
        buildCanvas.SetActive(false);
    }

    public void ChangeStateToBuild()
    {
        combatCanvas.SetActive(false);
        buildCanvas.SetActive(true);
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        combatCanvas.SetActive(false);
        buildCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        ShowScoreOnEnd();

    }

    private void ShowScoreOnEnd()
    {
        float currentScore = bonusHandler.GetCurrentScore();
        scoreText.text = $"{currentScore}";
        var submitEvent = new InputField.SubmitEvent();
        submitEvent.AddListener(SubmitName);
        inputField.onEndEdit = submitEvent;
    }

    private void SubmitName(string name)
    {
        if (name.Length > 0 && !_nameEntered)
        {
            _nameEntered = true;
            Debug.Log(name);
            bonusHandler.WriteScoreRowInTable(name);
        }

    }
}
