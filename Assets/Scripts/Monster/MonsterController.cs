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
    //[SerializeField]
    //private float audioVolume=1f;
    //[SerializeField]
    //private AudioClip death;
    [SerializeField]
    private GameObject soundHanlder;
    [SerializeField]
    private string namesound_death;
    private AudioManager audioManager;



    private GameHandler gameHandler;

    private Character _monsterInside;
    private Vector3 _vectorToTarget;
    private Vector3 _vectorToCastle;
    private float _attackCoolDownTimer;
    private Animator _monsterAnimator;
    private string _lastAttacker;

    private Vector3 _castleTargetPosition;
    private GameObject _castle;
    private GameObject _castleHeart;
    private Collider[] _buffersNear;
    private float _changeTargetRange;
    private float _changeCastleRange;
    private bool locked = false;
    private bool dead = false;
    private bool devilShield = false;
    private float shieldCD = 0.1f;
    private float nextShieldCastTime = 0f;
    private float timeToAttackCastle = 0f;
    private NavMeshHit hitNavCastle;
    

    public void OnCreate(GameObject target, GameObject castle, GameObject heartOfCastle, float changeTargetRange, GameObject bonus, GameObject scrapPrefab)
    {
        audioManager = FindObjectOfType<AudioManager>(); 
        _monsterAnimator = GetComponent<Animator>();
        bonusHandler = bonus;
        gameHandler = bonusHandler.GetComponent<GameHandler>();
        scrap = scrapPrefab;
        
        _changeTargetRange = changeTargetRange + attackRange;
        targetToAttack = target;
        //_castleTargetPosition =  castle.transform.position + Random.insideUnitSphere*attackRange;
        _castleTargetPosition = castle.transform.position + Random.insideUnitSphere * attackRange;
        _changeCastleRange = 8;
        NavMesh.SamplePosition(_castleTargetPosition, out hitNavCastle, 10.0f, NavMesh.AllAreas);
        print(hitNavCastle.position + " HIT NAV CASTLE POSITION");
        _castle = castle;
        _castleHeart = heartOfCastle;
        agent.SetDestination(_castleTargetPosition);

        _monsterInside = new Character(startHealth, damageValue, energyValue, shieldPower, attackRange, attackSpeed, movementSpeed, tag);
        agent.speed = movementSpeed;
        agent.stoppingDistance = 1;
        _monsterAnimator.speed = movementSpeed/3;
        _vectorToTarget = targetToAttack.transform.position - transform.position;
        _attackCoolDownTimer = 0;

        if (gunScript != null)
            gunScript.OnEquip(damageValue, attackSpeed, gameObject, gunScript.name);

        if (shieldEffect != null)
            shieldEffect.Stop();


        
    }

    private void FixedUpdate()
    {

        CalculateMovementVector();

        if (!buffer && shieldEffect != null)
        {
            CheckBuff();
            if (devilShield && !shieldEffect.isPlaying)
            {
                shieldEffect.Play();
            }
            else if (!devilShield && shieldEffect.isPlaying)
            {
                shieldEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
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
                transform.LookAt(targetToAttack.transform);
                Attack(targetToAttack.gameObject);
            }
        }
        else
        {
            if (_vectorToCastle.magnitude > _monsterInside.statsOut["range"].Value)
            {
                _monsterAnimator.SetFloat("Speed", _monsterInside.statsOut["movementSpeed"].Value);
                agent.isStopped = false;
            }
            else
            {
                _monsterAnimator.SetFloat("Speed", 0);
                agent.isStopped = true;
                transform.LookAt(_castleHeart.transform);
                Attack(_castle);
            }
            
            
        }
        


    }


    private void OnDrawGizmos()
    {
        // Set the color of Gizmos to green
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, shieldRange);

        //Gizmos.DrawWireSphere(_castleTargetPosition, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitNavCastle.position, attackRange);


        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(_castleTargetPosition, _changeCastleRange);
    }

    private void GetTarget()
    {
        if (lockOnPlayer)
        {
            transform.LookAt(targetToAttack.transform.position);
            agent.SetDestination(targetToAttack.transform.position);
            return;
        }

        if ((hitNavCastle.position - transform.position).magnitude <= attackRange)
        {
            locked = true;
            return;
        }


        if (buffer)
        {

            
            if (Time.time > timeToAttackCastle)
            {
                agent.SetDestination(hitNavCastle.position);
            }
            else
            {
                Transform startTransform = transform;
                transform.rotation = Quaternion.LookRotation(transform.position - targetToAttack.transform.position);
                Vector3 runTo = transform.position + transform.forward * 5;
                NavMeshHit hit;
                NavMesh.SamplePosition(runTo, out hit, 10, NavMesh.AllAreas);
                transform.position = startTransform.position;
                transform.rotation = startTransform.rotation;
                agent.SetDestination(hit.position);
            }
            return;
        }

        if (_vectorToTarget.magnitude > _changeTargetRange || !targetToAttack.activeSelf)
        {
            transform.LookAt(hitNavCastle.position);
            agent.SetDestination(hitNavCastle.position);
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
        _vectorToCastle = hitNavCastle.position - transform.position;
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
                audioManager.PlaySounds("shoot rusalka 1");
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
            timeToAttackCastle = Time.time + 2;
        }
        
        _lastAttacker = damageFrom;
        if (knockback)
        {
            KnockBack(damageAmount * 2);
           
        }
        DiedByDamage();

    }

    //public IEnumerator ShieldSelf()
    //{
    //    if (shieldEffect != null && !shieldEffect.isPlaying)
    //    {
    //        shieldEffect.Play();
    //        yield return new WaitForSeconds(shieldCD);
    //        shieldEffect.Stop();
    //    }
    //}

    private void CheckBuff()
    {
        if (Time.time > nextShieldCastTime)
        {
            _buffersNear = Physics.OverlapSphere(transform.position, shieldRange, LayerMask.GetMask("Buffer"));
            devilShield = _buffersNear.Length > 0;
            nextShieldCastTime = Time.time + shieldCD;
        }
    }


    //private void CastShieldOnMonsters()
    //{
    //    if (Time.time > nextShieldCastTime)
    //    {
    //        print("CAST THIS SHIELD");
    //        Collider[] monstersAround = Physics.OverlapSphere(transform.position, shieldRange, LayerMask.GetMask("Enemy"));
    //        _monsterAnimator.SetTrigger("Attack");
    //        foreach (var monster in monstersAround)
    //        {
    //            print(monster.name);
    //            MonsterController mobScript = monster.GetComponent<MonsterController>();
    //            if (mobScript != null)
    //            {
    //                StartCoroutine(mobScript.ShieldSelf());
    //            }
    //        }
    //        nextShieldCastTime += shieldCD;
    //    }
    //}

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
                GameObject scrapObj = Instantiate(scrap, transform.position + Vector3.up, Quaternion.identity);
                var scrapBonus = scrapObj.GetComponent<ScrapBonus>();
                scrapBonus.SetStats(gameHandler, (int)Random.Range(scrapValueMin, scrapValueMax));
                var objScript = scrapObj.GetComponent<Bonus>();
                objScript.aliveBonusTimer = scrapBonus.ReturnAliveTime();
                objScript.bonusHandler = bonusHandler;

            }
            GameObject soundObj = Instantiate(soundHanlder, transform.position, Quaternion.identity);
            AudioSource soundSource = soundObj.GetComponent<AudioSource>();
            audioManager.PlaySounds(namesound_death);
            
            Destroy(soundSource, 1f);
            BonusController bonusScript = bonusHandler.GetComponent<BonusController>();
            bonusScript.PlayerScoresUp(scoreForKill, _lastAttacker);
            
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
