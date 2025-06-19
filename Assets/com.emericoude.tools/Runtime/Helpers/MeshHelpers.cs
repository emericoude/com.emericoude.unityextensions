using System.Collections.Generic;
using UnityEngine;



namespace Emericoude.Helpers
{
    public static class MeshHelpers
    {
        //sourced from https://discussions.unity.com/t/raycasthit-texturecoord-does-the-reverse-exist/36255/3
        /// <summary> Takes a UV position and returns the possible world space points on the mesh. </summary>
        /// <returns>A list of positions on the mesh, if they were found.</returns>
        public static Vector3[] UVToWorldPoints(this Mesh aMesh, Vector2 aUVPos)
        {
            List<Vector3> result = new List<Vector3>();
            Vector3[] verts = aMesh.vertices;
            Vector2[] uvs = aMesh.uv;
            int[] indices = aMesh.triangles;
            for(int i = 0; i < indices.Length; i += 3)
            {
                int i1 = indices[i  ];
                int i2 = indices[i+1];
                int i3 = indices[i+2];
                Vector3 bary = VectorHelpers.GetTriangleBarycentric(uvs[i1],uvs[i2],uvs[i3],aUVPos);
                if (VectorHelpers.IsBarycentricInTriangle(bary))
                {
                    Vector3 localP = bary.x * verts[i1] + bary.y * verts[i2] + bary.z * verts[i3];
                    result.Add(localP);
                }
            }
            return result.ToArray();
        }
    }
}
