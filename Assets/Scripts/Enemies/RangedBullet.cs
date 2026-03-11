using UnityEngine;

//mostly aigen
public class RangedBullet : MonoBehaviour
{
	private float damage;
	private float speed;
	private Vector3 targetPosition;
	private bool initialized = false;

	public void Initialize(float damage, float speed, Vector3 targetPosition)
	{
		this.damage = damage;
		this.speed = speed;
		this.targetPosition = targetPosition;
		initialized = true;
	}

	private void HandleCollision(Collider2D other)
	{
		if (other.gameObject == GameManager.instance.PlayerDrill.gameObject)
		{
			Health drillHealth = other.GetComponent<Health>();
			if (drillHealth != null)
			{
				drillHealth.TakeDamage(damage);
			}
			Destroy(gameObject);
		}
		else if (!other.isTrigger)
		{
			// Destroy bullet on any solid collision
			Destroy(gameObject);
		}
	}

	void Update()
	{
		if (!initialized) return;
		// Move towards the target position
		Vector3 direction = (targetPosition - transform.position).normalized;
		transform.position += direction * speed * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		HandleCollision(other);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		HandleCollision(collision.collider);
	}
}
