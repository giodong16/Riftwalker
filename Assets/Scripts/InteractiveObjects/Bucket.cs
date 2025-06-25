using System.Collections;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    private Quaternion initialRotation;
    private void Start()
    {
        initialRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        transform.rotation = initialRotation;
    }
   /* private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.par
        }
    }*/
}
