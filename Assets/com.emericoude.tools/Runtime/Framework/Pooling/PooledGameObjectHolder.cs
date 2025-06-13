using System;
using System.Collections.Generic;

using UnityEngine;

namespace Emericoude.Framework
{
    public abstract class PooledGameObjectHolder : MonoBehaviour
    {
        public abstract bool IsReadyForRelease();
    }
}