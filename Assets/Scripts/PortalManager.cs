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
    private GameHandler gameHandler;

    private Dictionary<string, Factory> stringPortals;
    private bool lastState;
    private float nextUpdateTime;

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
        
    }

    private void Update()
    {
        if (Time.time > nextUpdateTime)
        {
            GameFinished();
            gameHandler.WaveStateChange(CheckWaveIsEnded());
            nextUpdateTime = Time.time + 1;
        }

    }

    public (bool, float) CheckWaveIsEnded()
    {
        bool waveInProcess = false;
        float timeToNextWave = Mathf.Infinity;
        foreach (var portal in stringPortals.Values)
        {
            if (portal.GetWaveStatus() == true)
            {
                print(portal.name + " TRUE!!! ");
                waveInProcess = true;
            }
            else
            {
                float buffTime = portal.TimeToNextWave();
                if (0 < buffTime && buffTime < timeToNextWave)
                {
                    timeToNextWave = buffTime;
                }
                
            }
        }
        return (waveInProcess, timeToNextWave);
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


}
