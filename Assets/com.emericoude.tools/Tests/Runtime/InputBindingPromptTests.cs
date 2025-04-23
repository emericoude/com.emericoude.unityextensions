using System;
using System.Linq;
using Emericoude.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Emericoude.Tests
{
    public class InputBindingPromptTests : MonoBehaviour
    {
        public enum CheckType
        {
            UseActiveActionBinding,
            UsePassiveActionBinding,
            UsePassiveActionBindingAsComposite
        }
        
        [SerializeField] private PlayerInput playerInputComponent;
        [SerializeField] private InputBindingSpriteAssetList spriteAssetList;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        [SerializeField] private Image image;
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
                CheckType.UseActiveActionBinding => $"You pressed {this.spriteAssetList.GetRichTextSpriteForActionInContext(context)}!",
                CheckType.UsePassiveActionBinding => $"You pressed {this.spriteAssetList.GetRichTextSpriteForActionControlMatch(context.action, SinglePlayerDeviceHandler.Instance.ActiveDevice, InputBindingSpriteHelpers.RichTextCompositeHandling.EachBindingSpriteSideBySide)}!",
                CheckType.UsePassiveActionBindingAsComposite => $"You pressed {this.spriteAssetList.GetRichTextSpriteForActionControlMatch(context.action, SinglePlayerDeviceHandler.Instance.ActiveDevice, InputBindingSpriteHelpers.RichTextCompositeHandling.UseCompositeNameForLookup)}!)",
                _ => throw new ArgumentOutOfRangeException()
            };
            
            image.sprite = checkType switch
            {
                CheckType.UseActiveActionBinding => this.spriteAssetList.GetSpriteForActionInContext(context),
                CheckType.UsePassiveActionBinding => this.spriteAssetList.GetSpritesForActionControlMatch(context.action, SinglePlayerDeviceHandler.Instance.ActiveDevice, InputBindingSpriteHelpers.SpriteCompositeHandling.SpriteListForEachBinding).FirstOrDefault(),
                CheckType.UsePassiveActionBindingAsComposite => this.spriteAssetList.GetSpritesForActionControlMatch(context.action, SinglePlayerDeviceHandler.Instance.ActiveDevice, InputBindingSpriteHelpers.SpriteCompositeHandling.UseCompositeNameForSingleSprite).FirstOrDefault(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
