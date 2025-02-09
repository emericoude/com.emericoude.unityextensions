using UnityEngine;

namespace Emericoude.Tests
{
    public class ShapeGeneratorTests : MonoBehaviour
    {
        [Header("Cone")]
        [SerializeField] private float coneRadius = 1;
        [SerializeField] private float coneHeight = 1;
        [SerializeField] private Vector3 coneUp = Vector3.up;
        [SerializeField] private int coneSubdivisions = 64;
        [SerializeField] private bool coneTipIsPivot = false;
        
        private Mesh cone;
        
        private void Start()
        {
            this.GenerateShapes();
        }

        private void OnValidate()
        {
            if (!Application.isPlaying) return;
            this.GenerateShapes();
        }

        private void GenerateShapes()
        {
            this.cone = ShapeGenerator.Cone(this.coneRadius, this.coneHeight, this.coneUp, this.coneSubdivisions, this.coneTipIsPivot);
            //add shapes here
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawMesh(this.cone, this.transform.position, this.transform.rotation);
        }
    }
}
