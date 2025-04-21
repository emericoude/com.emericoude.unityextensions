using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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
        
        public enum CompositeHandlingMethods
        {
            AttachCompositeBindings,
            UseCompositeName
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

        #region  Rich Text Utilities
        
        /// <summary>
        /// Tries to fetch a valid sprite from an input action's ACTIVE binding. If no active binding are found, this will not work.
        /// Because of that, you must use this in a reactive sense, such as from the event listening to the input.
        /// </summary>
        /// <returns> If we managed to find a sprite from the input, the formatted rich text string that will point to the sprite; otherwise the display string for that binding. </returns>
        /// <remarks> Does not support composite. Use <see cref="GetActionRichTextForControl"/> instead. </remarks>
        public string GetActionRichTextForActiveControl(InputAction action)
        {
            InputControl activeControl = action.activeControl;
            Type deviceType = activeControl.device.GetType();
            
            //Note that this method will only take currently active bindings into consideration. This means that if the given control could
            //come from one of the bindings on the action but does not currently do so, the method still returns -1.
            int bindingIndex = action.GetBindingIndexForControl(activeControl);
            return this.GetBindingRichTextOrDisplayString(action, deviceType, bindingIndex);
        }

        /// <summary>
        /// Tries to fetch a valid sprite (or series of sprite in some contexts) from the bindings associated with the given control.
        /// </summary>
        /// <param name="action"> The input action were evaluating. </param>
        /// <param name="activeControl"> The control device currently in use. </param>
        /// <param name="compositeHandlingMethods"> How to handle composites, see <see cref="CompositeHandlingMethods"/>.
        /// By default, it is set to AttachCompositeBindings (as it will cover any combination by putting each sprite next to one another), but UseCompositeName is most likely preferable if you have a single sprite to combine name (for instance a sprite that contains WASD).</param>
        /// <param name="compositeSeparator"> Defines how we separate each sprite in the string for a composite. Only valid when using AttachCompositeBindings. Empty by default. </param>
        /// <returns> If we managed to find a sprite from the input, the formatted rich text string that will point to the sprite; otherwise the display string for that binding. </returns>
        public string GetActionRichTextForControl(InputAction action, InputControl activeControl, CompositeHandlingMethods compositeHandlingMethods = CompositeHandlingMethods.AttachCompositeBindings, string compositeSeparator = "")
        {
            List<int> bindingIndexes = new List<int>();
            bool isComposite = false;
            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (!InputControlPath.Matches(action.bindings[i].effectivePath, activeControl)) continue;
                if (action.bindings[i].isPartOfComposite)
                {
                    isComposite = true;
                    if (compositeHandlingMethods == CompositeHandlingMethods.UseCompositeName)
                    {
                        bindingIndexes.Add(i - 1);
                        break; //we only need the composite's name
                    }
                    
                    bindingIndexes.Add(i);
                }
                else
                {
                    bindingIndexes.Add(i);
                    break; //we are not a composite, we know we don't need to keep going
                }
            }
            
            //fallback in case no matching bindings were found
            if (bindingIndexes.Count == 0) return action.GetBindingDisplayString(); 
            
            Type inputDeviceType = activeControl.device.GetType();
            if (!isComposite) return this.GetBindingRichTextOrDisplayString(action, inputDeviceType, bindingIndexes[0]);

            return compositeHandlingMethods switch
            {
                CompositeHandlingMethods.AttachCompositeBindings => bindingIndexes
                    .AsValueEnumerable()
                    .Select(bindingIndex => this.GetBindingRichTextOrDisplayString(action, inputDeviceType, bindingIndex))
                    .JoinToString(compositeSeparator),
                CompositeHandlingMethods.UseCompositeName => this.GetBindingRichTextOrDisplayString(action, inputDeviceType, bindingIndexes[0], true),
                _ => throw new ArgumentOutOfRangeException(nameof(compositeHandlingMethods), compositeHandlingMethods, null)
            };
        }

        private string GetBindingRichTextOrDisplayString(InputAction action, Type inputDeviceType, int bindingIndex, bool useRawNameToLowerInsteadOfControlPath = false)
        {
            if (bindingIndex == -1) return action.GetBindingDisplayString();
            string displayStringFallback = action.GetBindingDisplayString(bindingIndex, out string deviceLayout, out string controlPath);

            if (!this.spriteAssetsByType.TryGetValue(inputDeviceType, out var spriteAsset)) 
            {
                //if we did not find a direct Type reference in the dictionary, try to find a parent type
                spriteAsset = spriteAssetsByType.AsValueEnumerable().FirstOrDefault(kvp => inputDeviceType.IsSubclassOf(kvp.Key)).Value;
                if (spriteAsset == null) //if we are still null, return a text-based fallback
                {
                    Debug.LogWarning($"Could not find a sprite asset for the device type (or parent type): {inputDeviceType}.", this);
                    return displayStringFallback;
                }
            }
            
            //format the control path, in case a user wants to use one or another naming convention
            string formattedControlPath = useRawNameToLowerInsteadOfControlPath ?
                action.bindings[bindingIndex].name
                : this.characterNamingMode switch
            {
                SpriteSheetCharacterNamingMode.OnlyControlPath => controlPath,
                SpriteSheetCharacterNamingMode.SpriteSheetNameThenUnderscoreThenControlPath => $"{spriteAsset.name}_{controlPath}",
                _ => controlPath
            };

            //safety net, could be considered a useless performance hit
            formattedControlPath =  formattedControlPath.ToLower();
        
            //check if the sprite asset has a matching sprite
            if (spriteAsset.spriteCharacterTable.AsValueEnumerable().FirstOrDefault(c => c.name.ToLower() == formattedControlPath) == null)
            {
                Debug.LogWarning($"Sprite Asset {spriteAsset.name} does not have a character entry named {formattedControlPath}.", this);
                return displayStringFallback;
            }
        
            return $"<sprite=\"{spriteAsset.name}\" name=\"{formattedControlPath}\">";
        }

        #endregion
        #region Get Sprite Utilities
        
        //untested
        public Sprite GetActionSpriteForActiveControl(InputAction action)
        {
            InputControl activeControl = action.activeControl;
            Type deviceType = activeControl.device.GetType();
            
            //Note that this method will only take currently active bindings into consideration. This means that if the given control could
            //come from one of the bindings on the action but does not currently do so, the method still returns -1.
            int bindingIndex = action.GetBindingIndexForControl(activeControl);
            return this.GetBindingSprite(action, deviceType, bindingIndex);
        }
        
        //untested
        public Sprite[] GetActionSpriteForControl(InputAction action, InputControl activeControl, CompositeHandlingMethods compositeHandlingMethods = CompositeHandlingMethods.AttachCompositeBindings)
        {
            List<int> bindingIndexes = new List<int>();
            bool isComposite = false;
            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (!InputControlPath.Matches(action.bindings[i].effectivePath, activeControl)) continue;
                if (action.bindings[i].isPartOfComposite)
                {
                    isComposite = true;
                    if (compositeHandlingMethods == CompositeHandlingMethods.UseCompositeName)
                    {
                        bindingIndexes.Add(i - 1);
                        break; //we only need the composite's name
                    }
                    
                    bindingIndexes.Add(i);
                }
                else
                {
                    bindingIndexes.Add(i);
                    break; //we are not a composite, we know we don't need to keep going
                }
            }
            
            //fallback in case no matching bindings were found
            if (bindingIndexes.Count == 0) return Array.Empty<Sprite>();
            
            Type inputDeviceType = activeControl.device.GetType();
            if (!isComposite) return new [] { this.GetBindingSprite(action, inputDeviceType, bindingIndexes[0]) };

            return compositeHandlingMethods switch
            {
                CompositeHandlingMethods.AttachCompositeBindings => bindingIndexes
                    .AsValueEnumerable()
                    .Select(bindingIndex => this.GetBindingSprite(action, inputDeviceType, bindingIndex))
                    .ToArray(),
                CompositeHandlingMethods.UseCompositeName => new [] { this.GetBindingSprite(action, inputDeviceType, bindingIndexes[0], true) },
                _ => throw new ArgumentOutOfRangeException(nameof(compositeHandlingMethods), compositeHandlingMethods, null)
            };
        }
        
        //untested
        private Sprite GetBindingSprite(InputAction action, Type inputDeviceType, int bindingIndex, bool useRawNameToLowerInsteadOfControlPath = false)
        {
            if (bindingIndex == -1) return null;
            action.GetBindingDisplayString(bindingIndex, out string deviceLayout, out string controlPath);

            if (!this.spriteAssetsByType.TryGetValue(inputDeviceType, out var spriteAsset)) 
            {
                //if we did not find a direct Type reference in the dictionary, try to find a parent type
                spriteAsset = spriteAssetsByType.AsValueEnumerable().FirstOrDefault(kvp => inputDeviceType.IsSubclassOf(kvp.Key)).Value;
                if (spriteAsset == null) //if we are still null, return a text-based fallback
                {
                    Debug.LogWarning($"Could not find a sprite asset for the device type (or parent type): {inputDeviceType}.", this);
                    return null;
                }
            }
            
            //format the control path, in case a user wants to use one or another naming convention
            string formattedControlPath = useRawNameToLowerInsteadOfControlPath ?
                action.bindings[bindingIndex].name
                : this.characterNamingMode switch
            {
                SpriteSheetCharacterNamingMode.OnlyControlPath => controlPath,
                SpriteSheetCharacterNamingMode.SpriteSheetNameThenUnderscoreThenControlPath => $"{spriteAsset.name}_{controlPath}",
                _ => controlPath
            };

            //safety net, could be considered a useless performance hit
            formattedControlPath =  formattedControlPath.ToLower();
        
            //check if the sprite asset has a matching sprite
            
            var spriteCharacter = spriteAsset.spriteCharacterTable.AsValueEnumerable().FirstOrDefault(c => c.name.ToLower() == formattedControlPath);
            if (spriteAsset == null)
            {
                Debug.LogWarning($"Sprite Asset {spriteAsset.name} does not have a character entry named {formattedControlPath}.", this);
                return null;
            }
            
            return spriteAsset.spriteGlyphTable[spriteCharacter.glyph.atlasIndex].sprite;
        }
        
        #endregion
    }
}
