using UnityEngine;

public class Bullet : MonoBehaviour
{

    public LayerMask botsMask;
    public float damage = 10f;
    public float maxLifetime = 2f;

    private bool isRebound = false;

    [HideInInspector] public BotController firedBy;

    private void Start()
    {
        Destroy(gameObject, maxLifetime);
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

        ReflectiveArmorController armor = other.GetComponent<ReflectiveArmorController>();
        if (armor != null)
        {
            if (!isRebound && armor.bot.Equals(firedBy)) return;

            if (armor.CanReflect())
            {
                Rigidbody2D body = GetComponent<Rigidbody2D>();
                body.velocity = -body.velocity;
                isRebound = true;

                armor.TakeDamange(damage);
            }
        }
    }

}