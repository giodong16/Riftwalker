using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "Data/Level/MapData")]
public class MapData : ScriptableObject
{
    public string mapName;
    public List<LevelData> levels;
}
