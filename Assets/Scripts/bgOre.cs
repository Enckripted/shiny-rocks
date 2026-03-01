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
        SpriteRenderer spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        // move left according to the player's drill speed (assume PlayerController always exists)
        if (playerDrill.IsMoving)
        {
            transform.Translate(Vector3.left * (float)playerDrill.DrillSpeed * Time.deltaTime, Space.World);
        }


        if (spriteRenderer != null && transform.position.x < -50)
        {
            Destroy(gameObject);
        }
    }
}
