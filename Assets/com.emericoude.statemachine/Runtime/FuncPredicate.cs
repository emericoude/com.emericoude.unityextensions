using System;

namespace Emericoude.StateMachines
{
    /// <summary> A simple predicate that can implement a Func returning a boolean. </summary>
    public class FuncPredicate : IPredicate
    {
        private readonly Func<bool> func;

        public FuncPredicate(Func<bool> func)
        {
            this.func = func;
        }

        public bool Evaluate() => this.func.Invoke();
    }
}