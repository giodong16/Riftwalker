using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SFXToggle : MonoBehaviour, IPointerClickHandler
{
    public Image fillImage;
    public Image handleImage;
    public Sprite handleSpriteOn;
    public Sprite handleSpriteOff;

    [SerializeField] private RectTransform handle;
    [SerializeField] private bool isOn = false;
    [SerializeField] private float targetFillAmount;

    [SerializeField] private Vector2 handlePositionOn;
    [SerializeField] private Vector2 handlePositionOff;
    [SerializeField] private float durationTransitionTime = 0.5f;

    public bool IsOn { get => isOn; set => isOn = value; }

    private void Start()
    {
        float savedVolume = Pref.VolumeSFX;

        IsOn = savedVolume == 1.0f;

        UpdateVisualState(true);

     //   AudioManager.Instance?.ToggleSFX();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }
    public void Toggle(bool isInstant = false)
    {
        IsOn = !IsOn;

        Pref.VolumeSFX = IsOn ? 1.0f : 0.0001f;

        AudioManager.Instance?.ToggleSFX();

        UpdateVisualState(isInstant);
    }

    private void UpdateVisualState(bool isInstant)
    {
        handleImage.sprite = IsOn ? handleSpriteOn : handleSpriteOff;
        targetFillAmount = IsOn ? 1f : 0f;

        if (isInstant)
        {
            fillImage.fillAmount = targetFillAmount;
            handle.anchoredPosition = IsOn ? handlePositionOn : handlePositionOff;
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(AnimateToggle(targetFillAmount));
        }
    }


    private IEnumerator AnimateToggle(float targetFill)
    {
        float elapsedTime = 0f;
        float startFill = fillImage.fillAmount;
        Vector2 startPos = handle.anchoredPosition;
        Vector2 endPos = IsOn ? handlePositionOn : handlePositionOff;
        while (elapsedTime < durationTransitionTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / durationTransitionTime;
            fillImage.fillAmount = Mathf.Lerp(startFill, targetFill, t);
            handle.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        fillImage.fillAmount = targetFill;
        handle.anchoredPosition = endPos;
    }


}
