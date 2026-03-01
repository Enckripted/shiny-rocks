using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string Name;
    public int MaxHealth;
    public float Damage;
    public float Speed;
    public float AttackSpeed;
    public Texture2D Sprite;
}