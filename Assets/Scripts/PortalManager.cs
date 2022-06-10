using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField]
    private Factory upRightPortal;
    [SerializeField]
    private Factory upLeftPortal;
    [SerializeField]
    private Factory upPortal;
    [SerializeField]
    private Factory rightPortal;
    [SerializeField]
    private Factory leftPortal;
    [SerializeField]
    private Factory downPortal;
    [SerializeField]
    private float[] timeList;

    [SerializeField]
    private GameHandler gameHandler;

    private Dictionary<string, Factory> stringPortals;
    private bool lastState;
    private float nextUpdateTime;
    private int counter = 0;
    private bool waveInProcess;
    private float timeForNextWave = 30f;

    private void Awake()
    {
        lastState = true;
        stringPortals = new Dictionary<string, Factory>
        {
            {"UpRight", upRightPortal},
            {"UpLeft", upLeftPortal},
            {"Up", upPortal},
            {"Right", rightPortal},
            {"Left", leftPortal},
            {"Down", downPortal}
        };

        //RunNextWave();
        
    }

    private void Update()
    {
        if (Time.time > nextUpdateTime)
        {
            waveInProcess = CheckWaveIsEnded();
            gameHandler.WaveStateChange(waveInProcess, timeForNextWave);
            nextUpdateTime = Time.time + 1;
        }

    }

    public bool CheckWaveIsEnded()
    {
        bool waveInProcess = false;
        foreach (var portal in stringPortals.Values)
        {
            if (portal.GetWaveStatus() == true)
            {
                waveInProcess = true;
            }
            //else
            //{
            //    float buffTime = portal.TimeToNextWave();
            //    if (0 < buffTime && buffTime < timeToNextWave)
            //    {
            //        timeToNextWave = buffTime;
            //    }
                
            //}
        }
        return waveInProcess;
    }

    public bool GameFinished()
    {
        bool gameEnd = true;
        foreach (var portal in stringPortals.Values)
        {
            if (portal)
                if(portal.HaveNextWave())
                {
                    gameEnd = false;
                }
        }
        return gameEnd;
    }

    public void RunNextWave()
    {
        foreach (var portal in stringPortals.Values)
        {
            portal.ActivateNextWave(counter);
        }
        counter += 1;
        print($"Волна №{counter} Запущена!");
        waveInProcess = true;
        if(counter < timeList.Length)
          timeForNextWave = timeList[counter];
        


    }
}
