using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayDamageManage : MonoBehaviour
{
    public static DisplayDamageManage Instance;
    public GameObject damageTextPrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowDamage(int amount, Vector3 position, Color color)
    {
        GameObject go = Instantiate(damageTextPrefab, position, Quaternion.identity);
        go.GetComponent<FloatingDamageText>().Init(amount, color);
    }
}
