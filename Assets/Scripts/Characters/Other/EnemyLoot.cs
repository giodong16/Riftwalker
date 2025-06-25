using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    public LootTable lootTable;
    [Range(0f, 1f)] public float dropRate = 0.5f;
    public float dropForce = 200f;
    public void DropLoot()
    {
        if (lootTable == null) return;

        lootTable.SpawnSouls(transform);

        if (Random.value > dropRate) return;
       /* GameObject loot = lootTable.GetRandomLoot();
        if (loot != null)
        {
           GameObject lootItem = Instantiate(loot, transform.position, Quaternion.identity);
            // thay sprite 
            // thêm lực 
            Vector2 dropDirection = new Vector2(Random.Range(-1f,1f), Random.Range(0f, 1f));
            // 
            lootItem.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);

        }*/
    }
/*    private void OnDestroy()
    {
        DropLoot();
    }*/
}
