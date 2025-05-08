using System;
using System.Collections.Generic;

using Emericoude.Helpers;

using UnityEngine;

using ZLinq;

namespace Emericoude.UI.ProjectedCanvas
{
    /// <summary>
    /// A canvas meant to be used for projection onto a mesh (using a Render Texture). Useful for something like an interactable monitor or a map.
    /// <para/> An ideal scenario for this is:
    /// <br/> 1. You want to have 3D curved monitors(s) which contain an interactable UI.
    /// <br/> 2. You set up a UI canvas as Screen space Camera as a child of a Camera which targets a RenderTexture.
    /// <br/> 3. This Render Texture is used in a material, which is in turn assigned to your monitors(s)'s mesh renderers.
    /// <br/> 4. You use this component to add interactivity to your screens.
    /// </summary>
    /// <remarks> For this to function, it requires its projection targets to have Mesh Colliders, so that they may return a texture coordinate <see href="https://docs.unity3d.com/ScriptReference/RaycastHit-textureCoord2.html"/>. </remarks>
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(Camera))]
    public class ProjectedCanvas : MonoBehaviour
    {
        //TODO: It may be nice to have a custom editor to auto-setup targets or something of the sort.
        
        public Canvas RenderTextureCanvas => this.renderTextureCanvas;
        public Camera RenderTextureCamera => this.RenderTextureCanvas.worldCamera;
        public Camera EventCamera {
            get => this.m_EventCamera ??= Camera.main;
            set => this.m_EventCamera = value;
        }
        
        [Header("Rendering")]
        [SerializeField] private Camera m_EventCamera;
        [SerializeField] private Canvas renderTextureCanvas;
        [SerializeField] private List<ProjectionTarget> projectionTargets = new List<ProjectionTarget>(1);

        [Header("Navigation")]
        [SerializeField] private bool isRaycastable = true;
        
        private ProjectedCanvasGraphicRaycaster raycaster;

        private void Reset() {
            this.renderTextureCanvas = this.GetComponent<Canvas>();
            this.renderTextureCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            this.renderTextureCanvas.planeDistance = 0f;
            this.renderTextureCanvas.worldCamera = this.GetComponent<Camera>();
            this.renderTextureCanvas.worldCamera.orthographic = true;
            this.renderTextureCanvas.worldCamera.nearClipPlane = 0f;
            this.renderTextureCanvas.worldCamera.farClipPlane = 1f;
            this.renderTextureCanvas.worldCamera.backgroundColor = new Color(1f, 1f, 1f, 0f);
            this.renderTextureCanvas.worldCamera.clearFlags = CameraClearFlags.Color;
        }

        private void Start() {
            this.SetRaycastable(this.isRaycastable);
        }

        public void SetRaycastable(bool value) {
            if (value && this.raycaster == null) {
                this.raycaster = this.GetOrAddComponent<ProjectedCanvasGraphicRaycaster>();
                this.raycaster.ScreenProjectedCanvas = this;
            }

            if (this.raycaster != null) {
                this.raycaster.enabled = value;
            }
            
            this.isRaycastable = value;
        }

        internal bool IsPhysicalHitValid(RaycastHit hit) {
            if (this.projectionTargets.Count == 0) return false;
            
            return this.projectionTargets
                .AsValueEnumerable()
                .Any(t => t.Interactable && t.transform == hit.transform);
        }

        public void AddProjectionTarget(ProjectionTarget target) {
            this.projectionTargets.Add(target);
        }

        public void RemoveProjectionTarget(ProjectionTarget target) {
            this.projectionTargets.Remove(target);
        }
    }
}
