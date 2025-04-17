using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.Serialization;
using ZLinq;

namespace Emericoude
{
    //TODO: needs further support for specific device types
    // this sort of works as long as the device you want exists
    // For instance, steam deck does not have a specific input device type in Unity, so this may require further extension
    // Also, its good to give the user the option to choose which gamepad type to show (instead of auto-detect), so it'd be good to have it be overrideable
    // This is good enough for now though.
    [CreateAssetMenu(menuName = "Emericoude/Keybind Sprite Assets", fileName = "New Keybind Sprite Assets", order = 0)]
    public class KeybindSpriteAssets : ScriptableObject
    {
        public enum SpriteSheetCharacterNamingMode
        {
            OnlyControlPath,
            SpriteSheetNameThenUnderscoreThenControlPath,
        }
        
        [Serializable]
        public struct SpriteAssetEntry
        {
            [SerializeReference] public InputDevice deviceType;
            public TMP_SpriteAsset spriteAsset;
        }

        [Tooltip("The sprite assets, associated to a device type. Put child type overrides at the top.")]
        public List<SpriteAssetEntry> spriteAssetsSerialized = new List<SpriteAssetEntry>();
        
        [Tooltip("The way you will be naming each characters in your sprite assets." +
                 "\n\nOnlyControlPath - Basically only the character, for instance \"W\"." +
                 "\n\nSpriteSheetNameThenUnderscoreThenControlPath - Formatted to the following: \"Keyboard&Mouse_W\".")]
        public SpriteSheetCharacterNamingMode characterNamingMode = SpriteSheetCharacterNamingMode.OnlyControlPath;
        
        private Dictionary<Type, TMP_SpriteAsset> spriteAssetsByType;

        private void OnEnable()
        {
            spriteAssetsByType = new Dictionary<Type, TMP_SpriteAsset>();
            foreach (var spriteAsset in spriteAssetsSerialized)
            {
                spriteAssetsByType.Add(spriteAsset.deviceType.GetType(), spriteAsset.spriteAsset);
            }
        }

        //TODO: support composites
        public string GetActionActiveBindingToRichText(InputAction action)
        {
            Type inputDeviceType = action.activeControl.device.GetType();
            int bindingIndex = action.GetBindingIndexForControl(action.activeControl);
            action.GetBindingDisplayString(bindingIndex, out string deviceLayout, out string controlPath);

            if (!this.spriteAssetsByType.TryGetValue(inputDeviceType, out var spriteAsset)) 
            {
                //if we did not find a direct Type reference in the dictionary, try to find a parent type
                spriteAsset = spriteAssetsByType.AsValueEnumerable().FirstOrDefault(kvp => inputDeviceType.IsSubclassOf(kvp.Key)).Value;
                if (spriteAsset == null) //if we are still null, return a text-based fallback
                {
                    Debug.LogWarning($"Could not find a sprite asset for the device type (or parent type): {inputDeviceType}.", this);
                    return controlPath;
                }
            }
            
            //format the control path, in case a user wants to use one or another naming convention
            string formattedControlPath = this.characterNamingMode switch
            {
                SpriteSheetCharacterNamingMode.OnlyControlPath => controlPath,
                SpriteSheetCharacterNamingMode.SpriteSheetNameThenUnderscoreThenControlPath => $"{spriteAsset.name}_{controlPath}",
                _ => controlPath
            };
            
            //check if the sprite asset has a matching sprite
            if (spriteAsset.spriteCharacterTable.AsValueEnumerable().FirstOrDefault(c => c.name == formattedControlPath) == null)
            {
                Debug.LogWarning($"Sprite Sheet {spriteAsset.name} does not have a character entry named {formattedControlPath}.", this);
                return controlPath;
            }
            
            return $"<sprite=\"{spriteAsset.name}\" name=\"{formattedControlPath}\">";
        }
    }
}
