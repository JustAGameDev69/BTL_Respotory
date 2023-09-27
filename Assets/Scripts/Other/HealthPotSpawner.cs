using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotSpawner : MonoBehaviour
{
    public delegate void HealthPotCollectedHandle();
    public static event HealthPotCollectedHandle HealthPotCollected;

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
        if (collision.gameObject.CompareTag("Player") && HealthPotCollected != null)
        {
            HealthPotCollected();
            Destroy(gameObject);
        }
    }
}
