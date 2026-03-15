using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    public int Level { get; private set; }
    public float Cost { get => GetCost(); }

    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public float InitialCost { get; private set; }
    [field: SerializeField] public float CostScale { get; private set; }

    public abstract PlayerStats ApplyEffect(PlayerStats baseStats);

    private float GetCost()
    {
        return 0;
    }

    public void LevelUp() { }
    public void LevelDown() { }

    void Awake()
    {
        Level = 0;
    }
}