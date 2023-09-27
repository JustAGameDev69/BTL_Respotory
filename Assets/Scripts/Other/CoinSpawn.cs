using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawn : MonoBehaviour
{
    public delegate void CoinCollectedHandle();
    public static event CoinCollectedHandle CoinCollected;

    private Rigidbody2D rigidBody;
    public float forceUp = 5f;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigidBody.AddForce(Vector2.up * forceUp, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && CoinCollected != null)
        {
            AudioManager.Instance.PlaySFX("PickCoin");
            CoinCollected();
            Destroy(gameObject);
        }
    }

}
