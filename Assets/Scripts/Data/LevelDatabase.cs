using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "Data/Level/Level Database")]
public class LevelDatabase : ScriptableObject
{
    public LevelData[] levels;
}
