using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float AttackSpeed { get; private set; }
    [field: SerializeField] public List<Sprite> Sprite { get; private set; }

    [Header("Every 10 meters scale by")]
    [field: SerializeField] public float HealthScaleMult { get; private set; }
    [field: SerializeField] public float DamageScaleMult { get; private set; }
    [field: SerializeField] public float SpeedScaleAdd { get; private set; }
    [field: SerializeField] public float AttackSpeedScaleAdd { get; private set; }

    [Header("Spawn Conditions/Chance")]
    [field: SerializeField] public float SpawnDepth { get; private set; }
    [field: SerializeField] public float SpawnWeight { get; private set; }

    [field: SerializeField] public GameObject EnemyObject { get; private set; }
}