using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingDamageText : MonoBehaviour
{
    public Text damageText;
    public float speed = 1f;
    public float lifeTime = 1f;

    private Vector3 moveDirection;

    public void Init(int damageAmount, Color color)
    {
        damageText.text = damageAmount.ToString();
        damageText.color = color;
        moveDirection = new Vector3 (Random.Range(-0.5f,0.5f), 1f, 0);
        Destroy (gameObject,lifeTime);
    
    }
    private void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
