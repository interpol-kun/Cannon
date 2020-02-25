using UnityEngine;
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
    [Space(10)]
    [SerializeField]
    private Transform gun;
    [SerializeField]
    private Transform gunPosition;

    [SerializeField]
    private PlayerStats data;

    [SerializeField]
    private float fireRate;
    private float nextFire;
    [Tooltip("Speed factor of the projectile. Multiplies the basic projectile speed")]
    [SerializeField] 
    private float speedFactor;

    [SerializeField]
    private Image expImage;
    [SerializeField]
    private TMPro.TMP_Text levelText;
    [SerializeField]
    private TMPro.TMP_Text moneyText;

    //There was a maxMoney variable, but I think we don't need it
    [SerializeField]
    private int money;

    UIManager uiManager;

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
        moneyText = GameObject.Find("money_text").GetComponent<TMPro.TMP_Text>();
        uiManager = GameObject.Find("EventSystem").GetComponent<UIManager>();

        expImage.fillAmount = (float)experience / experienceCap;
        levelText.text = level.ToString();

        //Hack to refresh the UI and set the value to Text
        AddMoney(0);
    }

    void Update()
    {
        Vector3 pos;

        //UI
        FillExperienceBar();

        if (Touchscreen && Input.touchCount > 0)
        {
            pos = Input.GetTouch(0).position;
        }
        else
        {
            pos = Input.mousePosition;
        }

        if (!uiManager.isPaused)
        {
            if (Input.GetMouseButton(0) && Time.time > nextFire)
            {
                Shoot();
            }
            lookPos = Camera.main.ScreenToWorldPoint(pos);
            Quaternion rot = Quaternion.LookRotation(gun.position - lookPos, Vector3.forward);
            gun.rotation = rot;
            gun.eulerAngles = new Vector3(0, 0, gun.eulerAngles.z);

            if (!Touchscreen && Idle && !Input.GetMouseButton(0))
            {
                Time.timeScale = 0.3f;
            }
            else if (Touchscreen && Idle && Input.touchCount == 0)
            {
                Time.timeScale = 0.2f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
        else
        {
            Time.timeScale = 0f;
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
        GameObject st = ShotPool.shotPoolInstance.GetShot();
        st.transform.position = gunPosition.position;
        st.transform.rotation = gun.rotation;
        st.SetActive(true);
        st.GetComponent<Projectile>().Shoot(gun.rotation * Vector2.up, damage, speedFactor);
        nextFire = Time.time + fireRate;
    }

    //private void OnGUI()
    //{
    //    GUI.TextField(new Rect(10, 10, 200, 20), lookPos.ToString(), 25);
    //}

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

        if (moneyText != null)
        {
            moneyText.text = "$" + money;
        }
    }

    public void AddExperience(int exp)
    {
        experience = Mathf.Clamp(experience + exp, 0, experienceCap);
        if(experience == experienceCap)
        {
            LevelUp();
        }
    }
    private void FillExperienceBar()
    {
        expImage.fillAmount = Mathf.MoveTowards(expImage.fillAmount, (float)experience / experienceCap, Time.deltaTime);
    }

    private void LevelUp()
    {
        level++;
        experience = 0;
        experienceCap += (int)(experienceCap * 0.4f);
        expImage.fillAmount = (float)experience / experienceCap;
        levelText.text = level.ToString();

        if(OnLevelUp != null)
            OnLevelUp(GetComponent<CannonController>());
    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        CameraShake.Shake(0.5f, 0.1f);
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
