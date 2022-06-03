using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Factory : MonoBehaviour
{
    [System.Serializable]
    public class ObjToSpawn
    {
        public string name;
        public GameObject objectPrefab;
        public GameObject scrapPrefab;
        public int count;
        public float spawnTimeFrom;
        public bool spawned = false;

    }

    [System.Serializable]
    public class WaveOfObjects
    {
        public int order;
        public string name;
        public List<ObjToSpawn> allSpawnObjects;
        public float timeToNextWave;
        public bool lastWave;

    }

    [SerializeField]
    private List<WaveOfObjects> allWaves;

    [SerializeField]
    private bool loopWave;

    [SerializeField]
    private bool isMonsters;

    [SerializeField]
    private float interval;

    [SerializeField]
    private GameHandler gameHandler;

    private List<ObjToSpawn> currentSpawnObjects;
    private int waveOrder;
    private WaveOfObjects currentWave;
    private int maxWaveOrder;
    private bool fightPhase;
    private float nextWaveTime=0;
    private bool currentWaveInProcess;

    private void Awake()
    {
        currentSpawnObjects = new List<ObjToSpawn>();
        waveOrder = 0;
        maxWaveOrder = allWaves.Capacity;
        fightPhase = true;
        currentWaveInProcess = true;

    }

    private void Update()
    {
        if (allWaves.Count > 0)
        {
            if (fightPhase)
            {
                WhoToSpawnUpdate();
                StartCoroutine(SpawnObjects());
                fightPhase = false;
                print("����� �����������");

            }
            else
            {
                if (Time.time > nextWaveTime && !currentWaveInProcess)
                {
                    fightPhase = ChooseNextWave();
                }
            }
        }
        else if(currentWaveInProcess)
        {
            currentWaveInProcess = false;
        }


    }

    private bool ChooseNextWave()
    {
        


        if (waveOrder + 1 < maxWaveOrder)
        {
            waveOrder += 1;
        }
        else if (loopWave)
        {
            waveOrder = 0;
        }
        else
        {

            return false;
        }

        currentWaveInProcess = true;

        return true;
    }

    private void WhoToSpawnUpdate()
    {
        currentWave = allWaves[waveOrder];
        currentSpawnObjects = currentWave.allSpawnObjects;

        if (isMonsters)
        {
            print("����� � ����� ��������� " + currentSpawnObjects.Count);
            gameHandler.CountMonstersInGame(currentSpawnObjects.Count);
        }
    }

    private IEnumerator SpawnObjects()
    {

        yield return SpawnObject(interval, currentSpawnObjects);
        
        nextWaveTime = Time.time + currentWave.timeToNextWave;
        currentWaveInProcess = false;
        
    }

    public bool GetWaveStatus()
    {
        return currentWaveInProcess;
    }

    public float TimeToNextWave()
    {
        return nextWaveTime - Time.time;
    }

    public bool HaveNextWave()
    {
        if (currentWave != null)
        {
            return !currentWave.lastWave;
        }
        else
        {
            return false;
        }
        
    }

    protected abstract IEnumerator SpawnObject(float interval, List<ObjToSpawn> spawnObjectst);

}
