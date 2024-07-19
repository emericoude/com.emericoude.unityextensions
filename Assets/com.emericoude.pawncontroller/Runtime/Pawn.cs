using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Emericoude.Gameplay.PawnController
{
    [SelectionBase]
    public abstract class Pawn : MonoBehaviour
    {
        public event Action OnPossessed;
        public event Action OnUnpossessed;

        [Header("Settings")] 
        [SerializeField] private InputActionMap actionMap;
        
        public Controller Controller { get; private set; }
        public InputActionMap ActionMap => actionMap;

        internal virtual void Possess(Controller controller)
        {
            this.Controller = controller;
            this.OnPossessed?.Invoke();
        }

        internal virtual void Unpossess()
        {
            this.OnUnpossessed?.Invoke();
            this.Controller = null;
        }
        
        public virtual void SetupInputActions(PlayerInput playerInput) { }
    }
}
