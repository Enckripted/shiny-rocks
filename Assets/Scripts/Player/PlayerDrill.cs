public class PlayerDrill : Damagable
{
    public int InitialHealth;

    public void StartRun()
    {
        Health = InitialHealth;
        MaxHealth = InitialHealth;
    }
}