using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Weapon
{

    protected override void BulletSpawn(GameObject bullet, float damage, float speed, float range, bool knockback, Transform pointOfAttack)
    {
        for (float i=0; i<6; i++)
        {
            var angles = pointOfAttack.rotation.eulerAngles;
            angles.x += 5 * i;
            var bulletRotation = Quaternion.Euler(angles);
            GameObject bulletObject = Instantiate(bullet, pointOfAttack.position, bulletRotation);
            Rigidbody bulletBody = bulletObject.GetComponent<Rigidbody>();
            Bullet bulletInside = bulletObject.GetComponent<Bullet>();
            bulletInside.damage = damage;
            bulletInside.range = range;
            bulletInside.knockback = knockback;
            bulletBody.AddForce(pointOfAttack.forward * speed, ForceMode.Impulse);
        }

        
    }

}
