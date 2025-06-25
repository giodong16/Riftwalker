using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Animator anim;
    public Vector3 offset = new Vector3(0, 2, 0);
    [SerializeField] AudioSource audioSource;
    bool isChecked = false;
    private Vector3 respawnPos;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        respawnPos = transform.position+offset;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isChecked)
        {
            anim.SetTrigger("checked");

            isChecked = true;
            if (audioSource)
            {
                audioSource.PlayOneShot(audioSource.clip);
            }
            PlayerController.Instance.CurrentCheckPoint = respawnPos;

        }

    }
}
