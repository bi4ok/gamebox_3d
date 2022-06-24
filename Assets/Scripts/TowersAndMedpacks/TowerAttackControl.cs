using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TowerAttackControl : MonoBehaviour
{
    [SerializeField]
    private Weapon gunScript;
    [SerializeField]
    private float attackRange;
    [SerializeField, Range(1,15)]
    private float attackSpeed;
    [SerializeField, Range(0, 1)]
    private float aimForwardOffset;

    public AudioManager audioManager;

    private Collider[] monsters = new Collider[] { };
    private Collider target;
    private float attackCooldown;
    private float nextAttackTime;


    public void OnCreate()
    {
        attackCooldown = 1 / attackSpeed;
        nextAttackTime = attackCooldown;
        gunScript.OnEquip(1, 1, gameObject, gunScript.name);
        gunScript.ChooseAudioManager(audioManager);
    }
    private void Update()
    {
        if (Time.time > nextAttackTime)
        {
            GetAllMonsters();
            if (monsters.Length > 0)
            {
                ChooseTarget();
            }
            if (!target)
                return;
            print(target.name);
            Aim();
            gunScript.Shoot();
            nextAttackTime = Time.time + attackCooldown;
        }

    }

    private void GetAllMonsters()
    {
        monsters = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Enemy"));
    }

    private void OnDrawGizmos()
    {
        // Set the color of Gizmos to green
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void ChooseTarget()
    {
        if (target && monsters.Contains(target))
            return;
        if (monsters.Length > 0)
        {
            target = ChooseNearestTarget();
            
        }
    }

    private Collider ChooseNearestTarget()
    {
        Collider nearestTarget = null;
        float minDist = Mathf.Infinity;
        foreach (Collider mob in monsters)
        {
            var distanceToMob = Vector3.Distance(transform.position, mob.transform.position);
            if (minDist > distanceToMob)
            {
                minDist = distanceToMob;
                nearestTarget = mob;
            }
        }
        return nearestTarget;
    }

    private void Aim()
    {
        var direction = (target.transform.position + target.transform.forward * aimForwardOffset) - transform.position;
        direction.y = 0;
        transform.forward = direction;
        Debug.DrawLine(transform.position, direction);
    }

}
