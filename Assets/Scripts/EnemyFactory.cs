using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{

    [SerializeField]
    private Scenario scenario;
    [SerializeField]
    private Collider2D spawnArea;

    [SerializeField]
    List<GameObject> currentEnemies = new List<GameObject>();

    [SerializeField]
    private TMPro.TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BeginGame());
        EnemyController.OnDeath += RemoveEnemy;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator BeginGame()
    {
        Debug.Log("Begin game");
        //if (scenario.waveDelay == 0 && currentEnemies.Capacity == 0)
        //{
        //    for(int i = 0; i < scenario.WaveCount; i++)
        //    {

        //    }
        //}
        for (int i = 0; i < scenario.WaveCount; i++)
        {
            if(scenario.waveDelay == 0)
            {
                Debug.Log(i);
                if(currentEnemies.Count == 0)
                {
                    yield return new WaitForSeconds(3f);
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
                Spawn(i);
                yield return new WaitForSeconds(scenario.waveDelay);
            }
        }
    }

    void Spawn(int waveNumber)
    {
        text.text = "Wave: " + (waveNumber+1);
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
    void RemoveEnemy(GameObject g)
    {
        currentEnemies.Remove(g);
    }
}
