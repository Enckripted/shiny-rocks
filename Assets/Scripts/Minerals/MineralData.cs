using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MineralData")]
public class MineralData : ScriptableObject
{
    public string Name;
    public int SellValue;
    public int MinimumDepth;
    public int MaxHealth;
    public Color Color;
    public Texture2D Sprite;
}