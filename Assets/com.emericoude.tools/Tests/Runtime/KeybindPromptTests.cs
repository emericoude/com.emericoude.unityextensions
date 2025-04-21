using System;
using Emericoude.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Emericoude.Tests
{
    public class KeybindPromptTests : MonoBehaviour
    {
        public enum CheckType
        {
            UseActiveActionBinding,
            UsePassiveActionBinding,
            UsePassiveActionBindingAsComposite
        }
        
        [SerializeField] private PlayerInput playerInputComponent;
        [SerializeField] private KeybindSpriteAssets keybindSpriteAssets;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        [FormerlySerializedAs("spriteRenderer")] [SerializeField] private Image image;
        [SerializeField] private CheckType checkType;
        
        private void OnEnable()
        {
            playerInputComponent.actions["Move"].performed += this.OnActionPerformed;
            playerInputComponent.actions["Crouch"].performed += this.OnActionPerformed;
        }

        private void Update()
        {
            Debug.Log($"active device: {SinglePlayerDeviceHandler.Instance.ActiveDevice.name}, active control: {SinglePlayerDeviceHandler.Instance.ActiveControlScheme}");
        }

        private void OnActionPerformed(InputAction.CallbackContext context)
        {
            textMeshProUGUI.text = checkType switch
            {
                CheckType.UseActiveActionBinding => $"You pressed {this.keybindSpriteAssets.GetActionRichTextForActiveControl(context.action)}!",
                CheckType.UsePassiveActionBinding => $"You pressed {this.keybindSpriteAssets.GetActionRichTextForControl(context.action, SinglePlayerDeviceHandler.Instance.ActiveDevice)}!",
                CheckType.UsePassiveActionBindingAsComposite => $"You pressed {this.keybindSpriteAssets.GetActionRichTextForControl(context.action, SinglePlayerDeviceHandler.Instance.ActiveDevice, KeybindSpriteAssets.CompositeHandlingMethods.UseCompositeName)}!)",
                _ => throw new ArgumentOutOfRangeException()
            };
            
            image.sprite = checkType switch
            {
                CheckType.UseActiveActionBinding => this.keybindSpriteAssets.GetActionSpriteForActiveControl(context.action),
                CheckType.UsePassiveActionBinding => this.keybindSpriteAssets.GetActionSpriteForControl(context.action, SinglePlayerDeviceHandler.Instance.ActiveDevice)[0],
                CheckType.UsePassiveActionBindingAsComposite => this.keybindSpriteAssets.GetActionSpriteForControl(context.action, SinglePlayerDeviceHandler.Instance.ActiveDevice, KeybindSpriteAssets.CompositeHandlingMethods.UseCompositeName)[0],
                _ => throw new ArgumentOutOfRangeException()
            };
            
        }
    }
}
