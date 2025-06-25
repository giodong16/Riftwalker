using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Data/Level/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelNumber;
    public int soulRequired;
    public int playerLives;
    public int livesLeftRequire;
    public int timeRequire;
    public string missionDescription;
    
}
