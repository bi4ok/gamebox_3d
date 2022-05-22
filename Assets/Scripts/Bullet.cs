using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject hitEffect;

    private string attackerTag="Player";
    public float damage;

    private void Start()
    {
        Destroy(gameObject, 2f);
        StartCoroutine(BlastEffect(1.9f));
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
        if(!collision.CompareTag(attackerTag) && !collision.CompareTag("Bonus") && !collision.CompareTag("Bullet"))
        {
            Debug.Log(collision.name);
            BlastHim(collision);
            StartCoroutine(BlastEffect(0f));
            Destroy(gameObject, 0.1f);

        }

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
            enemy.TakeDamage(damage, gameObject.tag);
        }
        

    }

    public void ChooseAttacker(string tag)
    {
        attackerTag = tag;
    }

}
