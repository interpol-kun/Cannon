using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Tooltip("Toggle for the correct input values (mobile uses touchscreen)")]
    public bool Touchscreen;
    Vector3 lookPos;

    [SerializeField]
    private int health;
    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private GameObject gunPosition;

    //There was a maxMoney variable, but I think we don't need it
    [SerializeField]
    private int money;


    void Start()
    {
        Application.targetFrameRate = 60;
        health = maxHealth;
    }

    void Update()
    {
        Vector3 pos;

        if (Touchscreen)
        {
            pos = Input.GetTouch(0).position;
        }
        else
        {
            pos = Input.mousePosition;
        }

        lookPos = Camera.main.ScreenToWorldPoint(pos);
        Quaternion rot = Quaternion.LookRotation(transform.position - lookPos, Vector3.forward);
        //transform.LookAt(lookPos, Vector3.forward);
        transform.rotation = rot;
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);

        if (Input.GetMouseButtonDown(0))
        {
            if (projectile != null)
            {
                var proj = Instantiate(projectile, transform.position, transform.rotation);
                proj.GetComponent<Projectile>().Shoot((lookPos - transform.position).normalized);
            }
        }
    }

    private void OnGUI()
    {
        GUI.TextField(new Rect(10, 10, 200, 20), lookPos.ToString(), 25);
    }

    public void AddMoney(int amount)
    {
        if((money + amount) < 0)
        {
            //TODO: Add event that will fire the "Not enough money" message on screen or make items for purchase inactive
            Debug.Log("Can't buy");
        }
        else
        {
            money += amount;
        }
    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        if(health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Debug.Log("Death");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);
            other.GetComponent<IEnemy>().Death(false, 0f);
        }
    }
}
