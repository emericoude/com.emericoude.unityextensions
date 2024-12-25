using UnityEngine;
using UnityEngine.InputSystem;

namespace Emericoude.Pawns
{
    [SelectionBase, RequireComponent(typeof(PlayerInput))] 
    public class PlayerController : Controller
    {
        private enum InputActionAssetPriority
        {
            [Tooltip("The player manages input mapping.")]
            Player = 0,
            [Tooltip("Pawns can override input mappings. Useful when you have many pawns with different control schemes.")]
            Pawn = 1
        }

        [Header("Settings")]
        [Tooltip("The default pawn for this player. If assigned, the pawn is automatically possessed on start.")]
        [SerializeField] private Pawn defaultPawn;
        
        [Header("Settings: Input")]
        [Tooltip("The player's 'Player Input' component.")]
        [SerializeField] private PlayerInput playerInput;
        [Tooltip("Whether to prioritize input mapping management in the player or from pawns.")]
        [SerializeField] private InputActionAssetPriority actionAssetPriority = InputActionAssetPriority.Player;

        public PlayerInput Inputs => this.playerInput;

        private InputActionAsset defaultPlayerActionAsset;

        private void Awake()
        {
            this.defaultPlayerActionAsset = this.playerInput.actions;
        }

        protected virtual void Start()
        {
            if (this.defaultPawn != null)
            {
                this.PossessPawn(this.defaultPawn);
            }
        }

        public override void PossessPawn(Pawn pawn)
        {
            base.PossessPawn(pawn);

            if (this.actionAssetPriority == InputActionAssetPriority.Pawn && pawn.InputActionAsset != null)
            {
                this.playerInput.actions = pawn.InputActionAsset;
                this.playerInput.SwitchCurrentActionMap(pawn.DefaultActionMap);
            }
            
            this.SetupInputBindings();
            pawn.SetupInputActions(this.playerInput);
        }
        
        public override void UnpossessCurrentPawn()
        {
            base.UnpossessCurrentPawn();
            
            if (this.actionAssetPriority == InputActionAssetPriority.Pawn && this.playerInput.actions != this.defaultPlayerActionAsset)
            {
                this.playerInput.actions = this.defaultPlayerActionAsset;
                this.playerInput.SwitchCurrentActionMap(this.playerInput.defaultActionMap);
            }
        }

        protected virtual void SetupInputBindings() { }
    }
}
