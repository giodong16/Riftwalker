using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WindZone : MonoBehaviour
{
    [SerializeField] Vector2 direction;
    [SerializeField] float strength = 5f;
    [SerializeField] Transform rootTransform;


    private void OnTriggerStay2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null) {
            if (rootTransform != null) {
                rb.AddForce(direction * strength* rootTransform.localScale.x);
            }
            else
            {
                rb.AddForce(direction * strength);
            }
            
        }
    }
}
