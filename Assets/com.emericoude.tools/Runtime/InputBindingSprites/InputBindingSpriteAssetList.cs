using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Emericoude
{
    /// <summary>
    /// This is simply a holder that associates sprite assets to specific devices.
    /// Most important functions are inside <see cref="InputBindingSpriteHelpers"/>;
    /// <para/>
    /// Put the most-specific device type at the top of the list if you want them to be prioritized.
    /// For example, "Gamepad" should be below "XInputController".
    /// <para/>
    /// The utilities used by this expect each sprite character to be named according to the expected binding's path.
    /// For instance, if you have a sprite for the left joystick of a gamepad in your sprite asset, the associated character
    /// entry should be named "leftStick". Follow the readme associated with the utility to get more information on setup.
    /// </summary>
    /// <remarks>
    /// TODO: Known limitations:
    /// <br/> - Because not all devices have specific InputDevice classes in Unity (for example, Steam Deck or Nintendo Joycons do not have
    /// a specific type), this system has expected limitations.
    /// I wonder if it is possible to implement custom devices, otherwise we may need a more precise alternative to this.
    /// <br/> - There are cases where you'd would want to give the user the ability to choose which gamepad button types to display
    /// (usually between Xbox, PlayStation, Nintendo and Steam), in which case we'd need a way to enforce that.
    /// </remarks>
    [CreateAssetMenu(menuName = "Emericoude/Input Binding Sprite Asset List", fileName = "New Input Binding Sprite Asset List", order = 0)]
    public class InputBindingSpriteAssetList : ScriptableObject
    {
        [Serializable]
        public struct SpriteAssetEntry
        {
            [SerializeReference] public InputDevice deviceType;
            public TMP_SpriteAsset spriteAsset;
        }

        [SerializeField] private List<SpriteAssetEntry> spriteAssetsSerialized = new List<SpriteAssetEntry>();
        public Dictionary<Type, TMP_SpriteAsset> SpriteAssetsByType { get; private set; }

        private void OnEnable()
        {
            SpriteAssetsByType = new Dictionary<Type, TMP_SpriteAsset>();
            foreach (var spriteAsset in spriteAssetsSerialized)
            {
                SpriteAssetsByType.Add(spriteAsset.deviceType.GetType(), spriteAsset.spriteAsset);
            }
        }
    }
}