using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour, IEnemy
{
    [SerializeField] 
    private int bounty;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int maxHealth;
    private int health;
    [SerializeField]
    private float deathDelay = 0;

    private Transform target;
    private Rigidbody2D rb;

    public int Bounty { get => bounty; set => bounty = value; }
    public float Speed { get => speed; set => speed = value; }
    public int Health => health;

    void Start()
    {
        health = maxHealth;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        transform.rotation = Quaternion.LookRotation(Vector3.forward, target.position - transform.position);
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
    }

    void Update()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, target.position, speed*Time.deltaTime));
    }

    public void Death(bool isKilled, float delay)
    {
        Debug.Log(gameObject.name + " has died");
        if (isKilled)
        {
            //TODO: Maybe there is a more efficient way to do that rather that call GetComponent for every death?
            target.GetComponent<CannonController>().AddMoney(bounty);
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
        health = Mathf.Clamp(health + damage, 0, maxHealth);
        if(health == 0)
        {
            Death(true, deathDelay);
        }
    }
}
