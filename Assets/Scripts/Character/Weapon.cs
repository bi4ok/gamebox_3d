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
    private GameHandler gameHandler;

    private float baseDamage;
    private float attackSpeed;
    private float _nextRangeAttackTime = 0f;
    private GameObject glow;
    private bool _isUpgrade;

    public void OnEquip(float damage, float speed, GameObject player)
    {
        baseDamage = damage;
        attackSpeed = speed;
        glowEffect.color = glowColor;
    }

    public void UnEquip()
    {
        Destroy(glow);
    }

    protected abstract void BulletSpawn(GameObject bullet, float damage, float speed, float range, bool knockback, bool upgrade, Transform pointOfAttack);

    public void Shoot(GameObject bullet)
    {
        if (Time.time > _nextRangeAttackTime)
        {
            if (gameHandler != null && gameHandler.PlayerTryWasteScrap(scrapName, 1))
            {
                BulletSpawn(bullet, weaponDamage, attackSpeed, weaponRange, knockback, _isUpgrade, pointOfAttack);
                audioSource.PlayOneShot(blasterSound, 0.1f);
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
