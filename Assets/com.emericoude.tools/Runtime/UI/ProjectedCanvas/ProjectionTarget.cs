using UnityEngine;

namespace Emericoude.UI.ProjectedCanvas
{
    /// <summary>
    /// A target used by a ProjectedCanvas to be projected onto.
    /// You can manage the interactable state on a per-object basis here.
    /// </summary>
    /// <remarks> For the Event System's Raycast to go through, it requires its projection targets to have Mesh Colliders, so that they may return a texture coordinate <see href="https://docs.unity3d.com/ScriptReference/RaycastHit-textureCoord2.html"/>. </remarks>
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class ProjectionTarget : MonoBehaviour
    {
        [SerializeField] private bool m_Interactable = true;
        public bool Interactable {
            get => this.m_Interactable && this.isActiveAndEnabled;
            set => this.m_Interactable = value;
        }
        
        [SerializeField] private MeshRenderer m_MeshRenderer;
        [SerializeField] private MeshCollider m_MeshCollider;

        private void Reset() {
            this.m_MeshRenderer = this.GetComponent<MeshRenderer>();
            this.m_MeshCollider = this.GetComponent<MeshCollider>();
        }
    }
}