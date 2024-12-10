using System;

using UnityEngine;

namespace Emericoude.Gameplay.PawnController
{
    [SelectionBase]
    public abstract class Controller : MonoBehaviour
    {
        /// <summary> Event invoked at the end of the possession process. Passing the new pawn. </summary>
        public event Action<Pawn> OnPossessPawn;
        
        /// <summary> Event invoked at the end of the unpossession process. Passing the old pawn. </summary>
        public event Action<Pawn> OnUnpossessPawn;
        
        /// <summary> The currently possessed pawn. </summary>
        public Pawn Pawn { get; private set; }
        
        public virtual bool IsPlayer() => this is PlayerController;
        public virtual bool IsAI() => this is not PlayerController;

        /// <summary> Possess a pawn. If one is already possessed, unpossess that one first. </summary>
        /// <param name="pawn"> The new pawn to possess. </param>
        public virtual void PossessPawn(Pawn pawn)
        {
            if (pawn.Controller != null)
            {
                pawn.Controller.UnpossessCurrentPawn();
            }
            
            if (this.Pawn != null)
            {
                this.UnpossessCurrentPawn();
            }
            
            this.Pawn = pawn;
            this.Pawn.RegisterControllerPossession(this);
            this.OnPossessPawn?.Invoke(this.Pawn);
        }

        /// <summary> Unpossess the currently possessed pawn.</summary>
        public virtual void UnpossessCurrentPawn()
        {
            var previousPawn = this.Pawn;
            this.Pawn.UnregisterPossessingController();
            this.Pawn = null;
            
            OnUnpossessPawn?.Invoke(previousPawn);
        }
    }
}
