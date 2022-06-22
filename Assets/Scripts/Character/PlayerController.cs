using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private Weapon gunScript;
    [SerializeField] 
    private Weapon[] weapons;

    [SerializeField]
    private GameHandler gameManager;
    [SerializeField]
    private AudioClip soundOfDamage;
    [SerializeField]
    private AudioClip guitarSolo;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private PlayerSpawner playerSpawner;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private CastleController castle;

    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private LayerMask nonInteractMask;
    [SerializeField]
    private LayerMask enemyMask;
    [SerializeField]
    private AudioManager audioManager;
    
   

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
    private float _timeToRespawn;
    private bool thorns = false;
    private bool scrapBonus = false;

    void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
        _characterInside = new Character(startHealth, damageValue, energyValue, shieldPower, attackRange, attackSpeed, movementSpeed, tag);
        _meeleAttackCoolDown = 1 / attackSpeed;
        gunScript.OnEquip(damageValue, attackSpeed, gameObject, gunScript.name);
        playerAnimator.SetFloat("MovementSpeed", movementSpeed / 10);
        playerAnimator.SetFloat("AttackSpeed", attackSpeed);
        _timeToRespawn = baseTimeToRespawn;
        _inventory = new Dictionary<string, Item>() { 
            {"шапка", null },
            {"сапоги", null },
            {"земля", null },
            {"кольчуга", null },
            {"гиря", null},
            {"бонус", null},
            {"шипы", null},
            {"балалайка", null},
            {"брызги", null},
            {"пробитие", null},
            {"база", null},
            {"пламя", null},

        };


    }



    private void Update()
    {
        if (Time.timeScale > 0)
        {
            bool aimSucsess = AimOnMouse();


            if (Input.GetButton("Fire1"))
            {
                if (aimSucsess)
                {
                    playerAnimator.SetTrigger("Shoot");
                    gunScript.Shoot();
                }
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
        gunScript.OnEquip(damageValue, attackSpeed, gameObject, gunScript.name);
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
        //var currentMoveVector = CalculateMovementVector();
        //if (_movementAxes.magnitude > 0)
        //{

        //    _playerRigidBody.MovePosition(currentMoveVector);
        //    _playerRigidBody.velocity = Vector3.zero;
        //}

        _movementAxes.x = Input.GetAxisRaw("Horizontal");
        _movementAxes.z = Input.GetAxisRaw("Vertical");
        playerAnimator.SetFloat("CurrentSpeed", _movementAxes.magnitude);
        playerAnimator.SetFloat("Horizontal", _movementAxes.x * transform.right.normalized.x);
        playerAnimator.SetFloat("Vertical", _movementAxes.z * transform.forward.normalized.z);
        _playerRigidBody.velocity = new Vector3(_movementAxes.x, _playerRigidBody.velocity.y, _movementAxes.z)* _characterInside.statsOut["movementSpeed"].Value;

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

    private bool AimOnMouse()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            var direction = position - transform.position;
            direction.y = 0;
            transform.forward = direction;
            Debug.DrawRay(transform.position, direction);
            return true;
        }
        return false;

    }

    private (bool succsess, Vector3 position) GetMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfoNull, Mathf.Infinity, nonInteractMask) && !gameManager.gameStateFight)
        {
            return (succsess: false, position: Vector3.zero);
        }
        else if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask) && !EventSystem.current.IsPointerOverGameObject())
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
        

        Collider[] hitEnemies = Physics.OverlapSphere(pointOfAttack.position, attackRange, enemyMask);
        foreach (var enemy in hitEnemies)
        {
           
            Attack(enemy.gameObject);

        }
        if(hitEnemies.Length == 0f)
        {
            audioManager.PlaySounds("Promah" + " " + Random.Range(1, 4).ToString());
        }
        else
        {
            audioManager.PlaySounds("Bonk" + " " + Random.Range(1, 4).ToString());
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
            audioManager.PlaySounds("Ded uron");
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
            audioManager.PlaySounds("Ded pogib");
            alive = false;
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

        // можно добавить вариант с одеванием предмета до снятия
    }

    private IEnumerator EquipItemForSeconds(Item item, float seconds)
    {
        var curStats = _characterInside.statsOut;
        item.Equip(_characterInside);
        yield return new WaitForSeconds(seconds);
        item.Unequip(_characterInside);
    }
    public void UnequipProduct(Item item, string type, int level = 0)
    {
        item.Unequip(_characterInside);
        ItemEffeectApply(type, level, false);
    }
    public void EquipProduct(Item item, string type, int level = 0)
    {
        Item currentItem;
        if (_inventory.TryGetValue(type, out currentItem))
        {
            if(_inventory[type] != null)
            {
                print("Снимаем старье - " + _inventory[type]);
                _inventory[type].Unequip(_characterInside);
            }
            print("Надеваем новье - " + item);
            print(CheckStats("movementSpeed"));
            item.Equip(_characterInside);
            ItemEffeectApply(type, level, true);
            print(CheckStats("movementSpeed"));
            _inventory[type] = item;


        }
        
    }
    public float CurrentHpcheck()
    {
        return _characterInside.health;
    }

    private void ItemEffeectApply(string itemName, int level, bool equip)
    {
        print(itemName + " ITEM APPLY");
        switch (itemName)
        {
            case "земля":
                if (level == 1)
                {
                    _timeToRespawn = baseTimeToRespawn / 2;
                    print("РЕСПАУН ТЕПЕРЬ в 2 РАЗА МЕНЬШЕ");
                }
                else if (level == 2)
                {
                    _timeToRespawn = baseTimeToRespawn / 4;
                    print("РЕСПАУН ТЕПЕРЬ в 4 РАЗА МЕНЬШЕ");
                }
                break;
            case "шипы":
                thorns = equip;
                print("ШИПЫ надеты - " + CheckThorns());
                break;
            case "бонус":
                scrapBonus = equip;
                break;
            case "сапоги":
                playerAnimator.SetFloat("MovementSpeed", _characterInside.statsOut["movementSpeed"].Value / 10);
                break;
            case "брызги":
                print("UPGRADE ПРОИЗОШЁЛ");
                weapons[1].UpgradeWeapon();
                break;
            case "пробитие":
                weapons[0].UpgradeWeapon();
                break;
            case "пламя":
                weapons[2].UpgradeWeapon();
                break;
            case "база":
                print("UPGRADE БАЗЫ ");
                castle.Upgrade();
                break;


        }
    }
}
