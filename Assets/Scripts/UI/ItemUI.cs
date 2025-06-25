using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    ItemData itemData;
    public bool isHome = false;
    [SerializeField] Text qualityText;
    [SerializeField] Image itemImage;
    [SerializeField] Button itemButton;
    [SerializeField] Text keyNameText;
    public ItemData ItemData { get => itemData; set => itemData = value; }
    private void Awake()
    {
        if (!isHome)
        {
           // itemButton.onClick.AddListener(UseItem);
        }
        else
        {
            itemButton.onClick.AddListener(ShowInfomation);// show
        }

    }
    private void Update()
    {
        UpdateUI();
    }
    public void SetUp(ItemData itemData)
    {
        this.itemData = itemData;
        keyNameText.text= itemData.key;
    }
    public void UpdateUI()
    {
        if (itemData == null) return;
        int quality = ItemDataManager.Instance.GetQuality(itemData);
        if (quality > 0)
        {
            qualityText.text = ItemDataManager.Instance.GetQuality(itemData).ToString();
            itemImage.sprite = itemData.itemIcon;
        }
        else
        {
            Destroy(gameObject);
        }

    }
/*    public void UseItem()
    {
        if (itemData == null) return;
        itemData.UseItem();
        UpdateUI();
       

    }*/
    public void ShowInfomation()
    {
        InventoryManager.Instance.ShowInformationItem(itemData.itemName, itemData.itemStats);
    }

}
