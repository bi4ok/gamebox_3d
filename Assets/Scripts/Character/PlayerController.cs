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
    private float baseTimeToRespawn = 5f;

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
    private float _timeToRespawn;
    private bool thorns = false;
    private bool scrapBonus = false;

    public bool alive = true;

    void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
        _characterInside = new Character(startHealth, damageValue, energyValue, shieldPower, attackRange, attackSpeed, movementSpeed, tag);
        _meeleAttackCoolDown = 1 / attackSpeed;
        gunScript.OnEquip(damageValue, attackSpeed, gameObject);
        playerAnimator.SetFloat("MovementSpeed", movementSpeed / 10);
        playerAnimator.SetFloat("AttackSpeed", attackSpeed);
        _timeToRespawn = baseTimeToRespawn;
        _inventory = new Dictionary<string, Item>() { 
            {"�����", null },
            {"������", null },
            {"�����", null },
            {"��������", null },
            {"����", null},
            {"�����", null},
            {"����", null},
            {"���������", null},
            {"������", null},
            {"��������", null},

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
        return transform.position + _movementAxes * _characterInside.statsOut["movementSpeed"].Value * Time.fixedDeltaTime;
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
        print("���� ����� ������� " + _characterInside.statsOut["damage"].Value);
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
            print("������� ����� " + _timeToRespawn);
            playerSpawner.StartRespawn(_timeToRespawn);
        }

        return false;

    }

    public float CheckStats(string stat)
    {
        return _characterInside.statsOut[stat].Value;
    }

    public bool CheckThorns()
    {
        return thorns;
    }

    public bool CheckBonusScrap()
    {
        return scrapBonus;
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
    public void UnequipProduct(Item item, string type, int level=0)
    {
        item.Unequip(_characterInside);
        ItemEffeectApply(type, level, false);

    }
    public void EquipProduct(Item item, string type, int level=0)
    {
        print("������ ������� " + type);
        Item currentItem;
        if (_inventory.TryGetValue(type, out currentItem))
        {
            if(_inventory[type] != null)
            {
                print("������� ������ - " + _inventory[type]);
                _inventory[type].Unequip(_characterInside);
            }
            print("�������� ����� - " + item);
            item.Equip(_characterInside);
            ItemEffeectApply(type, level, true);
            _inventory[type] = item;

        }
        
    }

    private void ItemEffeectApply(string itemName, int level, bool equip)
    {
        print(itemName + " ITEM APPLY");
        switch (itemName)
        {
            case "�����":
                if (level == 1)
                {
                    _timeToRespawn = baseTimeToRespawn / 2;
                    print("������� ������ � 2 ���� ������");
                }
                else if (level == 2)
                {
                    _timeToRespawn = baseTimeToRespawn / 4;
                    print("������� ������ � 4 ���� ������");
                }
                break;
            case "����":
                thorns = equip;
                print("���� ������ - " + CheckThorns());
                break;
            case "�����":
                scrapBonus = equip;
                break;
            case "������":
                playerAnimator.SetFloat("MovementSpeed", _characterInside.statsOut["movementSpeed"].Value / 10);
                break;
            case "������":
                print("UPGRADE ������ب�");
                weapons[1].UpgradeWeapon();
                break;
            case "��������":
                weapons[0].UpgradeWeapon();
                break;

        }
    }

}
