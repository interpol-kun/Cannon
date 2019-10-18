using UnityEngine;

//In-editor and runtime class to hold all player data. To serialize and deserialize data use only plain C# class
[CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Cannon stats")]
    public float FireRate;
    public int MaxHealth;
    public int Damage;
    [Header("Cannon level stats")]
    public int Experience;
    public int Level;
    [Header("Currency")]
    public int Money;
}
