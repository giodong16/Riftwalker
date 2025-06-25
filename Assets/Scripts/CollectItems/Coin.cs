using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    private SpriteRenderer sprite;
    public float durationTimeFade;
    public float moveUpDistance = 1f;
    [Range(100, 1000)]
    public int quality = 100;
    public GameObject plusTextPrefab;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    private void CollectCoin()
    {
        StartCoroutine(CoinEffect());
        AudioManager.Instance?.PlaySFX(PlayerController.Instance.audioSource, PlayerController.Instance.collectSound,0.3f);
        GameObject plusText = Instantiate(plusTextPrefab, transform.position + Vector3.up * moveUpDistance*0.5f,Quaternion.identity,transform);
        plusText.GetComponent<PlusText>().ShowPlusText(quality,durationTimeFade);
    }

    IEnumerator CoinEffect() {
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + Vector3.up * moveUpDistance;

        while (elapsedTime < durationTimeFade) {
            float t = elapsedTime / durationTimeFade;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            sprite.color = new Color(1, 1, 1, 1 - t);
            elapsedTime += Time.deltaTime;
            yield return null;

        }
        //
        GameController.Instance?.CollectCoins(quality);
        Destroy(gameObject);
      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollectCoin();
        }
    }
}
