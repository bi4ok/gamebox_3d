using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    [SerializeField]
    private MenuScript menuScript;
    [SerializeField]
    private GameObject portalManagerHandler;
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
    private Text gameStateText;

    [SerializeField]
    private Text nextWaveText;

    [SerializeField]
    private GameObject player;

    private Dictionary<string, float> scrapStorage; 

    private PlayerController playerInfo;
    private PortalManager portalManager;
    
    private int monstersInGame;
    private bool waveInProcess;
    private bool gameStateFight;
    private float timeToNextWave;
    private bool gameEnd = false;

    private void Awake()
    {
        monstersInGame = 0;
        waveInProcess = true;
        gameStateFight = true;
        playerInfo = player.GetComponent<PlayerController>();
        portalManager = portalManagerHandler.GetComponent<PortalManager>();
        scrapStorage = new Dictionary<string, float>() { { "red", 10 }, { "blue", 1000 }, { "yellow", 10 } };
        UpdateInfo();
    }

    private void Update()
    {
        UpdateBars();
        CheckGameState();
        if (!gameStateFight)
        {
            nextWaveText.text = ((int)timeToNextWave).ToString();
        }
        if (gameEnd)
        {
            EndGame();
        }
        
    }
    private void UpdateInfo()
    {
        red_scrtxt.text = (scrapStorage["red"] > 999 ? "999" : scrapStorage["red"].ToString());
        blue_scrtxt.text = (scrapStorage["blue"] > 999 ? "999" : scrapStorage["blue"].ToString());
        brown_scrtxt.text = (scrapStorage["yellow"] > 999 ? "999" : scrapStorage["yellow"].ToString());
    }

    private void CheckGameState()
    {

        if (monstersInGame > 0 || waveInProcess)
        {
            if (!gameStateFight)
            {
                gameStateFight = true;
                menuScript.ChangeStateToFight();
            }

        }
        else
        {
            if (gameStateFight)
            {
                gameStateFight = false;
                gameEnd = portalManager.GameFinished();
                menuScript.ChangeStateToBuild();
            }
            
        }
        gameStateText.text = gameStateFight.ToString() + " " + monstersInGame.ToString() + " " + waveInProcess.ToString();

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
        menuScript.EndGame();
    }

    public void BackToMenuButton()
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        
    }

    public void PlayerGetScrap(string colorName, float scrapValue)
    {
        if (scrapStorage.ContainsKey(colorName))
        {
            if (playerInfo.CheckBonusScrap())
            {
                print("������ ��̨����, �������!!!");
                scrapValue *= 1.25f;
            }
            scrapStorage[colorName] += (int)scrapValue;
            print(scrapStorage + " ������� �������, ��������� " + scrapValue + " ����� " + colorName);
            UpdateInfo();
        }
        else
        {
            print(colorName + " ������ ������ ���� � �������");
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
            print($"�� ������� ������� ����� {colorName}");
            return false;
        }
    }
   

    public void CountMonstersInGame(int count)
    {
        monstersInGame += count;
    }

    public void WaveStateChange((bool, float) waveState)
    {
        waveInProcess = waveState.Item1;
        timeToNextWave = waveState.Item2;

    }

    public void MonsterIsDead()
    {
        monstersInGame -= 1;
    }
}
