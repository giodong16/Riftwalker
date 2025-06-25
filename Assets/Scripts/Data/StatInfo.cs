using UnityEngine;

[CreateAssetMenu(fileName = "StatInfo", menuName = "UI/Stat Info")]
public class StatInfo : ScriptableObject
{
    public StatType statType;
    public string displayName;
    public Sprite icon;
}
