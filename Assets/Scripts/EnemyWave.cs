using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Wave", menuName = "Enemy Wave")]
public class EnemyWave : ScriptableObject
{
    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private int enemyCount = 0;
    [SerializeField]
    private bool randomRatio = false;
    [SerializeField]
    private int[] ratio;

    void Randomize()
    {

    }
}