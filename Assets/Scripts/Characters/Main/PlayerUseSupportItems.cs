using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUseSupportItems : MonoBehaviour
{
    [SerializeField]ItemData buffPotion;
    [SerializeField]ItemData healingPotion;
    [SerializeField] ItemData shiledPotion;
    [SerializeField] ItemData thunderPotion;
    public void UsePotionBuff(InputAction.CallbackContext context)
    {
        if (!PlayerController.Instance.CanController || GameController.Instance.gameState != GameState.Playing) return;
        if (context.performed)
        {
            ItemDataManager.Instance.DecreaseItem(buffPotion, 1);
        }

    }

    public void UsePotionHealing(InputAction.CallbackContext context)
    {
        if (!PlayerController.Instance.CanController || GameController.Instance.gameState != GameState.Playing) return;
        if (context.performed)
        {
            ItemDataManager.Instance.DecreaseItem(healingPotion, 1);
        }

    }
    public void UseShiled(InputAction.CallbackContext context)
    {
        if (!PlayerController.Instance.CanController || GameController.Instance.gameState != GameState.Playing) return;
        if (context.performed)
        {
            ItemDataManager.Instance.DecreaseItem(shiledPotion, 1);
        }

    }

    public void UseThunder(InputAction.CallbackContext context)
    {
        if (!PlayerController.Instance.CanController || GameController.Instance.gameState != GameState.Playing) return;
        if (context.performed)
        {
            ItemDataManager.Instance.DecreaseItem(thunderPotion, 1);
        }

    }
}
