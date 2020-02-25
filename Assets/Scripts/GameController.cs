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

    // Start is called before the first frame update
    void Start()
    {
        BeginGame();
        EnemyController.OnDeath += RemoveEnemy;
    }

    void BeginGame()
    {
        //If there is no wave delay, the kill delay is applied
        if (scenario.waveDelay == 0)
        {
            StartCoroutine(KillSequence());
        }
        else
        {
            StartCoroutine(WaitSequence());
        }
        
    }

    IEnumerator KillSequence()
    {
        var smallWait = new WaitForSeconds(.3f);
        var defaultWait = new WaitForSeconds(defaultWaveDelay);

        for (int i = 0; i < scenario.WaveCount; i++)
        {
            currentWave = i;
            if (currentEnemies.Count == 0)
            {
                onWaveEnd?.Invoke(currentWave);
                //Show the wave number transition (synced with defaultWait)
                WaveText(i, defaultWaveDelay);
                //Wait for transition to end
                yield return defaultWait;
                Spawn(i);
            }
            else
            {
                i--;
                //Some small optimization to run coroutines 3 times a sec instead of every frame
                yield return smallWait;
            }
            Debug.Log(i);
        }
    }

    IEnumerator WaitSequence()
    {
        var defaultWait = new WaitForSeconds(defaultWaveDelay);

        for (int i = 0; i < scenario.WaveCount; i++)
        {
            currentWave = i;

            WaveText(i, defaultWaveDelay);
            yield return defaultWait;
            Spawn(i);
            yield return new WaitForSeconds(scenario.waveDelay);
        }      
    }

    void Spawn(int waveNumber)
    {
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
    }

    void WaveText(int waveNumber, float delay)
    {
        UIManager.instance.WaveNumber(waveNumber + 1, delay);
        text.text = "~" + (waveNumber + 1);
    }
    void RemoveEnemy(GameObject g)
    {
        currentEnemies.Remove(g);
    }

    public Vector3 GlobalShadowOffset()
    {
        return globalShadowOffset;
    }
}
