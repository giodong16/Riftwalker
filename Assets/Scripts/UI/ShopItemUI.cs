using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    private ItemData itemData;
    private int quality = 1;
    public Image itemImage;
    public Button buyButton;
    public Text priceText;
    public Text qualityText;


    public ItemData ItemData { get => itemData; set => itemData = value; }
    private void Awake()
    {
        if (buyButton == null) return;
            buyButton.onClick.AddListener(BuyItem);
    }
    public void UpdateShopItemUI()
    {
        if (itemData == null) return;
       
        itemImage.sprite = itemData.itemIcon;
        priceText.text = itemData.price.ToString();
    }

    private void BuyItem()
    {
        if (itemData == null) return;
        ResourceBar resourceBar = FindObjectOfType<ResourceBar>();
        if (Pref.Coins >= itemData.price)
        {
            // itemData.quantity += quality;
            ItemDataManager.Instance.AddItem(itemData,quality);
            Pref.Coins -= itemData.price;
            resourceBar.UpdateCoinsUI();
            InventoryManager.Instance.UpdateInventoryUIs();
            if (itemData.itemType == ItemType.Weapons && itemData.isOnly) {
                itemData.canSell = false;
                Destroy(gameObject);
            }
        }
        else
        {
            GameController.Instance.OnShowMessage("Message",Pref.MESSAGE_COINS);
        }
        

    }
}
