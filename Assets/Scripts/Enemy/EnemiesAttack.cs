using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesAttack : MonoBehaviour
{
    #region Public Variables
    public int damage = 10;
    private Animator animator;
    public PlayerCombat playerCombat;
    #endregion

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Debug.Log("Hit");
            playerCombat.playerCurrentHealth -= damage;
            playerCombat.beAttacked = true;
        }
    }
}
