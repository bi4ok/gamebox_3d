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
    private AudioClip blasterSound;

    [SerializeField]
    private AudioSource audioSource;

    private float baseDamage;
    private float attackSpeed;
    private float _nextRangeAttackTime = 0f;

    public void OnEquip(float damage, float speed)
    {
        baseDamage = damage;
        attackSpeed = speed;
    }

    protected abstract void BulletSpawn(GameObject bullet, float damage, float speed, Transform pointOfAttack);

    public void Shoot(GameObject bullet)
    {
        if (Time.time > _nextRangeAttackTime)
        {
            BulletSpawn(bullet, baseDamage, attackSpeed, pointOfAttack);
            audioSource.PlayOneShot(blasterSound, 0.1f);
            _nextRangeAttackTime = Time.time + 1 / rangeAttackCoolDown;
        }
    }
}
