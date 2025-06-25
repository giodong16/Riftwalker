using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owl : MonoBehaviour
{
    public Animator anim;
    [Range(3f,10f)]
    public float timeReset = 5f;
    public bool isAwake = false;
    Coroutine coroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !isAwake)
        {
            isAwake = true;
            anim.SetTrigger("awake");
            if (coroutine != null)
            {
                coroutine = null;
            }
           
            coroutine = StartCoroutine(Sleep());
        }
    }

    IEnumerator Sleep()
    {
        yield return new WaitForSeconds(timeReset);
        isAwake = false;
       
    }
}
