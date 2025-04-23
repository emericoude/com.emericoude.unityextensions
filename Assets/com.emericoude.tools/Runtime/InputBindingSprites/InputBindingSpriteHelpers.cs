using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using ZLinq;

namespace Emericoude
{
    /// <summary>
    /// Helpers for getting input bindings to rich text. In other words, this is a framework to get
    /// input prompts/hints displayed as sprites (or 'emojis' through rich text).
    /// </summary>
    public static class InputBindingSpriteHelpers
    {
        #region Binding to Sprite Rich Text
        
        public enum RichTextCompositeHandling
        {
            /// <summary> Will return a single piece of string that contains multiple sprites. </summary>
            EachBindingSpriteSideBySide,
            /// <summary> Will use the name of the composite (as written in the InputActionAsset) for lookup in the sprite asset. </summary>
            UseCompositeNameForLookup
        }

        /// <summary>
        /// Fetches the sprite matching the binding used by the user during the callback context.
        /// Use this with a PlayerInput component that is set to be used with events.
        /// </summary>
        /// <remarks> Basically just uses <see cref="GetRichTextSpriteForActiveControl"/>>. </remarks>
        /// <returns> The sprite formatted for rich text; or, if the sprite could not be found, the InputAction.GetBindingDisplayString(). </returns>
        public static string GetRichTextSpriteForActionInContext(
            this InputBindingSpriteAssetList spriteAssetList,
            InputAction.CallbackContext context
        )
        {
            return GetRichTextSpriteForActiveControl(spriteAssetList, context.action);
        }

        /// <summary>
        /// Fetches a sprite matching the binding used by the user in an ACTIVE context (i.e. while the action is in an 'active' state).
        /// An action is active while its being used, such as inside a InputAction.CallbackContext from an event.
        /// This means that this will not work unless the action was active this frame.
        /// See Action.GetBindingIndexForControl for more info.
        /// <para/>
        /// Generally, only use this if you want the binding to show in a responsive manner (which is in-of-itself a rare use-case).
        /// You may want to look into <see cref="GetRichTextSpriteForActionControlMatch"/> instead, which will give you the possible binding options instead.
        /// </summary>
        /// <returns> The sprite formatted for rich text; or, if the sprite could not be found, the InputAction.GetBindingDisplayString(). </returns>
        public static string GetRichTextSpriteForActiveControl(
            this InputBindingSpriteAssetList spriteAssetList,
            InputAction action
        )
        {
            InputControl control = action.activeControl;
            Type deviceType = control.device.GetType();
            int bindingIndex = action.GetBindingIndexForControl(control);
            return ProcessBindingIndexToRichTextSprite(spriteAssetList, action, deviceType, bindingIndex);
        }

        /// <summary>
        /// Tries to fetch a valid sprite (or series of sprites in some contexts) from the binding(s) associated with
        /// the given control, and then formats them into a rich text formats compatible with TextMeshPro.
        /// </summary>
        /// <param name="spriteAssetList"> The asset containing your sprite associated to device types. </param>
        /// <param name="action"> The action in which you want to find matching bindings. </param>
        /// <param name="control"> The control (or device) with which you want to find matching bindings. </param>
        /// <param name="compositeHandling"> How to handle composite bindings (e.g. WASD for movement). See <see cref="RichTextCompositeHandling"/> for more details. </param>
        /// <param name="compositeSeparator"> If your binding is a composite, and you use the CompositeHandlingMethods.EachBindingSpriteSideBySide method, how do you separate each binding. Empty by default. </param>
        /// <returns> The sprite(s) formatted for rich text; or, if a sprite could not be found, the InputAction.GetBindingDisplayString(). </returns>
        public static string GetRichTextSpriteForActionControlMatch(
            this InputBindingSpriteAssetList spriteAssetList, 
            InputAction action,
            InputControl control,
            RichTextCompositeHandling compositeHandling = RichTextCompositeHandling.EachBindingSpriteSideBySide,
            string compositeSeparator = ""
        )
        {
            //First, we try to find a set of matching binding indexes for the given control
            if (!action.TryGetBindingIndexMatchesForControl(out var bindingIndexMatches, control))
            {
                //We did not find a direct match, let's look for matching control scheme, in which case we can look into its other devices (if any)
                if (!action.TryGetMatchingControlScheme(out var controlSchemeMatch, control) || controlSchemeMatch.deviceRequirements.Count == 1)
                {
                    Debug.LogWarning($"Could not find a matching binding or control scheme in the {action.name} action using a {control.device.GetType()} device", spriteAssetList);
                    return action.GetBindingDisplayString();
                }
                
                int indexOfInitialControl = controlSchemeMatch.deviceRequirements.IndexOf(dr => dr.controlPath == control.device.path);
                for (int i = 0; i < controlSchemeMatch.deviceRequirements.Count; i++)
                {
                    if (i == indexOfInitialControl) continue;
                    var controlMatches = InputSystem.FindControls(controlSchemeMatch.deviceRequirements[i].controlPath);
                    foreach (var controlMatch in controlMatches)
                    {
                        if (action.TryGetBindingIndexMatchesForControl(out bindingIndexMatches, controlMatch))
                        {
                            break;
                        }
                    }

                    if (bindingIndexMatches.Count > 0) break; //no need to keep looking if we found a fallback
                }

                if (bindingIndexMatches.Count == 0)
                {
                    Debug.LogWarning($"Could not find a matching binding or inside control scheme(s) in the {action.name} action using a {control.device.GetType()} device", spriteAssetList);
                    return action.GetBindingDisplayString();
                }
            }
            
            var inputDeviceType = control.device.GetType();
            if (action.bindings[bindingIndexMatches[0]].isPartOfComposite)
            {
                return compositeHandling switch
                {
                    RichTextCompositeHandling.UseCompositeNameForLookup => ProcessBindingIndexToRichTextSprite(spriteAssetList, action, inputDeviceType, bindingIndexMatches[0] - 1, true),
                    RichTextCompositeHandling.EachBindingSpriteSideBySide => bindingIndexMatches
                        .AsValueEnumerable()
                        .Select(bindingIndex => ProcessBindingIndexToRichTextSprite(spriteAssetList, action, inputDeviceType, bindingIndex))
                        .JoinToString(compositeSeparator),
                    _ => throw new ArgumentOutOfRangeException(nameof(compositeHandling), compositeHandling, null)
                };
            }

            return ProcessBindingIndexToRichTextSprite(spriteAssetList, action, inputDeviceType, bindingIndexMatches[0]);
        }

        //utility function that processes an action's binding (by index) to a formatted rich text sprite
        private static string ProcessBindingIndexToRichTextSprite(
            InputBindingSpriteAssetList spriteAssetList,
            InputAction action, 
            Type inputDeviceType, 
            int bindingIndex,
            bool useRawName = false)
        {
            if (bindingIndex == -1) return action.GetBindingDisplayString();
            string displayStringFallback = action.GetBindingDisplayString(bindingIndex, out string deviceLayout, out string controlPath);

            //check for direct type references (inheritance not taken into account)
            if (!spriteAssetList.SpriteAssetsByType.TryGetValue(inputDeviceType, out var spriteAssetMatch))
            {
                //if we did not find a direct type, we look for parent types
                spriteAssetMatch = spriteAssetList.SpriteAssetsByType
                    .AsValueEnumerable()
                    .FirstOrDefault(kvp => inputDeviceType.IsSubclassOf(kvp.Key))
                    .Value;
            }

            if (spriteAssetMatch == null)
            {
                Debug.LogWarning($"Could not find a sprite asset for the {inputDeviceType} device type (or any parent types)", spriteAssetList);
                return displayStringFallback;
            }

            string spriteName = (useRawName ? action.bindings[bindingIndex].name : controlPath).ToLower();
            if (!spriteAssetMatch.spriteCharacterTable.AsValueEnumerable().Any(sp => sp.name.ToLower() == spriteName))
            {
                Debug.LogWarning($"Sprite Asset {spriteAssetMatch.name} does not have a character entry named {spriteName}.", spriteAssetMatch);
                return displayStringFallback;
            }
            
            return $"<sprite=\"{spriteAssetMatch.name}\" name=\"{spriteName}\">";
        }

        #endregion
        #region Binding to Sprite

        public enum SpriteCompositeHandling
        {
            /// <summary> Will return a list of sprites for each binding. </summary>
            SpriteListForEachBinding,
            /// <summary> Will use the name of the composite for lookup in the sprite asset, and try to return a single sprite. </summary>
            UseCompositeNameForSingleSprite
        }
        
        /// <summary>
        /// Fetches the sprite matching the binding used by the user during the callback context.
        /// Use this with a PlayerInput component that is set to be used with events.
        /// </summary>
        /// <remarks> Basically just uses <see cref="GetSpriteForActiveControl"/>>. </remarks>
        /// <returns> The sprite if found; otherwise null. </returns>
        public static Sprite GetSpriteForActionInContext(
            this InputBindingSpriteAssetList spriteAssetList,
            InputAction.CallbackContext context
        )
        {
            return GetSpriteForActiveControl(spriteAssetList, context.action);
        }

        /// <summary>
        /// Fetches a sprite matching the binding used by the user in an ACTIVE context (i.e. while the action is in an 'active' state).
        /// An action is active while its being used, such as inside a InputAction.CallbackContext from an event.
        /// This means that this will not work unless the action was active this frame.
        /// See Action.GetBindingIndexForControl for more info.
        /// <para/>
        /// Generally, only use this if you want the binding to show in a responsive manner (which is in-of-itself a rare use-case).
        /// You may want to look into <see cref="GetSpritesForActionControlMatch"/> instead, which will give you the possible binding options instead.
        /// </summary>
        /// <returns> The sprite if found; otherwise null. </returns>
        public static Sprite GetSpriteForActiveControl(
            this InputBindingSpriteAssetList spriteAssetList,
            InputAction action
        )
        {
            InputControl control = action.activeControl;
            Type deviceType = control.device.GetType();
            int bindingIndex = action.GetBindingIndexForControl(control);
            return ProcessBindingIndexToSprite(spriteAssetList, action, deviceType, bindingIndex);
        }
        
        /// <summary>
        /// Tries to fetch a valid sprite (or series of sprites in some contexts) from the binding(s) associated with the given control.
        /// </summary>
        /// <param name="spriteAssetList"> The asset containing your sprite associated to device types. </param>
        /// <param name="action"> The action in which you want to find matching bindings. </param>
        /// <param name="control"> The control (or device) with which you want to find matching bindings. </param>
        /// <param name="compositeHandling"> How to handle composite bindings (e.g. WASD for movement). See <see cref="SpriteCompositeHandling"/> for more details. </param>
        /// <returns> The sprite(s) found. We trim null references, so the list could be empty. </returns>
        public static List<Sprite> GetSpritesForActionControlMatch(
            this InputBindingSpriteAssetList spriteAssetList, 
            InputAction action,
            InputControl control,
            SpriteCompositeHandling compositeHandling = SpriteCompositeHandling.SpriteListForEachBinding
        )
        {
            //First, we try to find a set of matching binding indexes for the given control
            if (!action.TryGetBindingIndexMatchesForControl(out var bindingIndexMatches, control))
            {
                //We did not find a direct match, let's look for matching control scheme, in which case we can look into its other devices (if any)
                if (!action.TryGetMatchingControlScheme(out var controlSchemeMatch, control) || controlSchemeMatch.deviceRequirements.Count == 1)
                {
                    Debug.LogWarning($"Could not find a matching binding or control scheme in the {action.name} action using a {control.device.GetType()} device", spriteAssetList);
                    return new List<Sprite>();
                }
                
                int indexOfInitialControl = controlSchemeMatch.deviceRequirements.IndexOf(dr => dr.controlPath == control.device.path);
                for (int i = 0; i < controlSchemeMatch.deviceRequirements.Count; i++)
                {
                    if (i == indexOfInitialControl) continue;
                    var controlMatches = InputSystem.FindControls(controlSchemeMatch.deviceRequirements[i].controlPath);
                    foreach (var controlMatch in controlMatches)
                    {
                        if (action.TryGetBindingIndexMatchesForControl(out bindingIndexMatches, controlMatch))
                        {
                            break;
                        }
                    }

                    if (bindingIndexMatches.Count > 0) break; //no need to keep looking if we found a fallback
                }

                if (bindingIndexMatches.Count == 0)
                {
                    Debug.LogWarning($"Could not find a matching binding or inside control scheme(s) in the {action.name} action using a {control.device.GetType()} device", spriteAssetList);
                    return new List<Sprite>();
                }
            }
            
            var inputDeviceType = control.device.GetType();
            if (action.bindings[bindingIndexMatches[0]].isPartOfComposite)
            {
                return compositeHandling switch
                {
                    SpriteCompositeHandling.UseCompositeNameForSingleSprite => new List<Sprite>() { ProcessBindingIndexToSprite(spriteAssetList, action, inputDeviceType, bindingIndexMatches[0] - 1, true) },
                    SpriteCompositeHandling.SpriteListForEachBinding => bindingIndexMatches
                        .AsValueEnumerable()
                        .Select(bindingIndex => ProcessBindingIndexToSprite(spriteAssetList, action, inputDeviceType, bindingIndex))
                        .Where(sprite => sprite != null)
                        .ToList(),
                    _ => throw new ArgumentOutOfRangeException(nameof(compositeHandling), compositeHandling, null)
                };
            }

            return new List<Sprite>() { ProcessBindingIndexToSprite(spriteAssetList, action, inputDeviceType, bindingIndexMatches[0]) };
        }
        
        private static Sprite ProcessBindingIndexToSprite(
            InputBindingSpriteAssetList spriteAssetList,
            InputAction action, 
            Type inputDeviceType, 
            int bindingIndex,
            bool useRawName = false)
        {
            if (bindingIndex == -1) return null;
            action.GetBindingDisplayString(bindingIndex, out string deviceLayout, out string controlPath);
            
            //check for direct type references (inheritance not taken into account)
            if (!spriteAssetList.SpriteAssetsByType.TryGetValue(inputDeviceType, out var spriteAssetMatch))
            {
                //if we did not find a direct type, we look for parent types
                spriteAssetMatch = spriteAssetList.SpriteAssetsByType
                    .AsValueEnumerable()
                    .FirstOrDefault(kvp => inputDeviceType.IsSubclassOf(kvp.Key))
                    .Value;
            }

            if (spriteAssetMatch == null)
            {
                Debug.LogWarning($"Could not find a sprite asset for the {inputDeviceType} device type (or any parent types)", spriteAssetList);
                return null;
            }

            string spriteName = (useRawName ? action.bindings[bindingIndex].name : controlPath).ToLower();
            var matchingSpriteCharacter = spriteAssetMatch.spriteCharacterTable.AsValueEnumerable().FirstOrDefault(sp => sp.name.ToLower() == spriteName);
            if (matchingSpriteCharacter == null)
            {
                Debug.LogWarning($"Sprite Asset {spriteAssetMatch.name} does not have a character entry named {spriteName}.", spriteAssetMatch);
                return null;
            }

            return spriteAssetMatch.spriteGlyphTable[(int)matchingSpriteCharacter.glyph.index].sprite;
        }
        
        #endregion

        /// <summary>
        /// Tries to get a matching control scheme for a given control (or device). If multiple control schemes
        /// can use the device, this does not guarantee which will be picked.
        /// </summary>
        /// <remarks> This only works if the action is part of a InputActionAsset. </remarks>
        /// <returns> True if a matching control scheme was found; otherwise false. The out control scheme is written to in either cases (though default if false). </returns>
        public static bool TryGetMatchingControlScheme(
            this InputAction action,
            out InputControlScheme controlScheme, 
            InputControl control)
        {
            var nullableControlScheme = InputControlScheme.FindControlSchemeForDevice(control.device, action.actionMap.controlSchemes);
            controlScheme = nullableControlScheme.GetValueOrDefault();
            return nullableControlScheme != null;
        }

        /// <summary>
        /// Tries to find bindings that can be used by the given control in this action.
        /// Returns more than one entry in the context where the binding is a composite.
        /// </summary>
        /// <returns> True if matching bindings were found; otherwise false. </returns>
        public static bool TryGetBindingIndexMatchesForControl(this InputAction action, out List<int> bindingIndexes, InputControl control)
        {
            bindingIndexes = new List<int>();
            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (!InputControlPath.Matches(action.bindings[i].effectivePath, control)) continue;
                
                bindingIndexes.Add(i);
                if (!action.bindings[i].isPartOfComposite)
                {
                    break;
                }
            }

            return bindingIndexes.Count > 0;
        }
    }
}