using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{

    [SerializeField]
    private Scenario scenario;
    [SerializeField]
    private Collider2D spawnArea;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BeginGame());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator BeginGame()
    {
        Debug.Log("Begin game");
        for (int i = 0; i < scenario.WaveCount; i++)
        {
            Debug.Log("Spawn");
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
                float xPosition = Random.Range(
                        spawnArea.bounds.max.x, spawnArea.bounds.min.x
                );
                float yPosition = Random.Range(
                        spawnArea.bounds.max.y, spawnArea.bounds.min.y
                    );
                Instantiate(wave.Enemies[i], new Vector3(xPosition, yPosition, 0), Quaternion.identity);
            }
        }
    }
}
