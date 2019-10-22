﻿using UnityEngine;
using UnityEngine.UI;
public class CannonController : MonoBehaviour
{
    [Tooltip("Toggle for the correct input values (mobile uses touchscreen)")]
    public bool Touchscreen;
    [Tooltip("Turn on/off the idle mode: ")]
    public bool Idle;
    Vector3 lookPos;

    public delegate void LevelUpAction(CannonController cannon);
    public static event LevelUpAction OnLevelUp;

    [SerializeField]
    private int health;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int damage;
    [SerializeField]
    private int experience;
    [SerializeField]
    private int experienceCap;
    [SerializeField]
    private int level;
    [SerializeField]
    private Transform gunPosition;

    [SerializeField]
    private PlayerStats data;

    [SerializeField]
    private float fireRate;
    private float nextFire;

    [SerializeField]
    private Image expImage;
    [SerializeField]
    private TMPro.TMP_Text levelText;

    //There was a maxMoney variable, but I think we don't need it
    [SerializeField]
    private int money;

    void Start()
    {
        Application.targetFrameRate = 60;
        LoadStats();
        health = maxHealth;
        nextFire = 0.0f;

        level = 1;
        experience = 0;

        //TODO: Get rid of hardcoded stuff
        expImage = GameObject.Find("exp_image").GetComponent<Image>();
        levelText = GameObject.Find("level_text").GetComponent<TMPro.TMP_Text>();

        expImage.fillAmount = (float)experience / experienceCap;
        levelText.text = level.ToString();
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


        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            //var proj = Instantiate(projectile, gunPosition.position, transform.rotation);
            Shoot();
        }

        if (Idle && Input.GetMouseButton(0) == false)
        {
            Time.timeScale = 0.2f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private void LoadStats()
    {
        damage = data.Damage;
        fireRate = data.FireRate;
        maxHealth = data.MaxHealth;
    }

    private void Shoot()
    {
        Debug.Log("Shoot!");
        GameObject st = ShotPool.shotPoolInstance.GetShot();
        st.transform.position = gunPosition.position;
        st.transform.rotation = transform.rotation;
        st.SetActive(true);
        st.GetComponent<Projectile>().Shoot(transform.rotation * Vector3.up, damage);
        nextFire = Time.time + fireRate;
    }

    private void OnGUI()
    {
        GUI.TextField(new Rect(10, 10, 200, 20), lookPos.ToString(), 25);
    }

    public void KillConfirmed(int money, int exp, EnemyController.EnemyType enemyType)
    {
        //TODO: Code to count how many enemies of different types player killed
        AddExperience(exp);
        AddMoney(money);
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

    public void AddExperience(int exp)
    {
        experience = Mathf.Clamp(experience + exp, 0, experienceCap);
        if(experience == experienceCap)
        {
            LevelUp();
        }
        expImage.fillAmount = (float)experience / experienceCap;
    }

    private void LevelUp()
    {
        level++;
        experience = 0;
        experienceCap += (int)(experienceCap * 0.4f);
        expImage.fillAmount = (float)experience / experienceCap;
        levelText.text = level.ToString();
        //TODO: Fix that shit
        //OnLevelUp(this.gameObject.GetComponent<CannonController>());
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
