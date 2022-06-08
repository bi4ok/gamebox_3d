using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Weapon
{

    protected override void BulletSpawn(GameObject bullet, float damage, float speed, float range, bool knockback, bool upgrade, Transform pointOfAttack)
    {
        var newPoint = pointOfAttack;
        
        var delta = upgrade ? 21 : 3;
        var deltaAngle = Random.Range(-delta, delta);
        var newAngle = Quaternion.AngleAxis(deltaAngle, Vector3.one);
        var test = newPoint.rotation;
        newPoint.rotation = Quaternion.Euler(
            newPoint.rotation.eulerAngles.x + newAngle.eulerAngles.x,
            newPoint.rotation.eulerAngles.y + newAngle.eulerAngles.y,
            newPoint.rotation.eulerAngles.z + newAngle.eulerAngles.z);
        GameObject bulletObject = Instantiate(bullet, newPoint.position, newPoint.rotation);
        Rigidbody bulletBody = bulletObject.GetComponent<Rigidbody>();
        Bullet bulletInside = bulletObject.GetComponent<Bullet>();
        bulletInside.damage = damage;
        bulletInside.range = upgrade ? range * 2 : range;
        bulletInside.knockback = knockback;
        bulletInside.bounce = false;
        bulletBody.AddForce(newPoint.forward * speed, ForceMode.Impulse);
        newPoint.rotation = test;
    }

}
