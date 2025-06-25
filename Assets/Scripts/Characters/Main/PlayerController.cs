using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour

{
    public static PlayerController Instance;
    
    [SerializeField] MainCharacterData characterData;

    public StatsManager StatsManager { get; private set; }
    public Stats CurrentStats => StatsManager.GetFinalStats();
    [Header("References")]
    public Rigidbody2D _rb;
    public Animator _animator;
    [SerializeField] private List<WeaponAnimatorMapping> weaponMappings = new List<WeaponAnimatorMapping>();
    [SerializeField] private Dictionary<WeaponType, AnimatorOverrideController> _weaponAnimatorControllers;
    private WeaponType _currentWeapon;

    public Transform useEffectPosition;

    [Header("Base Stats")]
    private int maxHP = 100;

    [Header("Other Stats")]
    [Header("Movement")]
    [HideInInspector] public float currentMoveSpeed = 6.5f;
    [HideInInspector] public float currentJumpPower = 8f;
    private int currentMaxJumps = 2;
    public float climbSpeed = 6f;

    [Header("Dash")]
    public float currentDashTime = 0.2f;
    public float currentDashSpeed = 25f;
    public GameObject bloodPrefab;


    private bool canController = true;


    // climb ladder
    [HideInInspector]public bool _canClimbing;
    [HideInInspector] public bool _isClimbing;

    // attack
    [HideInInspector] private bool isAttacking = false;


    //gun
    public Transform _positonSpawnBulletHandgun;
    public Transform _positonSpawnBulletRifle;
    public Animator animator_muzzleHandgun;
    public Animator animator_muzzleRifle;
    [HideInInspector] public Animator CurrentAnimator_Muzzle;
    [HideInInspector] public Transform CurrentPositionSpawnBullet;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip hurtSound;
    public AudioClip dieSound;
    public AudioClip slidingSound;
    public AudioClip slashingSound;
    public AudioClip firingSound;
    public AudioClip throwingSound;
    public AudioClip dashSound;
    public AudioClip collectSound;
    public AudioClip usePotionSound;
    public AudioClip shieldBlock;

    public float knockbackForce = 8f;
    
    public bool IsHurting { get;  set; }
    
    public int MaxHP { get => maxHP; private set => maxHP = value; }
    public bool IsPotionActive { get; private set; }

    public Vector3 CurrentCheckPoint { get; set ; }
    private float _currentHP;
    private bool _isImmuneToDamage;

    //Coroutine
    private Coroutine shieldCoroutine;
    private Coroutine usePotionCoroutine;
    public WeaponType CurrentWeapon { get => _currentWeapon; set => _currentWeapon = value; }
    public float CurrentHP { get => _currentHP; set => _currentHP = value; }
    public bool IsImmuneToDamage { get => _isImmuneToDamage; set => _isImmuneToDamage = value; }
    public bool CanController { get => canController; set => canController = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
       

        SetUp();

        _weaponAnimatorControllers = new Dictionary<WeaponType, AnimatorOverrideController>();
        foreach (var mapping in weaponMappings)
        {
            _weaponAnimatorControllers[mapping.weaponType] = mapping.animatorOverride;
        }
        CurrentWeapon = WeaponType.Rifle;
        EquipGun(WeaponType.Handgun, _positonSpawnBulletHandgun, animator_muzzleHandgun ); 

    }

    //
    public void EquipGun(WeaponType weaponType, Transform transformSpawnBullet, Animator animatorMuzzleGun)
    {
       if(_currentWeapon == weaponType) return;
       _currentWeapon = weaponType;
        CurrentPositionSpawnBullet = transformSpawnBullet;
        CurrentAnimator_Muzzle = animatorMuzzleGun;
       _animator.runtimeAnimatorController = _weaponAnimatorControllers[weaponType];
    }

    #region SetUp stats 
    public void SetUp()
    {
        if (!characterData) return;

        CurrentCheckPoint = transform.position;
        MaxHP = characterData.maxHP;
        CurrentHP = MaxHP;
        StatsManager = new StatsManager();
        StatsManager.SetModifier(StatModifierType.Base, characterData.baseStats);

        ResetOtherStats();

}
    private void ResetStats()
    {
        if (!characterData) return;

        CurrentHP = MaxHP;
        
        isAttacking = false;
        _animator.SetBool("isAttacking", IsAttacking);
        CanController = true;
        IsHurting = false;
        IsImmuneToDamage = false;

        _animator.Play("Idle");

        ResetOtherStats();

    }

    public void TakeDamage(float damage, Vector2? hitPosition = null) { 
        if(GameController.Instance.gameState != GameState.Playing) return;

        if (IsImmuneToDamage) {
            AudioManager.Instance?.PlaySFX(audioSource, shieldBlock);
            return;
        }
        float totalDamage = damage - CurrentStats.defense; 
        if (totalDamage > 0) {
           
            IsHurting = CurrentHP > totalDamage;
            CurrentHP -= totalDamage;
            
            canController = false;
            AudioManager.Instance?.PlaySFX(audioSource, hurtSound);
            if (hitPosition.HasValue)
            {
                DisplayDamageManage.Instance.ShowDamage((int)totalDamage, (Vector2)hitPosition, Color.red);
                Vector2 hitDirection = hitPosition.Value - (Vector2)transform.position;
                Vector2 knockbackDirection = (Vector2.up + -hitDirection.normalized).normalized;
                _rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                if (bloodPrefab != null)
                {
                    Instantiate(bloodPrefab, hitPosition.Value, Quaternion.identity);
                }
            }
            else
            {
                _rb.velocity = new Vector2(0, knockbackForce);
                if (bloodPrefab != null)
                {
                    Instantiate(bloodPrefab, transform.position, Quaternion.identity);
                }
                DisplayDamageManage.Instance.ShowDamage((int)totalDamage,transform.position+ Vector3.up, Color.red);
            }
           
            if (IsHurting) {
                _animator.SetTrigger("Hurt"); 
            } 
            else
            {
                AudioManager.Instance?.PlaySFX(audioSource, dieSound);
                _animator.SetTrigger("Dying");
                CurrentHP = 0;
                LostHeart();
            }

            HPBar hpBar = FindObjectOfType<HPBar>();
            if (hpBar)
            {
                hpBar.UpdateHPBar(0.5f, false);
            }
        }
      
    }

    public void LostHeart()
    {
        
        if (GameController.Instance.currentHeart > 1)
        {   
            GameController.Instance.currentHeart--;
            // chờ tí rồi respawn 
            StartCoroutine(DelayRespawn());
            // cập nhập lại UI
            StatusBar statusBar = FindObjectOfType<StatusBar>();
            if (statusBar != null)
            {
                statusBar.UpdateHeartText();
            }
        }
        else
        {
           
            GameController.Instance.currentHeart = 0;
            StatusBar statusBar = FindObjectOfType<StatusBar>();
            if (statusBar != null)
            {
                statusBar.UpdateHeartText();
            }
            // game over
            
            GameController.Instance.Lose();
        }

        
    }

    public void Respawn()
    {     
        transform.position = CurrentCheckPoint;
        ResetStats();
        HPBar hpBar = FindObjectOfType<HPBar>();
        if (hpBar)
        {
            hpBar.UpdateHPBar(1f, false);
        }
    }
    IEnumerator DelayRespawn()
    {
        yield return new WaitForSeconds(0.5f);
        Respawn();
        
    }

    #endregion 

    private void ClampCurrentHP()
    {
        if (CurrentHP > MaxHP)
        {
            CurrentHP = MaxHP;
        }
    }
    public void UsePotion(Stats potionStats,ParticleName particleName, float duration = 3f)
    {
        if (IsPotionActive) return;
        AudioManager.Instance?.PlaySFX(audioSource,usePotionSound);
        IsPotionActive = true;
        StatsManager.SetModifier(StatModifierType.Potion, potionStats);
        PoolParticle.Instance.GetParticle(particleName, useEffectPosition.position);
        if (particleName == ParticleName.ElectroSlash)
        {
            UseElectroSlashPotion();
        }
        else if (particleName == ParticleName.Healing)
        {
            UseHPPotion();
            float t = PoolParticle.Instance.GetParticleDuration(ParticleName.Healing);
            duration = t;
        }
        else if (particleName == ParticleName.Buff)
        {
            UseBuffPotion();
        }
        else if (particleName == ParticleName.Shield) { 
            UseShieldPotion();
            float t = PoolParticle.Instance.GetParticleDuration(ParticleName.Shield);
            duration = t;
        }
        if (usePotionCoroutine != null)
            StopCoroutine(usePotionCoroutine);

        UIManager.Instance?.UpdatePotionCooldown(duration);
        usePotionCoroutine = StartCoroutine(RemovePotionAfterDuration(duration));
    }

    private IEnumerator RemovePotionAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        StatsManager.RemoveModifier(StatModifierType.Potion);
        IsPotionActive = false;
        usePotionCoroutine = null;
        ResetOtherStats();
        IsImmuneToDamage = false;
        
    }

    public void ResetOtherStats()
    {
        currentMoveSpeed = characterData.moveSpeed;
        currentJumpPower = characterData.jumpPower;
        currentMaxJumps = characterData.maxJumps;
        currentDashTime = characterData.dashTime;
        currentDashSpeed = characterData.dashSpeed;
    }
    public void UseElectroSlashPotion()
    {
        currentMoveSpeed += CurrentStats.bonusSpeed;
    }
    public void UseHPPotion()
    {
        float t = PoolParticle.Instance.GetParticleDuration(ParticleName.Healing);
        CurrentHP += CurrentStats.bonusHP;
        ClampCurrentHP();
        FindObjectOfType<HPBar>().UpdateHPBar(t, false);
    }
    public void UseShieldPotion()
    {
        IsImmuneToDamage = true;
    }    
    public void UseBuffPotion()
    {
        currentMoveSpeed += CurrentStats.bonusSpeed;
    }
    public float GetAttackDamage()
    {
        float baseDamage = CurrentStats.damage;
        bool isCritical = Random.value < CurrentStats.criticalRate;

        if (isCritical)
        {
            float criticalDamage = 1f + CurrentStats.criticalDamage;
            return baseDamage * criticalDamage;
        }

        return baseDamage;
    }

}
