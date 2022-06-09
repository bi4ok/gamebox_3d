using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour, IDamageAble, IDamageDealer<GameObject>
{
    [SerializeField]
    private float startHealth;
    [SerializeField]
    private float damageValue;
    [SerializeField]
    private float energyValue;
    [SerializeField, Range(0, 1)]
    private float shieldPower;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float attackRange=1f;
    [SerializeField]
    private float attackSpeed = 1f;
    [SerializeField]
    private float scoreForKill;
    [SerializeField]
    private bool endAfterDie;


    [SerializeField]
    private GameObject targetToAttack;
    [SerializeField]
    private GameObject deathEffect;
    [SerializeField]
    private GameObject scrap;
    [SerializeField]
    private float scrapValueMin;
    [SerializeField]
    private float scrapValueMax;
    [SerializeField]
    private Weapon gunScript;
    [SerializeField]
    private GameObject bonusHandler;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private ParticleSystem shieldEffect;
    [SerializeField]
    private bool buffer = false;
    [SerializeField]
    private bool lockOnPlayer = false;
    [SerializeField]
    private float shieldRange;

    private GameHandler gameHandler;

    private Character _monsterInside;
    private Vector3 _vectorToTarget;
    private float _attackCoolDownTimer;
    private Animator _monsterAnimator;
    private string _lastAttacker;

    private Vector3 _castleTargetPosition;
    private GameObject _castle;
    private float _changeTargetRange;
    private bool locked = false;
    private bool dead = false;
    private bool devilShield = true;
    private float shieldCD = 1f;
    private float nextShieldCastTime = 0f;
    private float timeToAttackCastle = 0f;
    

    public void OnCreate(GameObject target, GameObject castle, float changeTargetRange, GameObject bonus, GameObject scrapPrefab)
    {
        _monsterAnimator = GetComponent<Animator>();
        bonusHandler = bonus;
        gameHandler = bonusHandler.GetComponent<GameHandler>();
        scrap = scrapPrefab;
        
        _changeTargetRange = changeTargetRange + attackRange;
        targetToAttack = target;
        _castleTargetPosition =  castle.transform.position + Random.insideUnitSphere*attackRange;
        _castle = castle;
        agent.SetDestination(_castleTargetPosition);

        _monsterInside = new Character(startHealth, damageValue, energyValue, shieldPower, attackRange, attackSpeed, movementSpeed, tag);
        agent.speed = movementSpeed;
        agent.stoppingDistance = 1;
        _monsterAnimator.speed = movementSpeed/3;
        _vectorToTarget = targetToAttack.transform.position - transform.position;
        _attackCoolDownTimer = 0;

        if (gunScript != null)
            gunScript.OnEquip(damageValue, attackSpeed, gameObject);

        if (shieldEffect != null)
            shieldEffect.Stop();
    }

    private void FixedUpdate()
    {

        CalculateMovementVector();

        if (buffer)
        {
            CastShieldOnMonsters();
        }

        if (shieldEffect != null && shieldEffect.isPlaying)
        {
            devilShield = true;
        }
        else
        {
            devilShield = false;
        }

        if (!locked)
        {
            GetTarget();

            if (_vectorToTarget.magnitude > _monsterInside.statsOut["range"].Value || !targetToAttack.activeSelf)
            {
                _monsterAnimator.SetFloat("Speed", _monsterInside.statsOut["movementSpeed"].Value);
                agent.isStopped = false;
            }
            else
            {
                _monsterAnimator.SetFloat("Speed", 0);
                agent.isStopped = true;
                Attack(targetToAttack.gameObject);
            }
        }
        else
        {
            Attack(_castle);
        }
        


    }

    private void CastShieldOnMonsters()
    {
        if (Time.time > nextShieldCastTime)
        {
            print("CAST THIS SHIELD");
            Collider[] monstersAround = Physics.OverlapSphere(transform.position, shieldRange, LayerMask.GetMask("Enemy"));
            _monsterAnimator.SetTrigger("Attack");
            foreach (var monster in monstersAround)
            {
                print(monster.name);
                MonsterController mobScript = monster.GetComponent<MonsterController>();
                if (mobScript != null)
                {
                    StartCoroutine(mobScript.ShieldSelf());
                }
            }
            nextShieldCastTime += shieldCD;
        }
    }

    private void OnDrawGizmos()
    {
        // Set the color of Gizmos to green
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, shieldRange);
    }

    private void GetTarget()
    {
        if (lockOnPlayer)
        {
            transform.LookAt(targetToAttack.transform.position);
            agent.SetDestination(targetToAttack.transform.position);
            return;
        }

        if ((_castleTargetPosition - transform.position).magnitude <= attackRange)
        {
            locked = true;
            agent.isStopped = true;
            agent.SetDestination(_castleTargetPosition);
            return;
        }


        if (buffer)
        {
            print("BUFFER get target");

            
            if (Time.time > timeToAttackCastle)
            {
                print(timeToAttackCastle);
                transform.LookAt(_castleTargetPosition);
                agent.SetDestination(_castleTargetPosition);
            }
            else
            {
                Transform startTransform = transform;
                transform.rotation = Quaternion.LookRotation(transform.position - targetToAttack.transform.position);
                Vector3 runTo = transform.position + transform.forward * 5;
                NavMeshHit hit;
                NavMesh.SamplePosition(runTo, out hit, 5, 1 << NavMesh.GetNavMeshLayerFromName("Default"));
                transform.position = startTransform.position;
                transform.rotation = startTransform.rotation;
                agent.SetDestination(hit.position);
                //var awayFromAttacker = (transform.position + (transform.position - targetToAttack.transform.position));
                //print(targetToAttack.transform.position);
                //awayFromAttacker.Normalize();
                //Debug.DrawRay(transform.position, awayFromAttacker);
                //Debug.DrawRay(transform.position, (-1) * awayFromAttacker, Color.red);
                //print("RUN AWAY");
                //transform.LookAt(awayFromAttacker);
                //agent.SetDestination(awayFromAttacker);
            }
            return;
        }

        if (_vectorToTarget.magnitude > _changeTargetRange || !targetToAttack.activeSelf)
        {
            transform.LookAt(_castleTargetPosition);
            agent.SetDestination(_castleTargetPosition);
        }
        else if (targetToAttack.activeSelf)
        {
            transform.LookAt(targetToAttack.transform.position);
            agent.SetDestination(targetToAttack.transform.position);
        }


    }


    private Vector3 CalculateMovementVector()
    {
        _vectorToTarget = targetToAttack.transform.position - transform.position;
        return transform.position + _vectorToTarget.normalized * _monsterInside.statsOut["movementSpeed"].Value * Time.fixedDeltaTime;
    }

    public void Attack(GameObject target)
    {
        if (_attackCoolDownTimer <= 0)
        {
            // Запускаем анимацию атаки
            _monsterAnimator.SetTrigger("Attack");

            var player = target.GetComponent<PlayerController>();
            if (player != null && player.CheckThorns())
            {
                TakeDamage(_monsterInside.statsOut["damage"].Value, "self");
            }

            // Ищем цель и наносим урон
            if (gunScript != null)
            {
                gunScript.Shoot();
            }
            else
            {
                MeeleeAttack(target);
            }
            
            _attackCoolDownTimer = attackSpeed;
        }
        else
        {
            _attackCoolDownTimer -= Time.fixedDeltaTime;
        }

    }

    private void MeeleeAttack(GameObject target)
    {
        var targetType = target.GetComponent<IDamageAble>();
        if (targetType == null) return;
        targetType.TakeDamage(_monsterInside.statsOut["damage"].Value, tag);
    }

    //private void RangeAttack(GameObject target)
    //{
    //    print("RANGE ATTACK");
    //    Vector3 deltaDirection = (target.transform.position - transform.position).normalized;
    //    Debug.DrawLine(transform.position + (deltaDirection * 2), target.transform.position);
    //    Vector3 pointOfAttack = transform.position + deltaDirection;

    //    GameObject bullet = Instantiate(bulletPrefab, pointOfAttack, transform.rotation);
    //    Rigidbody bulletBody = bullet.GetComponent<Rigidbody>();
    //    Bullet bulletInside = bullet.GetComponent<Bullet>();
    //    bulletInside.damage = _monsterInside.statsOut["damage"].Value;
    //    bulletInside.ChooseAttacker(tag);
    //    bulletBody.AddForce(deltaDirection * 1, ForceMode.Impulse);
    //    //_monsterInside.statsOut["attackSpeed"].Value * _monsterInside.statsOut["movementSpeed"].Value
    //}

    public void TakeDamage(float damageAmount, string damageFrom, bool knockback=false)
    {
        _monsterAnimator.SetTrigger("Hitted");
        if (!devilShield)
        {
            _monsterInside.TakeDamage(damageAmount, damageFrom);
        }

        if (buffer)
        {
            timeToAttackCastle = Time.time + 100;
        }
        
        _lastAttacker = damageFrom;
        if (knockback)
        {
            KnockBack(damageAmount * 2);
        }
        DiedByDamage();

    }

    public IEnumerator ShieldSelf()
    {
        if (shieldEffect != null && !shieldEffect.isPlaying)
        {
            shieldEffect.Play();
            yield return new WaitForSeconds(shieldCD);
            shieldEffect.Stop();
        }
    }

    public bool DiedByDamage()
    {
        if (_monsterInside.DiedByDamage() && !dead)
        {
            dead = true;
            if (deathEffect)
            {
                GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
                Destroy(effect, 3f);
            }

            if (scrap)
            {
                GameObject scrapObj = Instantiate(scrap, transform.position, Quaternion.identity);
                var scrapBonus = scrapObj.GetComponent<ScrapBonus>();
                scrapBonus.SetStats(gameHandler, (int)Random.Range(scrapValueMin, scrapValueMax));
                var objScript = scrapObj.GetComponent<Bonus>();
                objScript.aliveBonusTimer = scrapBonus.ReturnAliveTime();
                objScript.bonusHandler = bonusHandler;

            }

            // создание ошмётка
            BonusController bonusScript = bonusHandler.GetComponent<BonusController>();
            bonusScript.PlayerScoresUp(scoreForKill, _lastAttacker);
            
            if (endAfterDie)
            {
                gameHandler.EndGame();
            }
            gameHandler.MonsterIsDead();
            Destroy(gameObject);
        }
        return true;
    }

    public void KnockBack(float power)
    {
        agent.velocity = transform.forward * -1 * power;
        
    }
}
