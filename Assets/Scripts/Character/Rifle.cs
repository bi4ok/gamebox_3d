using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{

    protected override void BulletSpawn(GameObject bullet, float damage, float speed, Transform pointOfAttack)
    {
        GameObject bulletObject = Instantiate(bullet, pointOfAttack.position, pointOfAttack.rotation);
        Rigidbody bulletBody = bulletObject.GetComponent<Rigidbody>();
        Bullet bulletInside = bulletObject.GetComponent<Bullet>();
        bulletInside.damage = damage;
        bulletBody.AddForce(pointOfAttack.forward * speed, ForceMode.Impulse);
    }

}
