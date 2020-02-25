using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour, IEnemy
{
    public delegate void DeathAction(GameObject g);
    public static event DeathAction OnDeath;
    public enum EnemyType
    {
        Default,
        Tank,
        Fast
    }

    [SerializeField]
    private EnemyType enemyType;
    [SerializeField] 
    private int bounty;
    [SerializeField]
    private int expBounty;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int health;
    [SerializeField]
    private float deathDelay = 0;

    [SerializeField]
    private GameObject deathEffect = default;

    private Transform target;
    private Rigidbody2D rb;

    [SerializeField]
    private Color color;

    [Header("Graphics")]
    [SerializeField]
    private bool dropShadow;
    private GameObject shadow;
    [SerializeField]
    private float shadowAlpha;

    public int Bounty { get => bounty; set => bounty = value; }
    public float Speed { get => speed; set => speed = value; }

    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int Health { get => health; set => health = value; }

    void Start()
    {
        health = maxHealth;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        //transform.rotation = Quaternion.LookRotation(Vector3.forward, target.position - transform.position);
        //transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        transform.rotation.SetLookRotation(Vector2.down);

        if (dropShadow)
        {
            CreateShadow();
        }
    }

    void Update()
    {
        //rb.MovePosition(Vector2.MoveTowards(transform.position, target.position, speed*Time.deltaTime));
        rb.MovePosition(Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, target.position.y), speed * Time.deltaTime));
    }

    public void Death(bool isKilled, float delay)
    {
        if(OnDeath != null)
            OnDeath(gameObject);

        Debug.Log(gameObject.name + " has died");
        if (isKilled)
        {
            var particle = Instantiate(deathEffect, transform.position, Quaternion.identity).GetComponent<ParticleSystem>().main;
            particle.startColor = new Color(color.r, color.g, color.b, color.a);
            //Debug.Break();
            //TODO: Maybe there is a more efficient way to do that rather that call GetComponent for every death?
            target.GetComponent<CannonController>().KillConfirmed(bounty, expBounty, enemyType);
        }
        //Micro-optimisation
        if (delay > 0) {
            Destroy(gameObject, delay);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        if(health == 0)
        {
            Death(true, deathDelay);
        }
    }

    private void CreateShadow()
    {
        shadow = new GameObject(gameObject.name + "_shadow");
        shadow.transform.SetParent(transform);
        shadow.transform.localScale = new Vector3(1f, 1f, 1f);

        var shadowSprite = shadow.AddComponent<SpriteRenderer>();
        shadowSprite.sprite = GetComponent<SpriteRenderer>().sprite;
        Color shadowColor = Color.black;
        shadowColor.a = shadowAlpha;
        shadowSprite.color = shadowColor;
        shadow.transform.position = transform.position + GameObject.FindGameObjectWithTag("Border").GetComponent<GameController>().GlobalShadowOffset();
    }
}
