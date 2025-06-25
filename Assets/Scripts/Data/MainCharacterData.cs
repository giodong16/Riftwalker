using UnityEngine;

[CreateAssetMenu(fileName = "newMainCharacterData", menuName = "Data/Main character stats")]
public class MainCharacterData : ScriptableObject
{
    [Header("Identity")]
    public string id;
    public string characterName;

    [Header("Base Stats")]
    public int maxHP = 100;
    public Stats baseStats;

    [Header("Movement")]
    public float moveSpeed = 6.5f;
    public float jumpPower = 8f;
    public int maxJumps = 2;
    public float climbSpeed = 6f;

    [Header("Dash")]
    public float dashTime = 0.2f;
    public float dashSpeed = 25f;
}
