using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Stats
{
    public float damage;
    [Range(0f, 1f)]
    public float criticalRate;
    [Range(0f, 1f)]
    public float criticalDamage;
    public float defense;
    public float cooldownRate;
    public float bonusHP;
    public float bonusSpeed;

    public static Stats operator +(Stats a, Stats b)
    {
        return new Stats
        {
            damage = a.damage + b.damage,
            criticalRate = Mathf.Clamp01(a.criticalRate + b.criticalRate),
            criticalDamage = a.criticalDamage + b.criticalDamage,
            defense = a.defense + b.defense,
            cooldownRate = a.cooldownRate + b.cooldownRate,
            bonusHP = a.bonusHP + b.bonusHP,
            bonusSpeed = a.bonusSpeed + b.bonusSpeed,
        };
    }

    public static Stats operator -(Stats a, Stats b)
    {
        return new Stats
        {
            damage = a.damage - b.damage,
            criticalRate = Mathf.Clamp01(a.criticalRate - b.criticalRate),
            criticalDamage = a.criticalDamage - b.criticalDamage,
            defense = a.defense - b.defense,
            cooldownRate = a.cooldownRate - b.cooldownRate,
            bonusHP = a.bonusHP - b.bonusHP,
            bonusSpeed = a.bonusSpeed - b.bonusSpeed
        };
    }
    public Stats Clone()
    {
        return new Stats
        {
            damage = this.damage,
            criticalRate = this.criticalRate,
            criticalDamage = this.criticalDamage,
            defense = this.defense,
            cooldownRate = this.cooldownRate,
            bonusHP = this.bonusHP,
            bonusSpeed = this.bonusSpeed,
        };
    }

}
