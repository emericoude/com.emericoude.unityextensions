using Emericoude.Gameplay;
using UnityEditor;
using UnityEngine;

namespace Emericoude.EditorExtensions
{
    [CustomEditor(typeof(Billboard))]
    public class BillboardEditor : Editor
    {
        private void OnSceneGUI()
        {
            if (Tools.current != Tool.Move) return;
            if (Application.isPlaying) return;
            
            var billboard = target as Billboard;

            if (!billboard) return;
            if (billboard.worldSpaceOffset == Vector3.zero) return;

            var transform = billboard.transform;
            
            //offset handles
            var offsetPosition = Handles.PositionHandle(transform.position + billboard.worldSpaceOffset, transform.rotation);
            billboard.worldSpaceOffset = offsetPosition - transform.position;
            
            //draw line between the two
            Handles.DrawDottedLine(transform.position, offsetPosition, 6);
        }
    }
}