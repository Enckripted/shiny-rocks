using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject, IEnemyData
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float AttackSpeed { get; private set; }
    [field: SerializeField] public Texture2D Sprite { get; private set; }

    [Header("Every 10 meters scale by")]
    [field: SerializeField] public float HealthScaleMult { get; private set; }
    [field: SerializeField] public float DamageScaleMult { get; private set; }
    [field: SerializeField] public float SpeedScaleAdd { get; private set; }
    [field: SerializeField] public float AttackSpeedScaleAdd { get; private set; }

    [field: SerializeField] public GameObject EnemyObject { get; private set; }
}