using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    private Transform pointOfAttack;

    [SerializeField]
    private float rangeAttackCoolDown;

    [SerializeField]
    private float weaponDamage;

    [SerializeField]
    private float weaponRange;

    [SerializeField]
    private bool knockback;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Light glowEffect;

    [SerializeField]
    private Color glowColor;

    [SerializeField]
    private string scrapName;

    [SerializeField]
    private AudioClip blasterSound;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private GameHandler gameHandler;
    private string name_shoot;

    [SerializeField]
    private GameObject scrapUI;

    private float baseDamage;
    private float attackSpeed;
    private float _nextRangeAttackTime = 0f;
    private GameObject glow;
    private GameObject attacker;
    private bool _isUpgrade;

    public void OnEquip(float damage, float speed, GameObject player, string name)
    {
        scrapUI.SetActive(true);
        print(scrapUI.name + " включено");
        baseDamage = damage;
        attackSpeed = speed;
        if (glowEffect)
            glowEffect.color = glowColor;
        attacker = player;
        name_shoot = name;
    }

    public void UnEquip()
    {
        scrapUI.SetActive(false);
        print(scrapUI.name + " выключено");
        Destroy(glow);
    }

    protected abstract void BulletSpawn(GameObject bullet, 
        float damage, float speed, float range, bool knockback, bool upgrade, 
        Transform pointOfAttack,
        GameObject attacker);

    public void Shoot()
    {
        if (Time.time > _nextRangeAttackTime)
        {
            if (scrapName == "tower" || (gameHandler != null && gameHandler.PlayerTryWasteScrap(scrapName, 1)))
            {
                BulletSpawn(bulletPrefab, weaponDamage, attackSpeed, weaponRange, knockback, _isUpgrade, pointOfAttack, attacker);
                if(name_shoot == "Vintovka")
                {
                    audioManager.PlaySounds(name_shoot + " " + Random.Range(1, 3).ToString());
                }
                else if(name_shoot == "Drobovic")
                {
                    audioManager.PlaySounds(name_shoot + " " + Random.Range(1, 2).ToString());
                }
                
                _nextRangeAttackTime = Time.time + 1 / rangeAttackCoolDown;
            }

        }
    }

    public string CheckWeaponColor()
    {
        return scrapName;
    }

    public void UpgradeWeapon()
    {
        _isUpgrade = true;
    }
}
