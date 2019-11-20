using UnityEngine;

public class Bullet : MonoBehaviour
{

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
        var bodyType = other.GetComponent<BodyTypeController>();
        if (bodyType != null && bodyType.bot != null)
        {
            if (!isRebound && bodyType.bot.Equals(firedBy)) return;

            bodyType.bot.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        var armor = other.GetComponent<ArmorController>();
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