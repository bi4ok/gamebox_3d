using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleController : MonoBehaviour, IDamageAble
{
    
    private float hitpoints;
    [SerializeField]
    private float maxhitpoints;

    [SerializeField]
    private GameObject deathEffect;
    [SerializeField]
    private Image linelife;

    [SerializeField]
    private GameObject gameManager;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip twentfive;
    [SerializeField]
    private AudioClip fifty;
    [SerializeField]
    private AudioClip seventyfive;
    private bool twentufive = true;
    private bool fifti = true;
    private bool seventy = true;

    private bool _alive = true;
    private Character _stats;

    private void Start()
    {
        hitpoints = maxhitpoints;
        _stats = new Character(hitpoints);
    }
    void Update()
    {
        linelife.fillAmount = _stats.health / maxhitpoints;
      if(linelife.fillAmount < 0.75f && seventy)
        {
            audioSource.clip =seventyfive;
            audioSource.Play();
            seventy = false;
        }
      if(linelife.fillAmount < 0.5 && fifti)
        {
            audioSource.clip = fifty;
            audioSource.Play();
            fifti = false;
        }
      if(linelife.fillAmount < 0.25 && twentufive)
        {
            audioSource.clip = twentfive;
            audioSource.Play();
            twentufive = false;
        }
    }

    public void TakeDamage(float damageAmount, string damageFrom, bool knockback = false)
    {
        if (_alive)
        {
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

    public void Upgrade()
    {
        
        Item hpBoost = new Item(healthPercent:50);
        print("дн сксвьемхъ - " + _stats.health);
        hpBoost.Equip(_stats);
        _stats.TakeHeal(_stats.statsOut["health"].Value - _stats.health);
        print(_stats.statsOut["health"].Value);
        print("оняке сксвьемхъ - " + _stats.health);
    }
}
