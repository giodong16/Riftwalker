using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponAnimatorMapping", menuName = "DataCharacter/WeaponAnimatorMapping")]
public class WeaponAnimatorMapping : ScriptableObject
{
    public WeaponType weaponType;
    public AnimatorOverrideController animatorOverride;
}
