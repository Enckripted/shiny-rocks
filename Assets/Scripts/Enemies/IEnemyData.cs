using UnityEngine;

public interface IEnemyData
{
	public string Name { get; }
	public int MaxHealth { get; }
	//public float Damage { get; }
	public float Speed { get; }
	public float AttackSpeed { get; }


	public float HealthScaleMult { get; }
	//public float DamageScaleMult { get; }
	public float SpeedScaleAdd { get; }
	public float AttackSpeedScaleAdd { get; }

	public Texture2D Sprite { get; }
	public GameObject EnemyObject { get; }
}