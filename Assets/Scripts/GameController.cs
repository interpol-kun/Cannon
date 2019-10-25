using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private Scenario scenario;
    [SerializeField]
    private Collider2D spawnArea;

    [SerializeField]
    List<GameObject> currentEnemies = new List<GameObject>();

    [SerializeField]
    private float defaultWaveDelay = 3f;

    [SerializeField]
    private TMPro.TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BeginGame());
        EnemyController.OnDeath += RemoveEnemy;
    }

    IEnumerator BeginGame()
    {
        Debug.Log("Begin game");
        for (int i = 0; i < scenario.WaveCount; i++)
        {
            if(scenario.waveDelay == 0)
            {
                Debug.Log(i);
                if(currentEnemies.Count == 0)
                {
                    WaveText(i, defaultWaveDelay);
                    yield return new WaitForSeconds(defaultWaveDelay);
                    Spawn(i);
                }
                else
                {
                    i--;
                    //Some small optimization to run coroutines 3 times a sec instead of every frame
                    yield return new WaitForSeconds(.3f);
                }
            }
            else
            {
                Debug.Log("Spawn");

                WaveText(i, scenario.waveDelay);
                yield return new WaitForSeconds(scenario.waveDelay);
                Spawn(i);
            }
        }
    }

    void Spawn(int waveNumber)
    {
        var wave = scenario.Waves[waveNumber];
        for(int i = 0; i < wave.Enemies.Length; i++)
        {
            for(int j = 0; j < wave.Ratio[i]; j++)
            {
                float xPosition = Random.Range(
                        spawnArea.bounds.max.x, spawnArea.bounds.min.x
                );
                float yPosition = Random.Range(
                        spawnArea.bounds.max.y, spawnArea.bounds.min.y
                    );
                currentEnemies.Add(Instantiate(wave.Enemies[i], new Vector3(xPosition, yPosition, 0), Quaternion.identity));
            }
        }
    }

    void WaveText(int waveNumber, float delay)
    {
        UIManager.instance.WaveNumber(waveNumber + 1, delay);
        text.text = "WAVE: " + (waveNumber + 1);
    }
    void RemoveEnemy(GameObject g)
    {
        currentEnemies.Remove(g);
    }
}
