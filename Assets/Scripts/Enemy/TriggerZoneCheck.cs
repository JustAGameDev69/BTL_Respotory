using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneCheck : MonoBehaviour
{

    #region Public Variables
    #endregion

    #region Private Variables
    private EnemiesController enemiesController;
    private bool inRange;
    private Animator animator;
    #endregion

    private void Awake()
    {
        enemiesController = GetComponentInParent<EnemiesController>();
        animator = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if(inRange && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            enemiesController.Flip();       
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inRange = false;
        gameObject.SetActive(false);
        enemiesController.triggerArea.SetActive(true);
        enemiesController.inRange = false;
        enemiesController.SelectTarget();           //Đưa quái vào trạng thái đi lòng vòng (tìm player)
    }

}
