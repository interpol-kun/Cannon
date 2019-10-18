using UnityEngine;

[CreateAssetMenu(fileName = "Scenario", menuName = "Scenario")]
public class Scenario : ScriptableObject
{
    [SerializeField]
    private EnemyWave[] waves;
    private int waveCount;
    public int multiplier;
    public int waveDelay;

    public EnemyWave[] Waves { get => waves; set => waves = value; }
    public int WaveCount { get => waveCount; set => waveCount = value; }

    private void OnEnable()
    {
        waveCount = waves.Length;
    }
}
