using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAttackData", menuName = "DataCharacter/PlayerDataAttack")]
public class AttackData : ScriptableObject
{
    public NameAttack attackName;
    public float cooldownTime; //thời gian hồi chiêu
    /*[HideInInspector]*/ public float lastAttackTime = -100f; // lần cuối tấn công
}
