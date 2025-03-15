using UnityEngine;

namespace Emericoude.Tests
{
    public class DebugFromUnityEvent : MonoBehaviour
    {
        public void Log(string value)
        {
            Debug.Log(value, this);
        }
        
        public void LogWarning(string value)
        {
            Debug.LogWarning(value, this);
        }
        
        public void LogErrorError(string value)
        {
            Debug.Log(value, this);
        }
    }
}
