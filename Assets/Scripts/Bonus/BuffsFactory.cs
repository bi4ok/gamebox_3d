using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffsFactory : Factory
{

    [SerializeField]
    private float timeToBonusLive;

    [SerializeField]
    private GameObject bonusHandler;

    protected override IEnumerator SpawnObject(float interval, List<ObjToSpawn> spawnObjects)
    {
        foreach (var spawnObject in spawnObjects)
        {
            for (int i = 0; i < spawnObject.count; i++)
            {
                yield return new WaitForSeconds(interval);
                var placeForSpawn = transform.position + Vector3.up;
                GameObject objFromPrefab = Instantiate(spawnObject.objectPrefab, placeForSpawn, Quaternion.identity);

                var objScript = objFromPrefab.GetComponent<Bonus>();
                objScript.aliveBonusTimer = timeToBonusLive;
                objScript.bonusHandler = bonusHandler;
                objFromPrefab.SetActive(true);
            }
        }
    }
}
