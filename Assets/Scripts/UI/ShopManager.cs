using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Data")]
    public List<ItemData> items = new List<ItemData>();

    [Header("UI")]
    public GameObject shopItemUIPrefab;
    public RectTransform tabAmmosContent;
    public RectTransform tabWeaponsContent;
    public RectTransform tabSupportItemsContent;

    private Dictionary<ItemType, Transform> tabContents;
    private void Awake()
    {
        tabContents = new Dictionary<ItemType, Transform>
        {
            { ItemType.Ammo, tabAmmosContent },
            { ItemType.Weapons, tabWeaponsContent },
            { ItemType.SupportItem, tabSupportItemsContent }
        };
    }
    private void OnEnable()
    {
        RefreshUI();
    }
    public void AddItemData(ItemData item)
    {
        if (item != null)
        {
            items.Add(item);
            RefreshUI();
        }
    }

    public void RefreshUI()
    {
        ClearAllUI();
        UpdateAllTabs();
    }

    private void UpdateAllTabs()
    {
        if (items == null || items.Count == 0 || shopItemUIPrefab == null) return;

        foreach (var item in items)
        {
            if (tabContents.TryGetValue(item.itemType, out Transform parent) && parent != null && item.canSell)
            {
                CreateShopItemUI(item, parent);
            }
        }
    }

    private void CreateShopItemUI(ItemData item, Transform parent)
    {
        GameObject shopItemUI = Instantiate(shopItemUIPrefab, parent);
        ShopItemUI shopUIComponent = shopItemUI.GetComponent<ShopItemUI>();
        if (shopUIComponent != null)
        {
            shopUIComponent.ItemData = item;
            shopUIComponent.UpdateShopItemUI();
        }
    }

    private void ClearAllUI()
    {
        foreach (var content in tabContents.Values)
        {
            ClearChildren(content);
        }
    }

    private void ClearChildren(Transform parent)
    {
        if (parent == null) return;
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
