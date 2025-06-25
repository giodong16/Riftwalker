using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] GemType gemType;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetCoins();
            
            Destroy(gameObject);
        }
    }
    public void GetCoins()
    {
        int coins = 0;
        switch (gemType)
        {
            case GemType.Clear:
                coins = 100;
                break;
            case GemType.Yellow:
                coins = 200;
                break;
            case GemType.Green:
                coins = 300;
                break;
            case GemType.Blue:
                coins = 500;
                break;
            case GemType.Pink:
                coins = 800;
                break;
            case GemType.Red:
                coins = 1000;
                break;
        }
        Pref.Coins += coins;
        Debug.Log(coins);
    }
}
