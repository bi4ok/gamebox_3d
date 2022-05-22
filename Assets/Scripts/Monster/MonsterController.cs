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
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject bonusHandler;
    [SerializeField]
    private NavMeshAgent agent;

    private GameHandler gameHandler;

    private Character _monsterInside;
    private Vector3 _vectorToTarget;
    private float _attackCoolDownTimer;
    private Animator _monsterAnimator;
    private string _lastAttacker;

    private Vector3 _castleTargetPosition;
    private GameObject _castle;
    private float _changeTargetRange;
    private bool locked=false;

    public void OnCreate(GameObject target, GameObject castle, float changeTargetRange, GameObject bonus, GameObject scrapPrefab)
    {
        _monsterAnimator = GetComponent<Animator>();
        bonusHandler = bonus;
        gameHandler = bonusHandler.GetComponent<GameHandler>();
        scrap = scrapPrefab;
        
        _changeTargetRange = changeTargetRange;
        targetToAttack = target;
        _castleTargetPosition =  castle.transform.position + Random.insideUnitSphere*attackRange;
        _castle = castle;
        agent.SetDestination(_castleTargetPosition);

        _monsterInside = new Character(startHealth, damageValue, energyValue, shieldPower, attackRange, attackSpeed, movementSpeed, tag);
        agent.speed = movementSpeed;
        agent.stoppingDistance = 1;
        _monsterAnimator.speed = movementSpeed;
        _vectorToTarget = targetToAttack.transform.position - transform.position;
        _attackCoolDownTimer = 0;
    }

    private void FixedUpdate()
    {

        CalculateMovementVector();
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

    private void GetTarget()
    {
        if ((_castleTargetPosition - transform.position).magnitude <= attackRange)
        {
            locked = true;
            agent.isStopped = true;
            agent.SetDestination(_castleTargetPosition);
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

            // Ищем цель и наносим урон
            if (bulletPrefab)
            {
                RangeAttack(target);
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

    private void RangeAttack(GameObject target)
    {
        Vector3 deltaDirection = (target.transform.position - transform.position).normalized;
        Debug.DrawLine(transform.position + (deltaDirection * 2), target.transform.position);
        Vector3 pointOfAttack = transform.position + deltaDirection;

        GameObject bullet = Instantiate(bulletPrefab, pointOfAttack, transform.rotation);
        Rigidbody bulletBody = bullet.GetComponent<Rigidbody>();
        Bullet bulletInside = bullet.GetComponent<Bullet>();
        bulletInside.damage = _monsterInside.statsOut["damage"].Value;
        bulletInside.ChooseAttacker(tag);
        bulletBody.AddForce(deltaDirection * _monsterInside.statsOut["attackSpeed"].Value * _monsterInside.statsOut["movementSpeed"].Value, ForceMode.Impulse);
    }

    public void TakeDamage(float damageAmount, string damageFrom, bool knockback=false)
    {
        _monsterAnimator.SetTrigger("Hitted");
        _monsterInside.TakeDamage(damageAmount, damageFrom);
        _lastAttacker = damageFrom;
        if (knockback)
        {
            KnockBack(damageAmount / 2);
        }
        DiedByDamage();

    }

    public bool DiedByDamage()
    {
        if (_monsterInside.DiedByDamage())
        {
            if (deathEffect)
            {
                GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
                Destroy(effect, 3f);
            }

            if (scrap)
            {
                GameObject scrapObj = Instantiate(scrap, transform.position, Quaternion.identity);
                var scrapBonus = scrapObj.GetComponent<ScrapBonus>();
                scrapBonus.SetGameHandler(gameHandler);
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
            Destroy(gameObject);
        }
        return true;
    }

    public void KnockBack(float power)
    {
        agent.velocity = transform.forward * -1 * power;
        
    }
}
