using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class PlayerController3D : MonoBehaviour
//{
//    [SerializeField]
//    private float startHealth;
//    [SerializeField]
//    private float damageValue;
//    [SerializeField]
//    private float energyValue;
//    [SerializeField, Range(0, 1)]
//    private float shieldPower;
//    [SerializeField]
//    private float movementSpeed;
//    [SerializeField]
//    private float attackRange = 0.5f;
//    [SerializeField]
//    private float attackSpeed = 1f;

//    [SerializeField]
//    private Transform pointOfAttack;
//    [SerializeField]
//    private GameObject gun;
//    [SerializeField]
//    private GameObject bulletPrefab;

//    [SerializeField]
//    private GameObject gameManager;
//    [SerializeField]
//    private AudioClip blasterSound;
//    [SerializeField]
//    private AudioSource audioSource;
//    [SerializeField]
//    private PlayerSpawner playerSpawner;
//    [SerializeField]
//    private GameObject deathEffect;

//    [SerializeField]
//    private LayerMask groundMask;

//    private Character _characterInside;
//    private Rigidbody _playerRigidBody;
//    private Vector3 _movementAxes;

//    private float _angleOfView;



//    void Awake()
//    {
//        _playerRigidBody = GetComponent<Rigidbody>();
//        _characterInside = new Character(startHealth, damageValue, energyValue, shieldPower, attackRange, attackSpeed, movementSpeed, tag);


//    }



//    private void Update()
//    {
//        if (Time.timeScale > 0)
//        {
//            AimOnMouse();

//            if (Input.GetButtonDown("Fire1"))
//            {
//                Shoot();
//            }
//        }

//    }

//    private void FixedUpdate()
//    {
//        if (Time.timeScale > 0)
//        {
//            Move();
//        }
//    }

//    private void Move()
//    {
//        var currentMoveVector = CalculateMovementVector();
//        if (_movementAxes.magnitude > 0)
//        {

//            _playerRigidBody.MovePosition(currentMoveVector);
//            _playerRigidBody.velocity = Vector3.zero;
//        }

//    }


//    private Vector3 CalculateMovementVector()
//    {
//        _movementAxes.x = Input.GetAxisRaw("Horizontal");
//        _movementAxes.z = Input.GetAxisRaw("Vertical");
//        return _playerRigidBody.position + _movementAxes * movementSpeed * Time.fixedDeltaTime;
//    }


//    private void AimOnMouse()
//    {
//        var (success, position) = GetMousePosition();
//        if (success)
//        {
//            var direction = position - transform.position;
//            direction.y = 0;
//            transform.forward = direction;
//            Debug.DrawLine(transform.position, direction);
//        }
        
//    }

//    private void Shoot()
//    {
//        GameObject bullet = Instantiate(bulletPrefab, pointOfAttack.position, pointOfAttack.rotation);
//        Rigidbody bulletBody = bullet.GetComponent<Rigidbody>();
//        Bullet bulletInside = bullet.GetComponent<Bullet>();
//        bulletInside.damage = _characterInside.statsOut["damage"].Value;
//        bulletBody.AddForce(pointOfAttack.forward * _characterInside.statsOut["attackSpeed"].Value, ForceMode.Impulse);
//        audioSource.PlayOneShot(blasterSound, 0.1f);
//    }

//    private (bool succsess, Vector3 position) GetMousePosition()
//    {
//        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
//        {
//            return (succsess: true, position: hitInfo.point);
//        }
//        else
//        {
//            return (succsess: false, position: Vector3.zero);
//        }
//    }

//}

[RequireComponent(typeof(Rigidbody))]
public class PlayerController3dt : MonoBehaviour, IDamageAble, IDamageDealer<GameObject>
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
    private SpriteRenderer shieldSprite;
    [SerializeField]
    private Transform pointOfAttack;
    [SerializeField]
    private GameObject gun;
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private GameObject lighter;
    [SerializeField]
    private GameObject aroundLight;

    [SerializeField]
    private GameObject gameManager;
    [SerializeField]
    private AudioClip blasterSound;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private PlayerSpawner playerSpawner;
    [SerializeField]
    private GameObject deathEffect;

    private GameHandler gameHandler;

    private float _valuesRegenCooldown = 5f;
    private float _valuesRegenTimer = 0f;
    private float _angleOfView;
    private bool _weaponEquipped = true;
    public bool alive = true;


    private Character _characterInside;
    private Rigidbody _playerRigidBody;
    private Vector3 _movementAxes;
    private SpriteRenderer _playerSprite;
    private Animator _playerAnimator;

    void Awake()
    {
        _characterInside = new Character(startHealth, damageValue, energyValue, shieldPower, attackRange, attackSpeed, movementSpeed, tag);
        _playerRigidBody = GetComponent<Rigidbody>();
        _playerAnimator = GetComponent<Animator>();
        _playerSprite = GetComponent<SpriteRenderer>();
        aroundLight.transform.position = transform.position;
        gameHandler = gameManager.GetComponent<GameHandler>();
        RefreshShieldColor();


        Item weapon = new Item(
                        damagePlus: 10, damagePercent: 10, attackSpeedPercent: 20,
                        healthPlus: 50, healthPercent: 10, energyPlus: 20
                        );
        //StartCoroutine(EquipItemForSeconds(weapon, 5f));

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


    private void Update()
    {
        if (Time.timeScale > 0)
        {
            AimOnMouse();
            UpdateLight();
            ChangingStatsInTime();
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
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

    private void UpdateLight()
    {
        aroundLight.transform.position = transform.position;
        lighter.transform.position = transform.position;
        var nAngle = Quaternion.Euler(lighter.transform.rotation.eulerAngles.x, lighter.transform.rotation.eulerAngles.y, _angleOfView - 90f);
        lighter.transform.rotation = nAngle;
    }


    private void RefreshShieldColor()
    {

        if (_characterInside.statsOut["energyPower"].Value >= 1)
        {
            var col = new Color(255f, 0f, 0f, 1);
            shieldSprite.color = col;
        }
        else
        {
            float currentPercentEnergy = _characterInside.energy / _characterInside.statsOut["energy"].Value;
            var col = new Color(255f, 255f, 255f, currentPercentEnergy);
            shieldSprite.color = col;
        }


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
        RefreshShieldColor();
    }

    private void Move()
    {
        var currentMoveVector = CalculateMovementVector();
        if (_movementAxes.magnitude > 0)
        {
            _playerAnimator.SetFloat("Speed", _characterInside.statsOut["movementSpeed"].Value);
            _playerAnimator.speed = _characterInside.statsOut["movementSpeed"].Value / 2;
            _playerRigidBody.MovePosition(currentMoveVector);
            _playerRigidBody.velocity = Vector3.zero;
        }
        else
        {
            _playerAnimator.SetFloat("Speed", 0);
        }
    }

    private void FlipSprite(float angle)
    {
        var newAngle = new Quaternion();
        if (90 >= angle && angle >= -90)
        {
            newAngle.Set(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w);
            transform.rotation = newAngle;
        }
        else
        {
            newAngle.Set(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
            transform.rotation = newAngle;
        }

        if (_weaponEquipped)
        {
            if (Mathf.Abs(angle) < 90)
            {
                if (angle < 0)
                {
                    angle = -180 - angle;
                }
                else
                {
                    angle = 180 - angle;
                }
            }

            var nAngle = Quaternion.Euler(gun.transform.rotation.eulerAngles.x, gun.transform.rotation.eulerAngles.y, angle + 180f);
            gun.transform.rotation = nAngle;
        }

    }

    private Vector3 CalculateMovementVector()
    {
        _movementAxes.x = Input.GetAxisRaw("Horizontal");
        _movementAxes.y = Input.GetAxisRaw("Vertical");
        return _playerRigidBody.position + _movementAxes * _characterInside.statsOut["movementSpeed"].Value * Time.fixedDeltaTime;
    }

    private void AimOnMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 deltaMouse = new Vector3(mousePosition.x, mousePosition.y) - _playerRigidBody.position;
        _angleOfView = Mathf.Atan2(deltaMouse.y, deltaMouse.x) * Mathf.Rad2Deg;
        FlipSprite(_angleOfView);
        Debug.DrawLine(_playerRigidBody.position, mousePosition);
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, pointOfAttack.position, pointOfAttack.rotation);
        Rigidbody bulletBody = bullet.GetComponent<Rigidbody>();
        Bullet bulletInside = bullet.GetComponent<Bullet>();
        bulletInside.damage = _characterInside.statsOut["damage"].Value;
        bulletBody.AddForce(-pointOfAttack.right * _characterInside.statsOut["attackSpeed"].Value, ForceMode.Impulse);
        audioSource.PlayOneShot(blasterSound, 0.1f);
    }

    public void Attack(GameObject target)
    {
        var targetType = target.GetComponent<IDamageAble>();
        if (targetType == null) return;
        targetType.TakeDamage(_characterInside.statsOut["damage"].Value, tag);
    }

    public void TakeDamage(float damageAmount, string damageFrom, bool knockback=false)
    {
        if (alive)
        {
            _playerAnimator.SetTrigger("Hitted");
            damageAmount = ShieldThatDamage(damageAmount);
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
        {/*
            _playerAnimator.SetTrigger("Death");
            Destroy(gameObject, 5f);
            Time.timeScale = 0;
            gameHandler.EndGame();
            return true;
            */
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            SpriteRenderer effectSprite = effect.GetComponent<SpriteRenderer>();
            effectSprite.flipX = _playerSprite.flipX;
            Destroy(effect, 3f);
            alive = false;
            playerSpawner.StartRespawn();
        }

        return false;

    }

    public float ShieldThatDamage(float damageAmount)
    {
        RefreshShieldColor();
        damageAmount = _characterInside.ShieldThatDamage(damageAmount);
        return damageAmount;
    }
    public float CheckStats(string stat)
    {
        return _characterInside.statsOut["health"].Value;
    }
}
