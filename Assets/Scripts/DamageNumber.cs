using System.Collections;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    // value to show
    public double damageNum = 0;

    private TextMeshPro damageText;

    [Header("Animation Settings")]
    [Tooltip("How far (in canvas units) the number should move from its start anchored position")]
    [SerializeField] private float moveDistance = 1f;
    [Tooltip("Duration of the animation in seconds (move + fade)")]
    [SerializeField] private float animationDuration = 1f;

    private RectTransform rect;
    private Vector2 startPos;
    private float elapsed;

    private Vector2 direction;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        damageText = GetComponent<TextMeshPro>();
    }

    void Update()
    {
        damageText.text = damageNum.ToString("F0");
    }

    void OnEnable()
    {
        // prepare for animation
        if (rect != null)
            startPos = rect.anchoredPosition;

        if (damageText != null)
        {
            Color c = damageText.color;
            c.a = 1f;
            damageText.color = c;

        }

        // start coroutine that handles movement/fade and cleanup
        direction = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
        StartCoroutine(AnimateAndDestroy());
    }

    private IEnumerator AnimateAndDestroy()
    {
        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);

            // move upward
            if (rect != null)
                rect.anchoredPosition = startPos + direction * moveDistance * t;

            // fade
            if (damageText != null)
            {
                Color c = damageText.color;
                c.a = Mathf.Lerp(1f, 0f, t);
                damageText.color = c;
                //damageText.text = damageNum.ToString("F1"); // in case value changes
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}