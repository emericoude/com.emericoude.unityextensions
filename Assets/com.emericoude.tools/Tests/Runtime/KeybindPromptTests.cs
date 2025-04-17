using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Emericoude.Tests
{
    public class KeybindPromptTests : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInputComponent;
        [SerializeField] private KeybindSpriteAssets keybindSpriteAssets;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        private void OnEnable()
        {
            playerInputComponent.actions["Move"].performed += this.OnActionPerformed;
        }

        private void OnActionPerformed(InputAction.CallbackContext context)
        {
            textMeshProUGUI.text = $"You pressed {this.keybindSpriteAssets.GetActionActiveBindingToRichText(context.action)}!";
        }
    }
}
