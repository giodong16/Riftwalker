using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemRuntimeData
{
    public ItemData itemData;
    public int quantity;

    public ItemRuntimeData(ItemData data, int quantity = 0)
    {
        this.itemData = data;
        this.quantity = quantity;
    }

    public void Use(int amount = 1)
    {
        if(quantity < amount)
        {
            AudioManager.Instance?.PlaySFX(AudioManager.Instance.warningClip);
            return;
        }
        if (!itemData.isOnly && quantity >= amount)
            
        {   if (itemData.itemType == ItemType.SupportItem && PlayerController.Instance.IsPotionActive) return;
            quantity -= amount;
           
        }
        
        itemData.UseItem(); 
    }
    public void Buy(int amount = 1)
    {
        quantity += amount;   
    }
}
public class ItemDataManager : MonoBehaviour
{
    public static ItemDataManager Instance;

    public List<ItemRuntimeData> runtimeItems;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        LoadItemQuantities();
    }

    public ItemRuntimeData GetItemRuntime(ItemData data)
    {
        return runtimeItems.Find(i => i.itemData.id == data.id);
    }

    public void AddItem(ItemData data, int quantity)
    {
        var item = GetItemRuntime(data);
        if (item != null)
        {
            item.Buy(quantity);
        }
        
        SaveItemQuantities();
    }

    public void SaveItemQuantities()
    {
        foreach (var item in runtimeItems)
        {
            PlayerPrefs.SetInt("item_" + item.itemData.id, item.quantity);
        }
        PlayerPrefs.Save();
    }

    public void LoadItemQuantities()
    {
        foreach (var item in runtimeItems)
        {
            string key = "item_" + item.itemData.id;

            if (PlayerPrefs.HasKey(key))
            {
                item.quantity = PlayerPrefs.GetInt(key);
            }
            else
            {
                item.quantity = item.itemData.defaultQuantity;
                PlayerPrefs.SetInt(key, item.quantity); 
            }
        }

        PlayerPrefs.Save();
    }

    public void DecreaseItem(ItemData data, int amount)
    {
        var item = GetItemRuntime(data);
        if (item != null)
        {
            item.Use(amount);
            SaveItemQuantities();
        }
    }
    public int GetQuality(ItemData item)
    {
        ItemRuntimeData itemRuntime = GetItemRuntime(item);
        if(itemRuntime != null)
        {
            return itemRuntime.quantity;
        }
        return 0;
    }
    [ContextMenu("Reset All Item Quantities")]
    public void ResetAllQuantities()
    {
        foreach (var item in runtimeItems)
        {
            PlayerPrefs.DeleteKey("item_" + item.itemData.id);
        }
        LoadItemQuantities();
    }
    [ContextMenu("Hack Items")]
    public void HackItems()
    {
        foreach(var item in runtimeItems)
        {
            item.quantity = 100;

        }
        SaveItemQuantities();
    }
}
