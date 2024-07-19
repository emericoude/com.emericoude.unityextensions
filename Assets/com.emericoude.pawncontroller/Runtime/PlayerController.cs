using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Emericoude.Gameplay.PawnController
{
    [SelectionBase, RequireComponent(typeof(PlayerInput))] 
    public class PlayerController : Controller
    {
        enum InputMappingPriority
        {
            [Tooltip("The player manages input mapping.")]
            Player = 0,
            [Tooltip("Pawns can override input mappings. Useful when you have many pawns with different control schemes.")]
            Pawn = 1
        }

        [Header("Settings")] 
        [SerializeField] private Pawn defaultPawn;
        
        [Header("Settings: Input")]
        [Tooltip("The player's 'Player Input' component.")]
        [SerializeField] private PlayerInput playerInput;
        [Tooltip("Whether to prioritize input mapping management in the player or from pawns.")]
        [SerializeField] private InputMappingPriority mappingPriority = InputMappingPriority.Player;

        public PlayerInput Inputs => playerInput;

        protected virtual void Awake()
        {
            if (defaultPawn != null)
            {
                PossessPawn(defaultPawn);
            }
        }

        public override void PossessPawn(Pawn pawn)
        {
            base.PossessPawn(pawn);

            if (pawn.ActionMap != null)
            {
                if (mappingPriority == InputMappingPriority.Pawn)
                {
                    playerInput.currentActionMap = pawn.ActionMap;
                }
            }
            
            this.SetupInputBindings();
            pawn.SetupInputActions(this.playerInput);
        }

        protected virtual void SetupInputBindings() { }
    }
}
