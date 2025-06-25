using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public string fullText;
    public float delay = 0.05f;

    public AudioSource audioSource;
    public AudioClip typeClip;
    private Coroutine typingCoroutine;
    /*    private string[] contents = 
            { "“17.04.2047 – Trạm Nghiên cứu số 6.\r\nBão năng lượng ngoài vùng kiểm soát…”",
            "“Chồng tín hiệu năng lượng? Không thể nào…”\r\n“Nó đang mở... một cổng không gian?!”",
            "Khôngggg!",
            "“…Mình còn sống?”\r\n“Đây là nơi quái quỷ nào vậy…?”",
            "“Không khí… lạ. Thực vật chưa từng ghi nhận.”\r\n“Cứ như một chiều không gian hoàn toàn khác…”",
            "“Phải ra khỏi đây. Phải tìm đường về…”",
            "“...nếu có một lối ra nào đó tồn tại…”"};*/

    private string[] contents =
{
    "“17.04.2047 – Research Station No. 6.\nEnergy storm out of control…”",
    "“Overlapping energy signals? That’s impossible…”\n“It’s opening… a spatial rift?!”",
    "Nooo!",
    "“…I’m alive?”\n“What the hell is this place…?”",
    "“The air… strange. Unknown flora.”\n“It’s like an entirely different dimension…”",
    "“I have to get out of here. Find a way back…”",
    "“…if there even is a way out…”"
};


    public void StartTyping(int index)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        if(contents.Length >= index)  
        {
            fullText = contents[index];
        }
        audioSource.Play();
        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        textMeshPro.text = "";
        foreach (char letter in fullText)
        {
            textMeshPro.text += letter;
            yield return new WaitForSeconds(delay);
        }
        audioSource.Stop();
        yield return new WaitForSeconds(0.5f);
        textMeshPro.text = "";

    }
}
