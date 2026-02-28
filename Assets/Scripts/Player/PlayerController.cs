using UnityEngine;

public class PlayerController : Damagable
{
    public float DrillSpeed;
    public int InitialHealth;

    private void EndRun()
    {
        //TODO: end run
    }

    public void OnRunBegin()
    {
        Health = InitialHealth;
        MaxHealth = InitialHealth;
        UpdateHealthbar();
    }

    void Start()
    {
        OnDeathEvent.AddListener(EndRun);
        OnRunBegin();
    }
}
