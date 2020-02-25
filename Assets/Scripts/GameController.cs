using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public delegate void OnWaveEnd(int waveNum);
    public static event OnWaveEnd onWaveEnd;

    [SerializeField]
    private Scenario scenario;
    [SerializeField]
    private Collider2D spawnArea;

    [SerializeField]
    List<GameObject> currentEnemies = new List<GameObject>();
    private int currentWave;

    [SerializeField]
    private float defaultWaveDelay = 3f;

    [SerializeField]
    private TMPro.TMP_Text text;

    [Header("Level Graphics")]
    [SerializeField]
    private Vector3 globalShadowOffset;

    private Timer timer;

    // Start is called before the first frame update
    void Start()
    {
        BeginGame();
        EnemyController.OnDeath += RemoveEnemy;
    }

    void BeginGame()
    {
        currentWave = 0;

        //If there is no wave delay, the kill delay is applied
        if (scenario.waveDelay == 0)
        {
            KillSequence();
            onWaveEnd += WaveEndForKill;
        }
        else
        {
            timer = gameObject.AddComponent<Timer>();
            Timer.onTimerExpired += NextWave;
            WaitSequence();
            //Does not subscribe for end wave event, because that sequence doesn't need that information
        } 
    }

    void KillSequence()
    {
        //Start the first wave
        WaveText(currentWave, defaultWaveDelay);
        StartCoroutine(Spawn(currentWave, defaultWaveDelay));
    }

    void WaitSequence()
    { 
        WaveText(currentWave, defaultWaveDelay);
        StartCoroutine(SetDelayedTimer(defaultWaveDelay));
        StartCoroutine(Spawn(currentWave, defaultWaveDelay));    
    }

    void WaveEndForKill(int wave)
    {
        currentWave++;
        if (currentWave < scenario.WaveCount)
        {
            WaveText(currentWave, defaultWaveDelay);
            StartCoroutine(Spawn((wave), defaultWaveDelay));
        }
    }

    void NextWave(float extraTime)
    {
        //TODO: Use extra time variable to give some bonus currency
        currentWave++;
        if (currentWave < scenario.WaveCount)
        {
            WaveText(currentWave, defaultWaveDelay);
            StartCoroutine(SetDelayedTimer(defaultWaveDelay));
            StartCoroutine(Spawn((currentWave - 1), defaultWaveDelay));
        }
    }

    IEnumerator SetDelayedTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        timer.SetTimer(scenario.waveDelay);
    }
    IEnumerator Spawn(int waveNumber, float delay)
    {
        yield return new WaitForSeconds(delay);
        var wave = scenario.Waves[waveNumber];
        for(int i = 0; i < wave.Enemies.Length; i++)
        {
            for(int j = 0; j < wave.Ratio[i]; j++)
            {
                float xPosition = UnityEngine.Random.Range(
                        spawnArea.bounds.max.x, spawnArea.bounds.min.x
                );
                float yPosition = UnityEngine.Random.Range(
                        spawnArea.bounds.max.y, spawnArea.bounds.min.y
                    );
                currentEnemies.Add(Instantiate(wave.Enemies[i], new Vector3(xPosition, yPosition, 0), Quaternion.identity));
            }
        }
        yield return null;
    }

    void WaveText(int waveNumber, float delay)
    {
        UIManager.instance.WaveNumber(waveNumber + 1, delay);
        text.text = "~" + (waveNumber + 1);
    }
    void RemoveEnemy(GameObject g)
    {
        currentEnemies.Remove(g);
        if(currentEnemies.Count == 0)
        {
            onWaveEnd?.Invoke(currentWave);
        }
    }

    public Vector3 GlobalShadowOffset()
    {
        return globalShadowOffset;
    }
}
