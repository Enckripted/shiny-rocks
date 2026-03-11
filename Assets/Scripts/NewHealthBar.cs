using TMPro;
using UnityEngine;

//aigen
public class NewHealthBar : MonoBehaviour
{
	[SerializeField] private GameObject healthBarObject;
	[SerializeField] private TMP_Text healthText;
	[SerializeField] private Health health; // Reference to Health object

	private Vector3 originalPos;
	private Vector3 originalScale;

	void Awake()
	{
		originalPos = healthBarObject.transform.localPosition;
		originalScale = healthBarObject.transform.localScale;
	}

	void Update()
	{
		if (health == null || health.MaxHealth <= 0) return;
		float percentage = Mathf.Clamp01(health.CurrentHealth / health.MaxHealth);
		healthBarObject.transform.localPosition = originalPos - new Vector3((1 - percentage) / 2.0f, 0.0f, 0.0f);
		healthBarObject.transform.localScale = originalScale - new Vector3(1 - percentage, 0.0f, 0.0f);
		healthText.text = Mathf.Ceil(health.CurrentHealth).ToString();
	}
}
