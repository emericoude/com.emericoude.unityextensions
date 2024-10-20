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
        
        //TODO: vet this system
        public InputActionMap ActionMap => actionMap;
        
        [Header("Settings")] 
        [SerializeField] private InputActionMap actionMap;

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
        
        //TODO: vet this system
        public virtual void SetupInputActions(PlayerInput playerInput) { }
    }
}
