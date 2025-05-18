using System;
using Emericoude.UI.ProjectedCanvas;
using UnityEngine;

namespace Emericoude.Tests
{
    public class Test_ProjectedCanvas : MonoBehaviour
    {
        [SerializeField] private ProjectedCanvas projectedCanvas;
        public RectTransform syncCanvasObject;
        public Transform syncWorldSpaceObject;

        private void Update()
        {
            if (this.projectedCanvas.TryCanvasToMeshPoint(syncCanvasObject.position, out Vector3 newWorldPosition, 0, 0))
            {
                syncWorldSpaceObject.position = newWorldPosition;
            }
        }
    }
}
