using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : Factory
{

    [SerializeField]
    private GameObject targetToAttack;

    [SerializeField]
    private GameObject targetToFollow;

    [SerializeField]
    private float changeTargetRange;

    [SerializeField]
    private GameObject bonusHandler;

    protected override IEnumerator SpawnObject(float interval, List<ObjToSpawn> spawnObjects)
    {
        foreach (var spawnObject in spawnObjects)
        {
            for (int i = 0; i < spawnObject.count; i++)
            {
                yield return new WaitForSeconds(interval);
                GameObject objFromPrefab = Instantiate(spawnObject.objectPrefab, transform.position + Vector3.up, Quaternion.identity);
                objFromPrefab.SetActive(true);
                var objScript = objFromPrefab.GetComponent<MonsterController>();
                if (targetToFollow != null)
                {
                    objScript.OnCreate(targetToAttack, targetToFollow, changeTargetRange, bonusHandler, spawnObject.scrapPrefab);
                }


            }
        }


    }
}
