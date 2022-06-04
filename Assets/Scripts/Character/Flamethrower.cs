using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Weapon
{

    protected override void BulletSpawn(GameObject bullet, float damage, float speed, float range, bool knockback, Transform pointOfAttack)
    {
        GameObject bulletObject = Instantiate(bullet, pointOfAttack.position, pointOfAttack.transform.rotation);
        Rigidbody bulletBody = bulletObject.GetComponent<Rigidbody>();
        Bullet bulletInside = bulletObject.GetComponent<Bullet>();
        bulletInside.damage = damage;
        bulletInside.range = range;
        bulletInside.knockback = knockback;
        bulletBody.AddForce(pointOfAttack.forward * speed, ForceMode.Impulse);
    }

}
