using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solt : MonoBehaviour
{
    private MonsterController monsterController;
    private void OnTriggerStay(Collider other)
    {
        monsterController = other.GetComponent<MonsterController>();
        if (monsterController != null) monsterController.TakeDamage(1f, "Solt", false);
    }
   public IEnumerator Lifetime( float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
