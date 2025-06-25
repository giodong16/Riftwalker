// C# Unity example
using UnityEngine;

public class SwingingTrap : MonoBehaviour
{
    public float swingAngle = 45f; // Biên độ góc ±45 độ
    public float swingSpeed = 2f; // Tốc độ đung đưa => chuẩn sẽ chuyển thành góc mỗi đơn vị thời gian xoay được

    public GameObject rope;
    private float startRotationZ;
    void Update()
    {
        float t = (Mathf.Sin(Time.time * swingSpeed) + 1f) / 2f; // Đưa Sin từ [-1,1] về [0,1] 
        float angle = Mathf.Lerp(-swingAngle, swingAngle, t); // Lerp(a,b,t)=a+(b−a)×t
        rope.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
   /* private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance?.TakeDamage(9999);
        }
    }*/
}
