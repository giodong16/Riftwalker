using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [Header("Data")]
    public List<ItemData> items = new List<ItemData>();

    [Header("UI GamePlay")]
    public GameObject itemUIPrefab;
    public RectTransform contentInventoryGamePlayTransform;
    public GameObject inventoryPanel;
    [Header("UI Home")]
    public GameObject itemUIHomePrefab;
    public RectTransform contentInventoryHomeTransform;
    public StatsDisplayUI statsDisplayUI;

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
        }

    }
    private void OnEnable()
    {
        UpdateInventoryUIs();
    }
    private void Start()
    {
        UpdateInventoryUIs();
    }

    public void UpdateInventoryUIs()
    {
        UpdateInventoryUI(contentInventoryHomeTransform,itemUIHomePrefab);
        UpdateInventoryUI(contentInventoryGamePlayTransform,itemUIPrefab);
    }
    public void UpdateInventoryUI(RectTransform rectTransform, GameObject prefab)
    {
        if (rectTransform != null) 
            foreach (Transform child in rectTransform)
            {
                Destroy(child.gameObject);
            }

        foreach (ItemData item in items)
        {
            if (ItemDataManager.Instance && ItemDataManager.Instance.GetQuality(item) > 0)
            {
                GameObject itemUI = Instantiate(prefab, rectTransform);
                itemUI.GetComponent<ItemUI>().SetUp(item);
                itemUI.GetComponent<ItemUI>().UpdateUI();
            }
        }
    }

    public void ShowInformationItem(string name,Stats stats)
    {
        if(statsDisplayUI != null)
        {
            statsDisplayUI.DisplayStats(name,stats);
        }
    }
    public void Show()
    {
        inventoryPanel.SetActive(true);
    }

    public void Hide()
    {
        inventoryPanel.SetActive(false);
    }
}
