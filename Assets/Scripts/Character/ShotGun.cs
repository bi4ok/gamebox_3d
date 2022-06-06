using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Weapon
{

    protected override void BulletSpawn(GameObject bullet, float damage, float speed, float range, bool knockback, bool upgrade, Transform pointOfAttack)
    {
        for (float i=-5; i<=4; i++)
        {
            var newPoint = pointOfAttack;
            var x = 3 * i;
            var newAngle = Quaternion.AngleAxis(x, Vector3.one);
            var test = newPoint.rotation;
            newPoint.rotation = Quaternion.Euler(
                newPoint.rotation.eulerAngles.x + newAngle.eulerAngles.x,
                newPoint.rotation.eulerAngles.y + newAngle.eulerAngles.y,
                newPoint.rotation.eulerAngles.z + newAngle.eulerAngles.z);

            GameObject bulletObject = Instantiate(bullet, newPoint.position, newPoint.rotation);
            Rigidbody bulletBody = bulletObject.GetComponent<Rigidbody>();
            Bullet bulletInside = bulletObject.GetComponent<Bullet>();
            bulletInside.damage = damage;
            bulletInside.range = range;
            bulletInside.knockback = knockback;
            bulletBody.AddForce(newPoint.forward * speed, ForceMode.Impulse);
            newPoint.rotation = test;
        }

        
    }

}
