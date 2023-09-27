using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerArea : MonoBehaviour
{
    #region Public Variables
    #endregion

    #region Private Variables
    private EnemiesController enemiesController;
    #endregion

    private void Awake()
    {
        enemiesController = GetComponentInParent<EnemiesController>();      //Lấy các component từ script cha.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))             //Khi player vào trong triggerzone.
        {
            gameObject.SetActive(false);
            enemiesController.target = collision.transform;     //Lấy vị trí của player truyền cho target
            enemiesController.inRange = true;                   //Mục tiêu (player) đã trong tầm nhìn
            enemiesController.triggerZone.SetActive(true);      //Active triggerZone
        }
    }
}
