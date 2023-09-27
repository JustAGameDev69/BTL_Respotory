using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    #region Public Variables
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.45f;
    public LayerMask enemyLayer;
    public int attackDamage = 20;
    public float attackSpeed = 1.2f;
    public bool trapFall = false;
    public int playerCurrentHealth;         //Máu hiện tại
    [HideInInspector] public bool beAttacked;
    public IngameUIManager ingameUI;
    public int playerHealth;       //Máu tối đa
    #endregion

    #region Private Variables

    private float nextAttackTime = 0f;
    #endregion

    private void Awake()
    {
        playerHealth = 100;
        playerCurrentHealth = playerHealth;
        ingameUI.UpdatePlayerHealthBar();
        beAttacked = false;
    }

    void Start()
    {
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(1))        //Nhấn chuột trái thực hiện đòn tấn công
            {
                animator.SetTrigger("Attacking");       //Attack animation
                nextAttackTime = Time.time + 1f / attackSpeed;          //Tính toán thời gian để tấn công lần tiếp theo
            }
        }

        if(beAttacked == true)
        {
            BeingHurt();
        }

        if(trapFall)
        {
            playerCurrentHealth = 0;
            Die();
        }
    }

    void BeingHurt()
    {
        if (beAttacked)
        {
            animator.SetTrigger("BeAttacked");
            ingameUI.UpdatePlayerHealthBar();
            if (playerCurrentHealth <= 0)
            {
                Die();
            }
            beAttacked = false;
        }

    }

    void Die()
    {
        AudioManager.Instance.PlaySFX("PlayerDie");
        animator.SetTrigger("Death");
        this.enabled = false;
    }

    public void CallDeathPanel()
    {
        ingameUI.DeathPanel();
    }

    void Attack()
    {
        AudioManager.Instance.PlaySFX("PlayerAttack");
        // Kiểm tra mọi thứ trong tầm đánh xem có kẻ địch nào không
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            ApplyDamage(enemy);
        }
    }

    void ApplyDamage(Collider2D target)
    {
        EnemiesController controller = target.GetComponent<EnemiesController>();
        FlyingEye flyingEye = target.GetComponent<FlyingEye>();

        if (controller != null)
        {
            controller.TakeDamage(attackDamage);
        }
        else if(controller == null)
        {
            flyingEye.TakeDamage(attackDamage);
        }
    }


    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
