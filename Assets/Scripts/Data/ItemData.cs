using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newItemData",menuName ="Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite itemIcon;
    public int defaultQuantity = 0;
    public int price;
    public bool canSell = true;
    public ItemType itemType;
    public ParticleName particleName;
    public bool isOnly = false;
    public string key;
    public Stats itemStats;

    public void UseItem()
    {
        var player = PlayerController.Instance;
        var statsManager = player.StatsManager;
        switch (itemType)
        {
            case ItemType.Weapons:
                statsManager.SetModifier(StatModifierType.Weapon, itemStats);
                statsManager.RemoveModifier(StatModifierType.Ammo);
                break;

            case ItemType.Ammo:
                statsManager.SetModifier(StatModifierType.Ammo, itemStats);   
                break;

            case ItemType.SupportItem:
                player.UsePotion(itemStats, particleName);
                break;
        }
    }
}
