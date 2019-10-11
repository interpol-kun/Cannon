using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Wave", menuName = "Enemy Wave")]
public class EnemyWave : ScriptableObject
{
    [SerializeField]
    private List<GameObject> enemies;
    [SerializeField]
    private List<int> ratio;

}