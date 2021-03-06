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
    private Image HPBar;
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
    private float timeForFirstWave=30f;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private CastleController castle;
   
    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    EducationManager educationManager;
    [SerializeField]
    private MedPackCellContoller medPackFactory;

    private Dictionary<string, float> scrapStorage; 

    private PlayerController playerInfo;
    
    
    private int monstersInGame;
    private bool waveInProcess;
    public bool gameStateFight;
    private float timeToNextWave;
    private float timeForStartNextWave;
    private bool gameEnd = false;

    private void Awake()
    {
      
        monstersInGame = 0;
        waveInProcess = false;
        gameStateFight = false;
        playerInfo = player.GetComponent<PlayerController>();
        scrapStorage = new Dictionary<string, float>() { { "red", 5 }, { "blue", 250 }, { "yellow", 10} };
        UpdateInfo();
        menuScript.ChangeStateToBuild();
        timeForStartNextWave = Time.time + timeForFirstWave;
        
    }

    private void Start()
    {
        audioManager.PlayDilogs();
    }

    private void Update()
    {
        UpdateBars();
        CheckGameState();
        if (!gameStateFight)
        {
           // nextWaveText.text = ((int)(timeForStartNextWave - Time.time)).ToString();
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

        red_scrtxt.text = CalculateScrap(scrapStorage["red"]);
        blue_scrtxt.text = CalculateScrap(scrapStorage["blue"]);
        brown_scrtxt.text = CalculateScrap(scrapStorage["yellow"]);
    }

    private string CalculateScrap(float scrapValue)
    {
        if (scrapValue <= 999)
        {
            return scrapValue.ToString();
        }
        else
        {
            string newScrapValue = string.Format("{0:f1}k", scrapValue/1000); ;
            return newScrapValue;
        }
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
                
                audioManager.PlayDilogs();
                audioManager.StopMusic("Battle");
                audioManager.PlayMusic("Phase 1 ambient");
                if (!educationManager.isEndEducation())
                    educationManager.NextEducationPanel();
            }

            if (!gameStateFight)
            {
                if (Input.GetKey(KeyCode.N))
                {
                    print("+++++");
                    portalManager.RunNextWave();
                    gameStateFight = true;
                    menuScript.ChangeStateToFight();
                    medPackFactory.OnWaveMedPackSpawn();
                    audioManager.StopMusic("Phase 1 ambient");
                    audioManager.PlayMusic("Battle");
                   
                }
            }
        }


        
        gameStateText.text = gameStateFight.ToString() + " " + monstersInGame.ToString() + " " + waveInProcess.ToString();

    }

    private void UpdateBars()
    {
        var hpAndEnergy = playerInfo.ShowCurrentStatus();

        // int indexOfHpSprite = GetIndexFromValue(hpAndEnergy[0]);
        HPBar.fillAmount = playerInfo.CurrentHpcheck() / playerInfo.CheckStats("health");
        
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
                print("?????? ????????, ???????!!!");
                scrapValue *= 1.25f;
            }
            scrapStorage[colorName] += (int)scrapValue;
            print(scrapStorage + " ???????? ???????, ????????? " + scrapValue + " ????? " + colorName);
            UpdateInfo();
        }
        else
        {
            print(colorName + " ?????? ??????? ???? ? ???????");
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
            print($"?? ??????? ???????? ????? {colorName}");
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

        UpdateInfo();
        return true;

    }
    public float CheckScrap(string colorName)
    {
      return scrapStorage[colorName];
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
