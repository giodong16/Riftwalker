using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownSystemUI : MonoBehaviour
{
    [System.Serializable]
    public class AttackCooldown
    {
        public NameAttack attackName;
        public Image cooldownImage; // UI cooldown cho từng attack
    }

    public List<AttackCooldown> cooldowns; // Danh sách các attack có cooldown UI
    private Dictionary<NameAttack, Coroutine> cooldownCoroutines = new Dictionary<NameAttack, Coroutine>();

    [Header("Potion")]
    public Image potionCooldownImage;
    private Coroutine potionCooldownCoroutine;

    public void StartCooldown(NameAttack attackName, float cooldownTime)
    {
        AttackCooldown cooldownUI = cooldowns.Find(c => c.attackName == attackName);
        if (cooldownUI == null || cooldownUI.cooldownImage == null) return;

        if (cooldownCoroutines.ContainsKey(attackName))
        {
            StopCoroutine(cooldownCoroutines[attackName]); 
        }

        cooldownCoroutines[attackName] = StartCoroutine(AnimateCooldown(attackName, cooldownUI.cooldownImage, cooldownTime));
    }

    private IEnumerator AnimateCooldown(NameAttack attackName, Image cooldownImage, float cooldownTime)
    {
        float elapsedTime = 0f;
        cooldownImage.fillAmount = 1;

        while (elapsedTime < cooldownTime)
        {
            elapsedTime += Time.deltaTime;
            cooldownImage.fillAmount = 1 - (elapsedTime / cooldownTime);
            yield return null;
        }

        cooldownImage.fillAmount = 0; 
        cooldownCoroutines.Remove(attackName);
    }
    public void StartCooldown(float potionCooldownTime)
    {
        if(potionCooldownCoroutine!=null)
        {
            StopCoroutine(potionCooldownCoroutine);
        }
        potionCooldownCoroutine = StartCoroutine(AnimateCooldown(potionCooldownTime));
    }
    private IEnumerator AnimateCooldown( float cooldownTime)
    {
        float elapsedTime = 0f;
        potionCooldownImage.fillAmount = 1;

        while (elapsedTime < cooldownTime)
        {
            elapsedTime += Time.deltaTime;
            potionCooldownImage.fillAmount = 1 - (elapsedTime / cooldownTime);
            yield return null;
        }

        potionCooldownImage.fillAmount = 0;
    }
    public void ResetAllCooldownUI()
    {
        // Reset cooldown của các kỹ năng
        foreach (var cooldown in cooldowns)
        {
            if (cooldown.cooldownImage != null)
            {
                cooldown.cooldownImage.fillAmount = 0;
            }
        }

        // Reset cooldown của potion
        if (potionCooldownImage != null)
        {
            potionCooldownImage.fillAmount = 0;
        }

        // Dừng toàn bộ coroutine 
        foreach (var coroutine in cooldownCoroutines.Values)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }
        cooldownCoroutines.Clear();

        if (potionCooldownCoroutine != null)
        {
            StopCoroutine(potionCooldownCoroutine);
            potionCooldownCoroutine = null;
        }
    }

}
