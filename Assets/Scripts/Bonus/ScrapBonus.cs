using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapBonus : Bonus
{
    [SerializeField]
    private float scrapValue;

    [SerializeField]
    private string scrapColor;

    [SerializeField]
    private GameHandler gameHandler;

    [SerializeField]
    private float aliveTime;


    private void Start()
    {
        base.OnCreate();
    }

    protected override IEnumerator BonusActivateEffect(Collider collision)
    {
        yield return new WaitForSeconds(0);
        gameHandler.PlayerGetScrap(scrapColor, scrapValue);
        Destroy(gameObject);

    }

    public void SetGameHandler(GameHandler gameHandelrOut)
    {
        gameHandler = gameHandelrOut;
    }

    public float ReturnAliveTime()
    {
        return aliveTime;
    }
}
