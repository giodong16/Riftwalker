using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField] PlayerMovement movement;
    // attack
    [Header("Attack Data")]
    public List<AttackData> attackList = new List<AttackData>();

    [Header("Data Item")]
    [SerializeField] private ItemData bulletData;
    [SerializeField] private ItemData bombData;
    [SerializeField] private ItemData handgunData;
    [SerializeField] private ItemData rifleData;
    [SerializeField] private ItemData knifeData;

    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private Transform spawnBombPosition;
    [SerializeField] public GameObject bombPrefab;
    [SerializeField] private Transform detectedPosition;
    [SerializeField] private Vector2 boxSizeDetect = new Vector2(4f, 2f);
    private Transform targetPos;

    [SerializeField] private Transform _slashTransform;
    [SerializeField] private float _slashRadius = 0.2f;

    [SerializeField] public GameObject bulletPrefab;

    private float damage;

    private void Awake()
    {
        foreach (var attack in attackList)
        {
            attack.lastAttackTime = -attack.cooldownTime;
        }
    }

    private void Start()
    {
        playerController = PlayerController.Instance;
    }
    private bool CanAttack(NameAttack attackName)  // kiểm tra tấn công có hồi chiêu
    {
        AttackData attack = attackList.Find(a => a.attackName == attackName);
        if (attack == null) return false;

        return Time.time >= attack.lastAttackTime + attack.cooldownTime * (1 - playerController.CurrentStats.cooldownRate);
    }

    public void Attack(NameAttack attackName, string nameTrigger)
    {

        if (!playerController.IsAttacking)
        {
            if (CanAttack(attackName))
            {

                AttackData attack = attackList.Find(a => a.attackName == attackName);

                attack.lastAttackTime = Time.time;
                float cooldownTime = attack.cooldownTime * (1 - playerController.CurrentStats.cooldownRate);

                playerController.IsAttacking = true;
                playerController._animator.SetBool("isAttacking", playerController.IsAttacking);
                playerController._animator.SetTrigger(nameTrigger);

                UIManager.Instance?.UpdateCooldown(attackName, cooldownTime);

                if (attackName == NameAttack.Firing)
                {

                    SpawnBullet();
                }
                else if (attackName == NameAttack.Slashing)
                {
                    MeleeAttack(_slashTransform, _slashRadius, 150);
                }
                else if (attackName == NameAttack.Throwing)
                {
                    ThrowingBomb();
                }
            }
            else
            {
         //       Debug.Log("Không thể attack vì cooldown chưa kết thúc.");
            }
        }
        else
        {
           // Debug.Log("Không thể attack vì đang trong trạng thái attack.");
        }
    }

    public void KickingAttack(InputAction.CallbackContext context)
    {
        if (!playerController.CanController) return;
        if (context.performed && !playerController.IsAttacking)
        {

            Attack(NameAttack.Kicking, "Kicking");
        }
    }

    public void SlashingAttack(InputAction.CallbackContext context)
    {
        if (!playerController.CanController) return;
        if (context.performed && !playerController.IsAttacking)
        {
            Attack(NameAttack.Slashing, "Slashing");
        }
    }



    public void ThrowingAttack(InputAction.CallbackContext context)
    {
        if (!playerController.CanController) return;
        int quality = ItemDataManager.Instance.GetQuality(bombData);
        if (quality <= 0)
        {
            AudioManager.Instance?.PlaySFX(AudioManager.Instance.warningClip);
            return;
        }
        if (context.performed && !playerController.IsAttacking && bombData != null)
        {
            Collider2D enemy = Physics2D.OverlapBox(detectedPosition.position, boxSizeDetect, 0,
          enemyLayer);

            if (enemy != null)
            {
                targetPos = enemy.transform;
                Attack(NameAttack.Throwing, "Throwing");
            }
        }

    }


    public void FiringAttack(InputAction.CallbackContext context)
    {
        if (!playerController.CanController) return;
        if (ItemDataManager.Instance.GetQuality(bulletData) <= 0)
        {
            AudioManager.Instance?.PlaySFX(AudioManager.Instance.warningClip);
            StatusBar statusBar = FindObjectOfType<StatusBar>();
            if (statusBar != null)
            {
                // audio
                statusBar.WarningBullets();
            }
            return;
        }
        if (context.performed && !playerController.IsAttacking)
        {
            Attack(NameAttack.Firing, "Firing");
        }
    }
    public void SpawnBullet()
    {
        if (playerController.CurrentAnimator_Muzzle != null)
        {
            playerController.CurrentAnimator_Muzzle.SetTrigger("isAttack");
        }

        AudioManager.Instance?.PlaySFX(playerController.audioSource, playerController.firingSound);

        if (playerController.CurrentWeapon == WeaponType.Rifle)
        {
            rifleData.UseItem();
            Vector3 pos = playerController.CurrentPositionSpawnBullet.position;
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.GetComponent<Bullet>().SetUp(movement.IsFacingRight, pos);
            CameraTrigger.Instance.ShakeCamera(0.5f, 0.05f);
        }
        else
        {
            handgunData.UseItem();
            CameraTrigger.Instance.ShakeCamera(0.5f, 0.05f);
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.GetComponent<Bullet>().SetUp(movement.IsFacingRight, playerController.CurrentPositionSpawnBullet.position);

        }


    }
    public void MeleeAttack(Transform attackTransform, float attackRadius, float attackDamage)
    {
        knifeData.UseItem();
        damage = playerController.GetAttackDamage();

        AudioManager.Instance?.PlaySFX(playerController.audioSource, playerController.slashingSound);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackTransform.position, attackRadius, enemyLayer);
        foreach (Collider2D hit in hitEnemies)
        {
            Vector2 hitPosition = hit.ClosestPoint(attackTransform.position);
            EnemyHeath enemyHeath = hit.GetComponent<EnemyHeath>();
            BossHeath bossHeath = hit.GetComponent<BossHeath>();
            DragonHeath dragonHeath = hit.GetComponent<DragonHeath>();
            if (enemyHeath != null)
            {
                enemyHeath.TakeDamage(damage, hitPosition);
            }
            if (bossHeath != null)
            {
                bossHeath.TakeDamage(damage, hitPosition);
            }
            if (dragonHeath != null)
            {
                dragonHeath.TakeDamage(damage, hitPosition);
            }
        }
    }

    public void ThrowingBomb()
    {
        AudioManager.Instance?.PlaySFX(playerController.audioSource, playerController.throwingSound);
        GameObject bomb = Instantiate(bombPrefab);
        bomb.GetComponent<Bomb>().SetUp(
            spawnBombPosition.position,
            targetPos.position
        );

    }

    public void EquipWeapon(InputAction.CallbackContext context)
    {
        if (!playerController.CanController) return;
        if (context.performed && !playerController.IsAttacking)
        {
            if (playerController.CurrentWeapon == WeaponType.Handgun && !rifleData.canSell)
                playerController.EquipGun(WeaponType.Rifle, playerController._positonSpawnBulletRifle, playerController.animator_muzzleRifle);
            else
            {
                playerController.EquipGun(WeaponType.Handgun, playerController._positonSpawnBulletHandgun, playerController.animator_muzzleHandgun);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_slashTransform.position, _slashRadius);
        Gizmos.color = Color.red;
        Gizmos.color = Color.green;
        Vector2 boxCenter = detectedPosition.position;
        Gizmos.DrawWireCube(boxCenter, boxSizeDetect);
    }
}
