using UnityEngine;

public class bgOre : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDrill playerDrill = FindFirstObjectByType<PlayerDrill>();
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        // move left according to the player's drill speed (assume PlayerController always exists)
        if (playerDrill.IsMoving)
        {
            transform.Translate(Vector3.left * playerDrill.DrillSpeed * Time.deltaTime, Space.World);
        }

        // destroy once it is no longer visible by any camera
        if (spriteRenderer != null && !spriteRenderer.isVisible && transform.position.x < 0)
        {
            Destroy(gameObject);
        }
    }
}
