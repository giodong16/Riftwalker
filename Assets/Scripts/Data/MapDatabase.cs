using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapDatabase", menuName = "Level/MapDatabase")]
public class MapDatabase : ScriptableObject
{
    public List<MapData> maps;
}
