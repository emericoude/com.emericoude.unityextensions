using System.Linq;
using Emericoude.Helpers;
using UnityEngine;

/// <summary>
/// Helpers to generate shape meshes.
/// </summary>
public static class ShapeGenerator {
    #region Cone
    
    /// <summary> Generates a cone (by default, the tip is at the top and the pivot is at the bottom). </summary>
    /// <param name="radius"> The radius of the cone's base. </param>
    /// <param name="height"> The height of the cone's tip. </param>
    /// <param name="up"> What is "up". If it's not null or Vector3.up, we will rotate the mesh around its pivot point towards up. </param>
    /// <param name="subdivisions"> The resolution of the mesh, 64 by default. </param>
    /// <param name="tipIsPivot"> If true, the cone will be inverted. Useful for instance if you're representing a cone of vision. </param>
    /// <returns> A conical mesh. </returns>
    public static Mesh Cone(float radius, float height, Vector3? up = null, int subdivisions = 64, bool tipIsPivot = false) {
        Vector3[] vertices = new Vector3[subdivisions + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[(subdivisions * 2) * 3];

        //bottom center
        vertices[0] = Vector3.zero;
        uv[0] = new Vector2(0.5f, 0f);

        //circle
        int lastSubdivisionIndex = subdivisions - 1;
        for (int i = 0; i < subdivisions; i++) {
            float ratio = (float)i / lastSubdivisionIndex;
            float r = ratio * (Mathf.PI * 2f);
            float x = Mathf.Cos(r) * radius;
            float z = Mathf.Sin(r) * radius;

            vertices[i + 1] = new Vector3(x, 0f, z);
            uv[i + 1] = new Vector2(ratio, 0f);
        }

        //point
        vertices[subdivisions + 1] = new Vector3(0f, height, 0f);
        uv[subdivisions + 1] = new Vector2(0.5f, 1f);

        //offset vertices if pivot is tip
        if (tipIsPivot) {
            Vector3 upOffset = Vector3.up * height;
            for (int i = 0; i < vertices.Length - 1; i++) {
                vertices[i] += upOffset;
            }
            vertices[0] -= upOffset;
        }

        //rotate vertices if up is not Vector3.up
        if (up != null && up != Vector3.up) {
            for (int i = 1; i < vertices.Length; i++) {
                vertices[i] = vertices[i].RotateAroundPivot(vertices[0], Quaternion.FromToRotation(Vector3.up, up.Value));
            }
        }

        //make bottom
        for (int i = 0; i < lastSubdivisionIndex; i++) {
            int offset = i * 3;
            triangles[offset] = 0;
            triangles[offset + 1] = i + 1;
            triangles[offset + 2] = i + 2;
        }

        //make sides
        int bottomOffset = subdivisions * 3;
        for (int i = 0; i < lastSubdivisionIndex; i++) {
            int offset = i * 3 + bottomOffset;
            triangles[offset] = i + 1;
            triangles[offset + 1] = subdivisions + 1;
            triangles[offset + 2] = i + 2;
        }

        Mesh mesh = new Mesh {
            vertices = vertices,
            uv = uv,
            triangles = triangles
        };
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }
    
    #endregion
}