using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IDamageAble, IDamageDealer<GameObject>
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
    private float attackRange = 0.5f;
    [SerializeField]
    private float attackSpeed = 1f;

    [SerializeField]
    private Transform pointOfAttack;
    [SerializeField]
    private GameObject gun;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Weapon gunScript;

    [SerializeField]
    private GameHandler gameManager;
    [SerializeField]
    private AudioClip blasterSound;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private PlayerSpawner playerSpawner;
    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private LayerMask groundMask;

    private Character _characterInside;
    private Rigidbody _playerRigidBody;
    private Vector3 _movementAxes;
    private Animator _playerAnimator;

    private float _valuesRegenCooldown = 5f;
    private float _valuesRegenTimer = 0f;
    private float _meeleAttackCoolDown;
    private float _nextMeeleAttackTime = 0f;

    private float _rangeAttackCoolDown;
    private float _nextRangeAttackTime = 0f;

    private float _nextAttackTime = 0f;
    private float _attackCoolDown = 0.1f;

    public bool alive = true;

    void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
        _characterInside = new Character(startHealth, damageValue, energyValue, shieldPower, attackRange, attackSpeed, movementSpeed, tag);
        _playerAnimator = GetComponent<Animator>();
        _meeleAttackCoolDown = 1/attackSpeed;
        //gunScript = gun.GetComponent<Weapon>();
        gunScript.OnEquip(damageValue, attackSpeed);

    }



    private void Update()
    {
        if (Time.timeScale > 0)
        {
            AimOnMouse();

                
            if (Input.GetButton("Fire1"))
            {
                gameManager.PlayerTryWasteScrap("red", 1);
                gunScript.Shoot(bulletPrefab);
                _nextAttackTime = Time.time + _attackCoolDown;
            }
            else if (Input.GetButton("Fire2") && Time.time > _nextAttackTime && Time.time > _meeleAttackCoolDown)
            {
                MeeleAttack();
                _nextMeeleAttackTime = Time.time + 1 / _meeleAttackCoolDown - _attackCoolDown;
                _nextAttackTime = Time.time + _attackCoolDown;
            }



        }

    }

    private void FixedUpdate()
    {
        if (Time.timeScale > 0)
        {
            Move();
        }
    }

    private void Move()
    {
        var currentMoveVector = CalculateMovementVector();
        if (_movementAxes.magnitude > 0)
        {

            _playerRigidBody.MovePosition(currentMoveVector);
            _playerRigidBody.velocity = Vector3.zero;
        }

    }


    private Vector3 CalculateMovementVector()
    {
        _movementAxes.x = Input.GetAxisRaw("Horizontal");
        _movementAxes.z = Input.GetAxisRaw("Vertical");
        return transform.position + _movementAxes * movementSpeed * Time.fixedDeltaTime;
    }


    private void AimOnMouse()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            var direction = position - transform.position;
            direction.y = 0;
            transform.forward = direction;
            Debug.DrawLine(transform.position, direction);
        }

    }



    private (bool succsess, Vector3 position) GetMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            return (succsess: true, position: hitInfo.point);
        }
        else
        {
            return (succsess: false, position: Vector3.zero);
        }
    }

    public void Attack(GameObject target)
    {
        var targetType = target.GetComponent<IDamageAble>();
        if (targetType == null) return;
        targetType.TakeDamage(_characterInside.statsOut["damage"].Value, tag, knockback: true);
    }

    private void MeeleAttack()
    {
        print("MEELEE ATTACK!!!" + attackRange);
        
        Collider[] hitEnemies = Physics.OverlapSphere(pointOfAttack.position, attackRange, LayerMask.GetMask("Enemy"));
        foreach(var enemy in hitEnemies)
        {
            print(enemy.name);
            Attack(enemy.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        // Set the color of Gizmos to green
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(pointOfAttack.position, attackRange);
    }

    public void TakeDamage(float damageAmount, string damageFrom, bool knockback = false)
    {
        if (alive)
        {
            _playerAnimator.SetTrigger("Hitted");
            _characterInside.TakeDamage(damageAmount, damageFrom);
            DiedByDamage();
        }

    }

    public void TakeHeal(float healAmount)
    {
        _characterInside.TakeHeal(healAmount);
    }

    public bool DiedByDamage()
    {
        if (_characterInside.DiedByDamage())
        {
            if (deathEffect)
            {
                GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
                SpriteRenderer effectSprite = effect.GetComponent<SpriteRenderer>();
                Destroy(effect, 3f);
            }
            alive = false;
            playerSpawner.StartRespawn();
        }

        return false;

    }

    public float CheckStats(string stat)
    {
        return _characterInside.statsOut["health"].Value;
    }

    public float[] ShowCurrentStatus()
    {
        return new float[] {
            _characterInside.health/_characterInside.statsOut["health"].Value,
            _characterInside.energy/_characterInside.statsOut["energy"].Value };
    }

    private void ChangingStatsInTime()
    {

        if (_valuesRegenTimer < _valuesRegenCooldown)
        {
            _valuesRegenTimer += Time.deltaTime;
        }
        else
        {
            _characterInside.ChangingStatsInTime();
            _valuesRegenTimer = 0;
        }
    }

    public void EquipItem(Item item, float seconds = 0)
    {
        if (seconds > 0)
        {
            StartCoroutine(EquipItemForSeconds(item, seconds));
        }

        // можно добавить вариант с одеванием предмета до снятия
    }

    private IEnumerator EquipItemForSeconds(Item item, float seconds)
    {
        var curStats = _characterInside.statsOut;
        item.Equip(_characterInside);
        yield return new WaitForSeconds(seconds);
        item.Unequip(_characterInside);
    }
    public void UnequipProduct(Item item)
    {
        item.Unequip(_characterInside);
    }
    public void EquipProduct(Item item)
    {
        item.Equip(_characterInside);
    }

}
