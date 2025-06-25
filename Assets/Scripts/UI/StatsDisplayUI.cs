using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class StatsDisplayUI : MonoBehaviour
{
    public GameObject statInfoPrefab;     
    public RectTransform root;
    public Text nameText;
    public List<StatInfo> statInfoList;
    

    private Dictionary<StatType, StatInfo> statInfoDict;

    private void Awake()
    {
        statInfoDict = new Dictionary<StatType, StatInfo>();
        foreach (var info in statInfoList)
        {
            statInfoDict[info.statType] = info;
        }
        Clear();
    }
    private void OnDisable()
    {
        Clear();
    }
    public void Clear()
    {
        nameText.text = "";
        foreach (Transform child in root)
        {
            Destroy(child.gameObject);
        }
    }

    public void DisplayStats(string name ,Stats stats)
    {
        Clear();
        DisplayName(name);
        void CreateEntry(StatType type, float value)
        {
            if (value == 0f || !statInfoDict.ContainsKey(type)) return;

            GameObject ui = Instantiate(statInfoPrefab, root);
            StatUI statUI = ui.GetComponent<StatUI>();

            StatInfo info = statInfoDict[type];
            statUI.SetUp(info.icon, value.ToString());
        }
        
        CreateEntry(StatType.Damage, stats.damage);
        CreateEntry(StatType.CriticalRate, stats.criticalRate);
        CreateEntry(StatType.CriticalDamage, stats.criticalDamage);
        CreateEntry(StatType.Defense, stats.defense);
        CreateEntry(StatType.CooldownRate, stats.cooldownRate);
        CreateEntry(StatType.BonusHP, stats.bonusHP);
        CreateEntry(StatType.BonusSpeed, stats.bonusSpeed);
    }
    public void DisplayName(string name) { 
        nameText.text = name;
    }
}
