using UnityEngine;

public class Bullet : MonoBehaviour
{

    public LayerMask botsMask;
    public float damage = 10f;
    public float maxLifetime = 2f;

    private bool isRebound = false;
    private Rigidbody2D body;

    [HideInInspector] public BotController firedBy;

    private void Start()
    {
        Destroy(gameObject, maxLifetime);
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BotController bot = other.GetComponent<BotController>();
        if (bot != null)
        {
            if (!isRebound && bot.Equals(firedBy)) return;

            bot.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        ArmorController armor = other.GetComponent<ArmorController>();
        if (armor != null && !ShouldIgnoreCollision(armor))
        {
            armor.CollideWith(this);
        }
    }

    private bool ShouldIgnoreCollision(ArmorController withArmor)
    {
        return withArmor.bot.Equals(firedBy) && !isRebound;
    }

    public void Rebound()
    {
        body.rotation += 180f;
        body.velocity *= -1;
        isRebound = true;
    }

}