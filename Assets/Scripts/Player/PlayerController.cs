using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.8f;
    public float jumpForce = 9f;

    #region Public Variables
    public Animator animator;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public PlayerCombat playerCombat;
    public IngameUIManager ingameUI;
    #endregion

    #region Private Variables
    private bool isTouchingGround;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundCheckRadius = 0.25f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        //Kiểm tra xem có đang chạm đất không và trả về true hoặc false
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Kiểm tra xem có đang ở trên mặt đất hay không

        if (isTouchingGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                AudioManager.Instance.PlaySFX("Jump");
                animator.SetBool("isJumping", true);

            }
            else
            {
                animator.SetBool("isJumping", false);
            }
        }
        
        if(playerCombat.enabled == false)
        {
            GetComponent<Collider2D>().enabled = false;
            rb.gravityScale = 0;
        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // Di chuyển nhân vật
        float moveHorizontal = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(moveHorizontal)); //Đổi animation sang Walking

        if(moveHorizontal < 0)          //Nếu player di chuyển sang trái thì flip.
        {
            spriteRenderer.flipX = true;
        }
        else if (moveHorizontal > 0)        //Di chuyển sang phải tắt flipX.
        {
            spriteRenderer.flipX = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeadlyTrap"))
        {
            playerCombat.trapFall = true;
            ingameUI.UpdatePlayerHealthBar();
        }
    }

}
