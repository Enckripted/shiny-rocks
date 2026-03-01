using System;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;

public class bgOreSpawner : MonoBehaviour
{
    [SerializeField] GameObject bgOre;
    private PlayerDrill playerDrill;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Vector2 range1;
    [SerializeField] private Vector2 range2;
    [SerializeField] private Vector2 scaleRange;
    private bool coroutineStarted = false;

    void Awake()
    {
        playerDrill = FindFirstObjectByType<PlayerDrill>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // move left according to the player's drill speed (assume PlayerController always exists)
        if (playerDrill.IsMoving)
        {
            if (!coroutineStarted)
            {
                StartCoroutine(OreSpawn());
                coroutineStarted = true;
            }
            transform.Translate(Vector3.left * (float)playerDrill.DrillSpeed * Time.deltaTime, Space.World);
        }

        // destroy once it is no longer visible by any camera
        if (spriteRenderer != null && !spriteRenderer.isVisible && transform.position.x < 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator OreSpawn()
    {
        while (true)
        {
            if (playerDrill.IsMoving)
            {
                GameObject ore = Instantiate(bgOre);
                float x = Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect);
                float y;
                int randInt = UnityEngine.Random.Range(1, 3);
                if(randInt == 1)
                {
                    y = UnityEngine.Random.Range(range1.x, range1.y);
                    
                }
                else
                {
                    y = UnityEngine.Random.Range(range2.x, range2.y);
                }
                ore.transform.position = new Vector2(x+3, y);
                float scale = UnityEngine.Random.Range(scaleRange.x, scaleRange.y);
                ore.transform.localScale.Set(scale, scale, 1);

                float randomRotation = UnityEngine.Random.Range(0f, 360f);
                ore.transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);
                
            }
            float waitTime = UnityEngine.Random.Range(1f, 3f);   
            yield return new WaitForSeconds(waitTime);
        }
    }

}

