using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using ZLinq;

namespace Emericoude.Framework
{
    public class SinglePlayerDeviceHandler : LazySingleton<SinglePlayerDeviceHandler>
    {
        public InputDevice ActiveDevice { get; private set; }
        public InputControlScheme? ActiveControlScheme { get; private set; }

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
                    this.ActiveDevice = user.pairedDevices[0];
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