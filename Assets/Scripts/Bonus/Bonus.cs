using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bonus : MonoBehaviour
{
    [SerializeField]
    public float aliveBonusTimer;

    [SerializeField]
    public GameObject bonusHandler;

    private bool isActivated = false;


    protected void OnCreate()
    {
        StartCoroutine(BonusDissappear());
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.name);
        if (collision.CompareTag("Player"))
        {
            isActivated = true;
            BonusController bonusScript = bonusHandler.GetComponent<BonusController>();
            bonusScript.PlayerScoresUp();
            StartCoroutine(BonusActivateEffect(collision));
        }
    }

    protected abstract IEnumerator BonusActivateEffect(Collider collision);

    private IEnumerator BonusDissappear()
    {
        yield return new WaitForSeconds(aliveBonusTimer);
        if (!isActivated)
        {
            Destroy(gameObject);
        }
    }
}
