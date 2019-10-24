using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public LayerMask botsMask;
    public float damage = 10f;
    public float maxLifetime = 2f;

    [HideInInspector] public BotController firedBy;

    private void Start()
    {
        Destroy(gameObject, maxLifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered");

        BotController bot = other.GetComponent<BotController>();
        if (bot == null || bot.Equals(firedBy)) return;
        
        bot.TakeDamage(damage);
        Destroy(gameObject);
    }
}