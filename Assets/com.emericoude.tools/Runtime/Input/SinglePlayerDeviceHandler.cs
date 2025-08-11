using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using ZLinq;

namespace Emericoude.Framework
{
    /// <summary>
    /// A simple utility to fetch the last device or control scheme used by the current user.
    /// TODO: This is kind of specific and doesn't have a strong identity yet.
    /// </summary>
    public class SinglePlayerDeviceHandler : LazySingleton<SinglePlayerDeviceHandler>
    {
        public Action<InputDevice> OnDeviceChanged;
        public Action<InputControlScheme> OnControlSchemeChanged;

        private InputDevice activeDevice;
        public InputDevice ActiveDevice
        {
            get => activeDevice ??= GetLastDeviceUsed();
            private set
            {
                if (activeDevice == value) return;
                activeDevice = value;
                OnDeviceChanged?.Invoke(activeDevice);
            }
        }

        private InputControlScheme? activeControlScheme;
        public InputControlScheme? ActiveControlScheme
        {
            get => activeControlScheme ??= InputUser.all[0].controlScheme;
            private set
            {
                if (activeControlScheme == value) return;
                activeControlScheme = value;
                if (activeControlScheme != null)
                {
                    OnControlSchemeChanged?.Invoke(activeControlScheme.Value);
                }
            }
        }

        private void OnEnable()
        {
            InputUser.onChange += OnInputUserChange;
        }

        private void OnDisable()
        {
            InputUser.onChange -= OnInputUserChange;
        }

        private void OnInputUserChange(InputUser user, InputUserChange change, InputDevice device)
        {
            switch (change)
            {
                case InputUserChange.ControlsChanged:
                case InputUserChange.ControlSchemeChanged:
                    this.ActiveControlScheme = user.controlScheme;
                    break;
                case InputUserChange.DeviceUnpaired:
                case InputUserChange.DeviceLost:
                    this.ActiveDevice = user.pairedDevices.Count > 0 
                        ? user.pairedDevices
                            .AsValueEnumerable()
                            .Where(d => d.enabled)
                            .MaxBy(d => d.lastUpdateTime) 
                        : null;
                    break;
                case InputUserChange.DeviceRegained:
                case InputUserChange.DevicePaired:
                    this.ActiveDevice = device;
                    break;
            }
        }
        
        public InputDevice GetLastDeviceUsed() => InputSystem.devices.AsValueEnumerable().MaxBy(d => d.lastUpdateTime);
    }
}