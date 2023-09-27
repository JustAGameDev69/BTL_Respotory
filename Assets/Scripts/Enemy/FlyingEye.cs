using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{

    #region Public Variables
    public int armor;
    public int currentHealth;
    public float flySpeed = 6f;
    public PlayerCombat playerCombat;
    public float timer;
    public GameObject[] coinDrops;
    #endregion

    #region Private Variables
    private int maxHealth = 75;
    private Rigidbody2D rb;
    public bool isDetected = false;
    private bool isAttack = false;
    private bool isCooldown;
    private float intTimer;
    private Animator animator;
    private Collider2D thisCollider;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject detectedZone;
    [SerializeField] private GameObject attackZone;
    #endregion



    private void Awake()
    {
        intTimer = timer;
        animator = GetComponent<Animator>();
        thisCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        armor = 5;
    }

    private void Update()
    {
        if (isDetected)
        {
            Chase();
        }
    }

    void CoolDown()         //Tính toán thời gian dừng giữa các đòn tấn công của quái
    {
        timer -= Time.deltaTime;

        if (timer < 0 && isCooldown && isAttack)
        {
            isCooldown = false;
            timer = intTimer;           //Cài lại thời gian dừng
        }
    }

    void TriggerCooldown()      //Gọi trong frame attack
    {
        isAttack = false;
        isCooldown = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        Collider2D zoneCollider = detectedZone.GetComponent<Collider2D>();
        Collider2D attackCollider = attackZone.GetComponent<Collider2D>();

        if (zoneCollider.IsTouching(other) && other.CompareTag("Player"))
        {
            isDetected = true;                                                  //If player in zone, then chase the player
        }

        if (attackCollider.IsTouching(other) && other.CompareTag("Player"))
        {
            isAttack = true;
        }
        

    }

    void Attack()                                                                   //Gọi trong frame Attack
    {
        if (isAttack)
        {
            playerCombat.playerCurrentHealth -= 15;
            playerCombat.beAttacked = true;
            timer = intTimer;
        }
    }

    void Chase()        
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;

        transform.position += direction * flySpeed * Time.deltaTime;

        if(isAttack == true)
        {
            animator.SetTrigger("Attack");
        }

        if (isCooldown)
        {
            CoolDown();
            animator.SetBool("Attack", false);
        }
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("TakeDamage");

        if(currentHealth > 0)
        {
            currentHealth -= (damage - armor);
        }
        else if (currentHealth <= 0)
        {
            Die();
        }
    }



    private void Die()
    {
        CoinDrop();
        animator.SetBool("IsDead", true);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        GetComponent<Collider2D>().isTrigger = true;
        detectedZone.SetActive(false);
        this.enabled = false;
    }

    private void CoinDrop()             //Spawn coin when monsters die
    {
        for (int i = 0; i < coinDrops.Length; i++)
        {
            float rand = Random.Range(-1f, 2f);
            Instantiate(coinDrops[i], transform.position - new Vector3(rand, 1, 0), Quaternion.identity);
        }
    }
}

