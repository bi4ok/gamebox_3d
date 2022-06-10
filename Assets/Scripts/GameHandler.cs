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
    private PortalManager portalManager;
    [SerializeField]
    private Text HPBar;
    [SerializeField]
    private Sprite[] hpSprites;
    [SerializeField]
    private Text MPBar;
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
    [SerializeField]
    private CastleController castle;
    [SerializeField]
    private Soundmessegemanager soundmessr;

    [SerializeField]
    private MedPackCellContoller medPackFactory;

    private Dictionary<string, float> scrapStorage; 

    private PlayerController playerInfo;
    
    
    private int monstersInGame;
    private bool waveInProcess;
    private bool gameStateFight;
    private float timeToNextWave;
    private float timeForStartNextWave;
    private bool gameEnd = false;

    private void Awake()
    {
      
        monstersInGame = 0;
        waveInProcess = false;
        gameStateFight = false;
        playerInfo = player.GetComponent<PlayerController>();
        scrapStorage = new Dictionary<string, float>() { { "red", 5 }, { "blue", 250 }, { "yellow", 5} };
        UpdateInfo();
        menuScript.ChangeStateToBuild();
        timeForStartNextWave = Time.time + 30;
        soundmessr.PlayMessege();
    }

    private void Update()
    {
        UpdateBars();
        CheckGameState();
        if (!gameStateFight)
        {
            nextWaveText.text = ((int)(timeForStartNextWave - Time.time)).ToString();
        }
        if (gameEnd)
        {
            WinGame();
        }
        else if (castle.DiedByDamage())
        {
            EndGame();
        }
        
    }
    private void UpdateInfo()
    {
        red_scrtxt.text = (scrapStorage["red"] > 99 ? "99" : scrapStorage["red"].ToString());
        blue_scrtxt.text = (scrapStorage["blue"] > 99 ? "99" : scrapStorage["blue"].ToString());
        brown_scrtxt.text = (scrapStorage["yellow"] > 99 ? "99" : scrapStorage["yellow"].ToString());
    }

    private void CheckGameState()
    {

        if (monstersInGame > 0 || waveInProcess)
        {
            //if (!gameStateFight)
            //{
            //    gameStateFight = true;
            //    menuScript.ChangeStateToFight();
            //    print("MED PACKS SUMMON!! ");
            //    medPackFactory.OnWaveMedPackSpawn();
            //}

        }
        else
        {

            if (gameStateFight)
            {
                timeForStartNextWave = Time.time + timeToNextWave;
                print(timeToNextWave);
                gameStateFight = false;
                gameEnd = portalManager.GameFinished();
                menuScript.ChangeStateToBuild();
                soundmessr.PlayMessege();
            }

            if (!gameStateFight)
            {
                if (Time.time > timeForStartNextWave)
                {
                    print("+++++");
                    portalManager.RunNextWave();
                    gameStateFight = true;
                    menuScript.ChangeStateToFight();
                    medPackFactory.OnWaveMedPackSpawn();
                }
            }
        }


        
        gameStateText.text = gameStateFight.ToString() + " " + monstersInGame.ToString() + " " + waveInProcess.ToString();

    }

    private void UpdateBars()
    {
        var hpAndEnergy = playerInfo.ShowCurrentStatus();

        // int indexOfHpSprite = GetIndexFromValue(hpAndEnergy[0]);
        HPBar.text = playerInfo.CurrentHpcheck().ToString();
        
       // int indexOfMpSprite = GetIndexFromValue(hpAndEnergy[1]);
      //  MPBar.sprite = mpSprites[indexOfMpSprite];

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

    public void WinGame()
    {
        menuScript.WinGame();
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
                print("БОЛЬШЕ ОШМЁТКОВ, ОФИГЕТЬ!!!");
                scrapValue *= 1.25f;
            }
            scrapStorage[colorName] += (int)scrapValue;
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

    public bool PlayerTryWasteScrap(Dictionary<string, float> cost)
    {
        foreach (KeyValuePair<string, float> scrap in cost)
        {
            if (scrapStorage.ContainsKey(scrap.Key) && scrapStorage[scrap.Key] < scrap.Value)
                return false;
        }


        foreach (KeyValuePair<string, float> scrap in cost)
        {
            if (scrapStorage.ContainsKey(scrap.Key))
            {
                scrapStorage[scrap.Key] -= scrap.Value;
            }
        }
        return true;

    }


    public void CountMonstersInGame(int count)
    {
        monstersInGame += count;
    }

    public void WaveStateChange(bool waveState, float nextWaveTime)
    {
        waveInProcess = waveState;
        timeToNextWave = nextWaveTime;

    }

    public void MonsterIsDead()
    {
        monstersInGame -= 1;
    }
}
