using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointToPan : MonoBehaviour
{
    [Header("Nên đặt trong object có collider là istrigger")]
    public Transform target;
    public float durationTime = 0.5f;
    public float holdTime = 1f;

    [SerializeField] bool isPaned = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision!=null && !isPaned)
        {
            isPaned = true;
            
            if (CameraTrigger.Instance && target != null)
            {
               // CameraTrigger.Instance.PanToPosition(target, durationTime, holdTime);              
            }
        }
    }
  /*  private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isPaned)
        {
            isPaned = false;
        }
    }*/
}
