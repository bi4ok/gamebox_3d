using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Weapon
{

    protected override void BulletSpawn(GameObject bullet, float damage, float speed, float range, bool knockback, Transform pointOfAttack)
    {

        var x = Random.Range(-30f, 30f);
        var bulletRotation = Quaternion.Euler(x, 0, 0);
        print(bulletRotation);
        GameObject bulletObject = Instantiate(bullet, pointOfAttack.position, bulletRotation);
        Rigidbody bulletBody = bulletObject.GetComponent<Rigidbody>();
        Bullet bulletInside = bulletObject.GetComponent<Bullet>();
        bulletInside.damage = damage;
        bulletInside.range = range;
        bulletInside.knockback = knockback;
        var newPoint = pointOfAttack;
        bulletBody.AddForce(pointOfAttack.forward * speed, ForceMode.Impulse);
    }

}
