using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    #region Public Variables
    public int currentHealth;
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject triggerArea;
    public GameObject triggerZone;
    public GameObject player;
    public GameObject[] objectDrops;
    #endregion

    #region Private Variables
    public int armor = 0;
    private int maxHealth;
    private Animator animator;
    private float distance;                 //Between our player and enemy
    private bool isAttack;
    private bool isCooldown;
    private float intTimer;
    private bool isShieldUp;
    #endregion

    private void Awake()
    {
        SelectTarget();
        animator = GetComponent<Animator>();
        intTimer = timer;

        if (gameObject.CompareTag("Skeleton"))
        {
            maxHealth = 150;
        }
        if (gameObject.CompareTag("Goblin"))
        {
            maxHealth = 100;
        }

    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (!isAttack)
        {
            Move();
        }

        if(!IsInsideLimit() && !inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            SelectTarget();
        }

        if (inRange)
        {
            EnemiesLogic();
        }
    }

    void EnemiesLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);
        if(distance > attackDistance)
        {
            StopAttack();
        }
        else if(distance <= attackDistance && isCooldown == false)
        {
            Attack();
        }

        if (isCooldown)
        {
            CoolDown();
            animator.SetBool("Attacking", false);
        }
    }

    private void Move()
    {
        animator.SetBool("IsRunning", true);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))      //Check for current animationt isn't attack
        {
            Vector2 targetPos = new Vector2(target.position.x, transform.position.y);         //Take player position

            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);    //Move enemy
        }
    }


    void Attack()
    {
        timer = intTimer;
        isAttack = true;
        AudioManager.Instance.PlayEnemySound("SkeletonAttack");
        if (isAttack == true)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("Attacking", true);

        }


    }


    void StopAttack()
    {
        isCooldown = false;
        isAttack = false;
        animator.SetBool("Attacking", false);
    }

    public void TakeDamage(int damage)  
    {
        currentHealth -= damage - armor;            //Biến damage lấy từ script PlayerCombat
        animator.SetTrigger("TakeDamage");          //Animation Take Damage

        if(currentHealth <= 0)
        {
            EnemiesDie();
        }

        //Skill buff của quái vật skeleton
        if(currentHealth == 50 && gameObject.CompareTag("Skeleton"))
        {
            animator.SetTrigger("Buff");
            ShieldBuff();
            isShieldUp = true;
        }
        else
        {
            isShieldUp = false;
        }
        if (isShieldUp)
        {
            animator.SetBool("IsRunning", false);
        }
    }

    void ShieldBuff()
    {
        armor += 10;
        currentHealth += 20;
    }

    void EnemiesDie()
    {
        ObjectDrop();
        animator.SetBool("IsDead", true);           //Animation Die

        GetComponent<Collider2D>().enabled = false;             //Tắt luôn collider để player có thể đi qua xác
        this.enabled = false;           //Tắt script này -> quái chết
    }

    void CoolDown()         //Tính toán thời gian dừng giữa các đòn tấn công của quái
    {
        timer -= Time.deltaTime;

        if(timer < 0 && isCooldown && isAttack)
        {
            isCooldown = false;
            timer = intTimer;           //Cài lại thời gian dừng
        }
    }

    void TriggerCooldown()      //Gọi trong frame attack
    {
        isCooldown = true;
    }

    private bool IsInsideLimit()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;     //Trả về true nếu quái đang trong vùng di chuyển
    }

    public void SelectTarget()
    {
        //Tính toán khoảng cách từ 2 điểm giới hạn tới quái
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if(distanceToLeft > distanceToRight)
        {
            target = leftLimit;                 //nếu quái đang lệch phải thì mục tiêu đi tới là limit bên trái
        }
        else
        {
            target = rightLimit;                //Ngược lại!
        }

        Flip();
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if(transform.position.x > target.position.x)        //Nếu player đang ở bên phải thì rotate để quay mặt
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }
        transform.eulerAngles = rotation;
    }

    private void ObjectDrop()             //Spawn coin when monsters die
    {
        for(int i =0; i< objectDrops.Length; i++)
        {
            float rand = Random.Range(-1f, 2f);
            Instantiate(objectDrops[i], transform.position - new Vector3(rand, 1, 0), Quaternion.identity);
        }
    }
}
