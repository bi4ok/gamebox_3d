using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject hitEffect;

    private string attackerTag="Player";
    private string targetOfAttack= "Monster";
    public float damage;
    public float range;
    public bool knockback;
    public bool through = false;
    public bool bounce = false;
    private List<string> targetsOfAttack = new List<string>() { "Player", "Castle" };

    private void Start()
    {
        //targetOfAttack = (attackerTag == "Player" || attackerTag == "Tower") ? "Monster" : "Player";
        Destroy(gameObject, range);
        StartCoroutine(BlastEffect(range-0.01f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        ApplyDamage(collision.collider);
    }

    private void OnTriggerEnter(Collider collision)
    {
        ApplyDamage(collision);
    }

    private void ApplyDamage(Collider collision)
    {
        if (targetsOfAttack.Contains(collision.tag))
        {
            BlastHim(collision);
            StartCoroutine(BlastEffect(0f));
            if (collision.CompareTag(targetOfAttack))
            {
                if (!through)
                    Destroy(gameObject, 0f);
            }
            else
            {
                Destroy(gameObject, 0f);
            }
            if (bounce)
            {
                SpawnBounce();
            }
        }
        else if (collision.CompareTag("Bullet") || collision.CompareTag("Bonus"))
        {
            
        }
        else
        {
            Destroy(gameObject, 0f);
        }
        
        //if(!collision.CompareTag(attackerTag) 
        //    && !collision.CompareTag("Bonus") 
        //    && !collision.CompareTag("Bullet")
        //    && !collision.CompareTag("Tower")
        //    && !collision.CompareTag("Castle"))
        //{
        //    BlastHim(collision);
        //    StartCoroutine(BlastEffect(0f));
        //    if (collision.CompareTag(targetOfAttack))
        //    {
        //        if (!through)
        //            Destroy(gameObject, 0f);
        //    }
        //    else
        //    {
        //        Destroy(gameObject, 0f);
        //    }
        //    if (bounce)
        //    {
        //        SpawnBounce();
        //    }
            
        //}
        //else if (collision.CompareTag("Castle"))
        //{
        //    Destroy(gameObject, 0f);
        //}

    }

    private void SpawnBounce()
    {
        var delta = 180;
        var deltaAngle = Random.Range(-delta, delta);
        var newAngle = Quaternion.AngleAxis(deltaAngle, Vector3.one);
        transform.rotation = Quaternion.Euler(
            0,
            newAngle.eulerAngles.y,
            newAngle.eulerAngles.z);
        GameObject bulletObject = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
        Rigidbody bulletBody = bulletObject.GetComponent<Rigidbody>();
        Bullet bulletInside = bulletObject.GetComponent<Bullet>();
        bulletInside.damage = damage;
        bulletInside.range = range;
        bulletInside.knockback = knockback;
        bulletInside.bounce = false;
        bulletBody.AddForce(transform.forward * 1, ForceMode.Impulse);
    }

    private IEnumerator BlastEffect(float sec)
    {
        yield return new WaitForSeconds(sec);
        if (hitEffect)
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }
        
        
    }

    private void BlastHim(Collider collision)
    {
        IDamageAble enemy = collision.GetComponent<IDamageAble>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage, gameObject.tag, knockback);
        }
        

    }

    public void ChooseAttacker(string attacker)
    {
        attackerTag = attacker;
        //targetOfAttack = (attackerTag == "Player" || attackerTag == "Tower") ? "Monster" : "Player";
        if (attackerTag == "Player" || attackerTag == "Tower")
        {
            targetsOfAttack = new List<string>() { "Monster" };
        }
        else if (attackerTag == "Monster")
        {
            targetsOfAttack = new List<string>() { "Player", "Castle" };
        }
    }

}
