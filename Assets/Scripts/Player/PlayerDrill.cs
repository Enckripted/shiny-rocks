using UnityEngine;

public class PlayerDrill : Damagable
{
    public float DrillSpeed;
    public int InitialHealth;
    public float DrillDamage;
    public float DrillDepth;

    private void StopRun()
    {
        GameManager.instance.StopRun();
    }

    private void OnRunBegin()
    {
        Health = InitialHealth;
        MaxHealth = InitialHealth;
        UpdateHealthbar();
    }

    void Start()
    {

        OnDeathEvent.AddListener(StopRun);
        GameManager.instance.runStartEvent.AddListener(OnRunBegin);
        OnRunBegin();
    }
}
