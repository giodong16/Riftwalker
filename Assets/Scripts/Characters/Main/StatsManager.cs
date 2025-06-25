using System.Collections.Generic;
using UnityEngine;

public enum StatModifierType
{
    Base,
    Weapon, // knife, gun, rifle,bomb
    Ammo, // bullet
    Potion,
    Debuff,
}

[System.Serializable]
public class StatsManager
{
    private Dictionary<StatModifierType, Stats> modifiers = new Dictionary<StatModifierType, Stats>();
    [SerializeField] private Stats finalStats = new Stats();

    public void SetModifier(StatModifierType type, Stats stats)
    {
        modifiers[type] = stats.Clone();
        RecalculateFinalStats();
    }

    public void RemoveModifier(StatModifierType type)
    {
        if (modifiers.ContainsKey(type))
        {
            modifiers.Remove(type);
            RecalculateFinalStats();
        }
    }

    private void RecalculateFinalStats()
    {
        finalStats = new Stats();
        foreach (var mod in modifiers.Values)
        {
            finalStats += mod;
        }
    }

    public Stats GetFinalStats()
    {
        return finalStats.Clone();
    }

    public void ClearAllModifiers()
    {
        modifiers.Clear();
        finalStats = new Stats();
    }
}
