using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointResizeCamera : MonoBehaviour
{
    public float size = 5;
    public bool isZoomed = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isZoomed)
        {
            if (CameraTrigger.Instance != null)
            {
                CameraTrigger.Instance.Zoom(size);
                isZoomed = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isZoomed)
        {
            if (CameraTrigger.Instance != null)
            {
                CameraTrigger.Instance.Zoom();
                isZoomed = false;
            }
        }
    }

}
