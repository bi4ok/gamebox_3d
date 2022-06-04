using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapBonus : Bonus
{
    [SerializeField]
    private string scrapColor;

    [SerializeField]
    private GameHandler gameHandler;

    [SerializeField]
    private float aliveTime;

    private float scrapValue;

    private void Awake()
    {
        base.OnCreate();
    }

    protected override IEnumerator BonusActivateEffect(Collider collision)
    {
        yield return new WaitForSeconds(0);
        gameHandler.PlayerGetScrap(scrapColor, scrapValue);
        Destroy(gameObject);

    }

    public void SetStats(GameHandler gameHandelrOut, float value)
    {
        gameHandler = gameHandelrOut;
        scrapValue = value;
    }

    public float ReturnAliveTime()
    {
        return aliveTime;
    }
}
