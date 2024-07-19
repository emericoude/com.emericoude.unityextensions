using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Emericoude.Gameplay.PawnController
{
    [SelectionBase]
    public abstract class Controller : MonoBehaviour
    {
        public event Action<Pawn> OnPossess;
        public event Action<Pawn> OnUnpossess;
        
        public Pawn Pawn { get; private set; }

        public virtual void PossessPawn(Pawn pawn)
        {
            if (pawn.Controller != null)
            {
                pawn.Controller.UnpossessPawn();
            }
            
            if (this.Pawn != null)
            {
                this.UnpossessPawn();
            }
            
            this.Pawn = pawn;
            this.Pawn.Possess(this);
            this.OnPossess?.Invoke(this.Pawn);
        }

        public virtual void UnpossessPawn()
        {
            OnUnpossess?.Invoke(this.Pawn);
            this.Pawn.Unpossess();
            this.Pawn = null;
        }

        public virtual bool IsPlayer()
        {
            return this is PlayerController;
        }

        public virtual bool IsAI()
        {
            return this is not PlayerController;
        }
    }
}
