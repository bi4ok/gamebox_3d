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
    private GameObject bulletPrefab;
    [SerializeField]
    private Weapon gunScript;
    [SerializeField] 
    private Weapon[] weapons;

    [SerializeField]
    private GameHandler gameManager;
    [SerializeField]
    private AudioClip blasterSound;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private PlayerSpawner playerSpawner;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private LayerMask groundMask;

    private Character _characterInside;
    private Dictionary<string, Item> _inventory;
    private Rigidbody _playerRigidBody;
    private Vector3 _movementAxes;

    private float _valuesRegenCooldown = 5f;
    private float _valuesRegenTimer = 0f;
    private float _meeleAttackCoolDown;
    private float _nextMeeleAttackTime = 0f;

    private float _nextAttackTime = 0f;
    private float _attackCoolDown = 0.1f;
    private int _currentWeaponIndex = 0;

    public bool alive = true;

    void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
        _characterInside = new Character(startHealth, damageValue, energyValue, shieldPower, attackRange, attackSpeed, movementSpeed, tag);
        _meeleAttackCoolDown = 1 / attackSpeed;
        gunScript.OnEquip(damageValue, attackSpeed, gameObject);
        playerAnimator.SetFloat("MovementSpeed", movementSpeed / 10);
        playerAnimator.SetFloat("AttackSpeed", attackSpeed);

        _inventory = new Dictionary<string, Item>() { 
            {"�����", null },
            {"������", null },
            {"�����", null },
            {"��������", null },
            {"����", null}
        };

    }



    private void Update()
    {
        if (Time.timeScale > 0)
        {
            AimOnMouse();


            if (Input.GetButton("Fire1"))
            {
                
                playerAnimator.SetTrigger("Shoot");
                gunScript.Shoot(bulletPrefab);
                
                _nextAttackTime = Time.time + _attackCoolDown;
            }
            else if (Input.GetButton("Fire2") && Time.time > _nextAttackTime && Time.time > _nextMeeleAttackTime)
            {
                MeeleAttack();
                playerAnimator.SetTrigger("Hit");
                _nextMeeleAttackTime = Time.time + _meeleAttackCoolDown - _attackCoolDown;
                _nextAttackTime = Time.time + _attackCoolDown;
            }

            if (Input.mouseScrollDelta.y != 0)
            {
                int newWeaponIndex = (_currentWeaponIndex + (int)Input.mouseScrollDelta.y) % weapons.Length;
                newWeaponIndex = newWeaponIndex < 0 ? weapons.Length - 1 : newWeaponIndex;
                ChangeWeapon(_currentWeaponIndex, newWeaponIndex);
                _currentWeaponIndex = newWeaponIndex;
            }



        }

    }

    private void ChangeWeapon(int from, int to)
    {
        //weapons[from].SetActive(false);
        //weapons[to].SetActive(true);
        gunScript.UnEquip();
        gunScript = weapons[to];
        gunScript.OnEquip(damageValue, attackSpeed, gameObject);
        print(gunScript.name);
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
        playerAnimator.SetFloat("CurrentSpeed", _movementAxes.magnitude);
        playerAnimator.SetFloat("Horizontal", _movementAxes.x * transform.right.normalized.x);
        playerAnimator.SetFloat("Vertical", _movementAxes.z * transform.forward.normalized.z);
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

        Collider[] hitEnemies = Physics.OverlapSphere(pointOfAttack.position, attackRange, LayerMask.GetMask("Enemy"));
        foreach (var enemy in hitEnemies)
        {
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
        return _characterInside.statsOut[stat].Value;
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

        // ����� �������� ������� � ��������� �������� �� ������
    }

    private IEnumerator EquipItemForSeconds(Item item, float seconds)
    {
        var curStats = _characterInside.statsOut;
        item.Equip(_characterInside);
        yield return new WaitForSeconds(seconds);
        item.Unequip(_characterInside);
    }
    public void UnequipProduct(Item item, string type)
    {

        item.Unequip(_characterInside);
    }
    public void EquipProduct(Item item, string type)
    {
        Item currentItem;
        if (_inventory.TryGetValue(type, out currentItem))
        {
            if(_inventory[type] != null)
            {
                print("������� ������ - " + _inventory[type]);
                _inventory[type].Unequip(_characterInside);
            }
            print("�������� ����� - " + item);
            print(CheckStats("movementSpeed"));
            item.Equip(_characterInside);

            print(CheckStats("movementSpeed"));
            _inventory[type] = item;

        }
        
    }

}
