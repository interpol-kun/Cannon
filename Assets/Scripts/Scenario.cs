using UnityEngine;

[CreateAssetMenu(fileName = "Scenario", menuName = "Scenario")]
public class Scenario : ScriptableObject
{
    [SerializeField]
    private EnemyWave[] waves;
    private int waveCount;
    [Range(1f, 5f)]
    public float multiplier;
    [Tooltip("Set to 0 to spawn a new wave after the last enemy of previous wave is killed")]
    public int waveDelay;

    public EnemyWave[] Waves { get => waves; set => waves = value; }
    public int WaveCount { get => waveCount; set => waveCount = value; }

    private void OnEnable()
    {
        waveCount = waves.Length;
    }
}
