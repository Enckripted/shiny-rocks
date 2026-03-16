using Unity.VisualScripting;
using UnityEngine;

public class HealBubble : MonoBehaviour
{
    [SerializeField] private float animationTime;
    [SerializeField] private float startAlpha;
    private float startTime;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, startAlpha);
        startTime = Time.fixedTime;
    }

    void Update()
    {
        if (Time.fixedTime - startTime >= animationTime)
            Destroy(gameObject);
        spriteRenderer.color = spriteRenderer.color - new Color(0, 0, 0, startAlpha * ((Time.fixedTime - startTime) / animationTime));
    }
}