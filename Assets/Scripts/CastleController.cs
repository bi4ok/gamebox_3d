using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleController : MonoBehaviour, IDamageAble
{
    [SerializeField]
    private float hitpoints;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject gameManager;

    private bool _alive = true;
    private Character _stats;

    private void Start()
    {
        _stats = new Character(hitpoints);
    }

    public void TakeDamage(float damageAmount, string damageFrom, bool knockback = false)
    {
        if (_alive)
        {
            print("TAKE DAMAGE" + damageAmount + damageFrom);
            _stats.TakeDamage(damageAmount, damageFrom);
            DiedByDamage();
        }

    }

    public bool DiedByDamage()
    {
        if (_stats.DiedByDamage())
        {
            print("CASTLE DIED");
            if (deathEffect)
            {
                GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
                SpriteRenderer effectSprite = effect.GetComponent<SpriteRenderer>();
                Destroy(effect, 3f);
            }
            _alive = false;
            return true;
        }

        return false;

    }
}
