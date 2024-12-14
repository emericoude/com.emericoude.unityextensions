using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Emericoude.Gameplay.PawnController
{
    [SelectionBase]
    public abstract class Pawn : MonoBehaviour
    {
        /// <summary> Invoked when the pawn is possessed. We pass in the new controller. </summary>
        public event Action<Controller> OnPossessed;
        
        /// <summary> Invoked when the pawn is unpossessed. We pass in the old controller. </summary>
        public event Action<Controller> OnUnpossessed;

        /// <summary> The owning controller. </summary>
        public Controller Controller { get; protected set; }
        
        internal InputActionAsset InputActionAsset => this.inputActionAsset;
        internal string DefaultActionMap => this.defaultActionMap;

        [Header("Settings")]
        [Tooltip("Used by a PlayerController whose 'InputActionAssetPriority' is set to 'Pawn'. If assigned to this pawn, we will instead use this action asset and default map.")]
        [SerializeField] private InputActionAsset inputActionAsset;
        [Tooltip("Used by a PlayerController whose 'InputActionAssetPriority' is set to 'Pawn'. If assigned to this pawn, we will instead use this action asset and default map.")]
        [SerializeField] private string defaultActionMap;

        /// <summary>
        /// Registers a <c>Controller</c> taking control of this pawn. <para/>
        /// Stores the <see cref="Controller"/>, then invokes <see cref="OnPossessed"/> passing the new controller.
        /// </summary>
        /// <remarks> Possessing should be done through the controller. See <see cref="Controller"/>.<see cref="Controller.PossessPawn(Pawn)"/> for more info. </remarks>
        /// <param name="controller"> The controller taking control. </param>
        public virtual void RegisterControllerPossession(Controller controller)
        {
            this.Controller = controller;
            this.OnPossessed?.Invoke(controller);
        }

        /// <summary>
        /// Unregisters the current <see cref="Controller"/> controlling this pawn. <para/>
        /// Removes the reference to <see cref="Controller"/>, then invokes <see cref="OnUnpossessed"/> passing the old controller. 
        /// </summary>
        /// <remarks> Unpossessing should be done through the controller. See <see cref="Controller"/>.<see cref="Controller.UnpossessCurrentPawn()"/> for more info. </remarks>
        public virtual void UnregisterPossessingController()
        {
            var previousController = this.Controller;
            this.Controller = null;
            
            this.OnUnpossessed?.Invoke(previousController);
        }
        
        public virtual void SetupInputActions(PlayerInput playerInput) { }
        public virtual void ClearInputActions(PlayerInput playerInput) { }
    }
}
