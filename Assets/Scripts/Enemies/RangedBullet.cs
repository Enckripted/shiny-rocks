using UnityEngine;

//mostly aigen
public class RangedBullet : MonoBehaviour
{
	private float damage;
	private float speed;
	private Vector3 targetPosition;

	public void Initialize(float damage, float speed, Vector3 targetPosition)
	{
		this.damage = damage;
		this.speed = speed;
		this.targetPosition = targetPosition;
	}

	private void HandleCollision(Collider2D other)
	{
		if (other.gameObject == GameManager.instance.PlayerDrill.gameObject)
		{
			//TODO: once june has abilities ready migrate the player drill to the Health component
			/*
			Health drillHealth = other.GetComponent<Health>();
			if (drillHealth != null)
			{
				drillHealth.TakeDamage(damage);
			}*/
			GameManager.instance.PlayerDrill.DealDamage(damage);
			Destroy(gameObject);
		}
	}

	void Update()
	{
		if (!GameManager.instance.inRun)
		{
			Destroy(gameObject);
		}

		// Move towards the target position
		Vector3 direction = (targetPosition - transform.position).normalized;
		transform.position += direction * speed * Time.deltaTime;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		HandleCollision(collision.collider);
	}
}
