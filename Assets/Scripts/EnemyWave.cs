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

    public int EnemyCount { get => enemyCount; set => enemyCount = value; }
    public GameObject[] Enemies { get => enemies; set => enemies = value; }
    public bool RandomRatio { get => randomRatio; set => randomRatio = value; }
    public int[] Ratio { get => ratio; set => ratio = value; }
}