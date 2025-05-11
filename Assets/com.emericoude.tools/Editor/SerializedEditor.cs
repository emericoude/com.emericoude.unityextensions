using UnityEditor;
using UnityEngine;

namespace Emericoude.CustomEditors
{
    public abstract class SerializedEditor : Editor
    {
        protected abstract string EditorPrefKey { get; }

        protected virtual void OnEnable() {
            string serializedData = EditorPrefs.GetString(this.EditorPrefKey, JsonUtility.ToJson(this, false));
            JsonUtility.FromJsonOverwrite(serializedData, this);
        }

        protected virtual  void OnDisable() {
            string serializedData = JsonUtility.ToJson(this, false);
            EditorPrefs.SetString(this.EditorPrefKey, serializedData);
        }
    }
}