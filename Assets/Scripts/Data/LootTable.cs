using UnityEngine;

[CreateAssetMenu(fileName = "New Loot Table", menuName = "Loot System/Loot Table")]
public class LootTable : ScriptableObject
{
    public LootItem[] lootItems;
    public GameObject prefabs;
    public GameObject soulPrefab;
    public Vector2 offset = new Vector2(0, 0.5f);
 /*   public GameObject GetRandomLoot()
    {
        float totalChance = 0;
        foreach (var item in lootItems)
        {
            totalChance += item.dropChance;
        }

        float randomValue = Random.Range(0, totalChance);
        float cumulative = 0;

        foreach (var item in lootItems)
        {
            cumulative += item.dropChance;
            if (randomValue <= cumulative)
            {
               
               // return item.itemPrefab;
            }
        }
        return null;
    }*/
    public void SpawnSouls(Transform transformSpawn)
    {
        if (soulPrefab == null) return;
        Instantiate(soulPrefab,transformSpawn.position+(Vector3)offset,Quaternion.identity);
    }
}
