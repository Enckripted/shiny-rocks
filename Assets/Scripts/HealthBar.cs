using System;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject healthBarObject;
    [SerializeField] private TMP_Text healthText;

    public float MaxHealth;
    public float Health;

    private Vector3 originalPos;
    private Vector3 originalScale;

    void Update()
    {
        float percentage = Math.Clamp(Health / MaxHealth, 0.0f, 1.0f);
        healthBarObject.transform.localPosition = originalPos - new Vector3((1 - percentage) / 2.0f, 0.0f, 0.0f);
        healthBarObject.transform.localScale = originalScale - new Vector3(1 - percentage, 0.0f, 0.0f);
        healthText.text = Math.Ceiling(Health).ToString();
    }

    void Awake()
    {
        originalPos = healthBarObject.transform.localPosition;
        originalScale = healthBarObject.transform.localScale;
    }
}