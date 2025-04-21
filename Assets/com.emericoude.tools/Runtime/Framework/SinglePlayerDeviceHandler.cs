using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using ZLinq;

namespace Emericoude.Framework
{
    public class SinglePlayerDeviceHandler : LazySingleton<SinglePlayerDeviceHandler>
    {
        public Action<InputDevice> OnActiveDeviceChanged;

        public InputDevice ActiveDevice
        {
            get => _activeDevice;
            private set
            {
                if (_activeDevice == value) return;
                _activeDevice = value;
                OnActiveDeviceChanged?.Invoke(_activeDevice);
            }
        }
        
        public InputControlScheme? ActiveControlScheme { get; private set; }
        
        private InputDevice _activeDevice;

        protected void Awake()
        {
            this.ActiveDevice = GetLastDeviceUsed();
            this.ActiveControlScheme = InputUser.all[0].controlScheme;
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