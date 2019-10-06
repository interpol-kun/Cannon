using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed;
    //[SerializeField]
    //private Vector2 direction;
    private int damage;

    public void Shoot(Vector2 dir, int dmg)
    {
       damage = dmg;
       GetComponent<Rigidbody2D>().velocity = dir * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Target is hitted");
            collision.GetComponent<IEnemy>().TakeDamage(damage);
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Border"))
        {
            Debug.Log("Out of range");
            gameObject.SetActive(false);
        }
    }
}
