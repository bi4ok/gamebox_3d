using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject inGameCanvas;
    [SerializeField]
    private GameObject gameOverCanvas;
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Image HPBar;
    [SerializeField]
    private Sprite[] hpSprites;
    [SerializeField]
    private Image MPBar;
    [SerializeField]
    private Sprite[] mpSprites;
    [SerializeField]
    private Text red_scrtxt;
    [SerializeField]
    private Text blue_scrtxt;
    [SerializeField]
    private Text brown_scrtxt;

    [SerializeField]
    private GameObject player;

    private Dictionary<string, float> scrapStorage; 

    private PlayerController playerInfo;

    private BonusController _bonusHandler;
    private InputField.SubmitEvent submitEvent;
    private bool _nameEntered=false;

    private void Start()
    {
        _bonusHandler = GetComponent<BonusController>();
        inGameCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);
        playerInfo = player.GetComponent<PlayerController>();

        scrapStorage = new Dictionary<string, float>() { { "red", 0 }, { "blue", 0 }, { "yellow", 0 } };
    }

    private void Update()
    {
        UpdateBars();
    }
    private void UpdateInfo()
    {
        red_scrtxt.text = scrapStorage["red"].ToString();
        blue_scrtxt.text = scrapStorage["blue"].ToString();
        brown_scrtxt.text = scrapStorage["yellow"].ToString();
    }

    private void UpdateBars()
    {
        var hpAndEnergy = playerInfo.ShowCurrentStatus();

        int indexOfHpSprite = GetIndexFromValue(hpAndEnergy[0]);
        HPBar.sprite = hpSprites[indexOfHpSprite];

        int indexOfMpSprite = GetIndexFromValue(hpAndEnergy[1]);
        MPBar.sprite = mpSprites[indexOfMpSprite];

    }

    private int GetIndexFromValue(float value)
    {
        int indexOfSprite = (int)Mathf.Floor(value * 10);
        if (indexOfSprite >= mpSprites.Length)
        {
            indexOfSprite = mpSprites.Length - 1;
        }
        else if (indexOfSprite < 0)
        {
            indexOfSprite = 0;
        }
        return indexOfSprite;
    }

    public void EndGame()
    {
        inGameCanvas.SetActive(false);
        Time.timeScale = 0f;
        gameOverCanvas.SetActive(true);
        ShowScoreOnEnd();

    }

    private void ShowScoreOnEnd()
    {
        float currentScore = _bonusHandler.GetCurrentScore();
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
            _bonusHandler.WriteScoreRowInTable(name);
        }
        
    }

    public void BackToMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        
    }

    public void PlayerGetScrap(string colorName, float scrapValue)
    {
        if (scrapStorage.ContainsKey(colorName))
        {
            scrapStorage[colorName] += scrapValue;
            print(scrapStorage + " обновлён счётчик, добавлено " + scrapValue + " цвета " + colorName);
            UpdateInfo();
        }
        else
        {
            print(colorName + " такого ошмётка нету в перечне");
        }
    }

    public bool PlayerTryWasteScrap(string colorName, float scrapValue)
    {
        if (scrapStorage.ContainsKey(colorName) && scrapStorage[colorName] >= scrapValue)
        {
            scrapStorage[colorName] -= scrapValue;
            UpdateInfo();
            return true;
           
            
        }
        else
        {
            print($"Не хватает ошмётков цвета {colorName}");
            return false;
        }
    }
   
}
