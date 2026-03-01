using UnityEngine;

public class PlayerDrill : Damagable
{
    public float DrillSpeed;
    public int InitialHealth;
    public float DrillDamage;
    public float DrillDepth;

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
