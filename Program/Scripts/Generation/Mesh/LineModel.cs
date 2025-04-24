using Fantasma.Graphics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Generation
{
    public class LineModel
    {
        public static Vector2[] m_uvs =
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
        };
        public static Vector3[] m_vertices =
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
        };

        public static void Build(Vector3 origin, Vector3 start, Vector3 end, Quaternion rotation, MeshData data, ref int vertexOffset, ref int triangleIndex, ref int vertexIndex)
        {
            Vector3 faceVertex;
            //Vector3 axis = (end - start).Normalized();

            for (int i = 0; i < 4; i++)
            {
                faceVertex = m_vertices[i];
                //faceVertex -= new Vector3(0.5f, 0.5f, 0);
                faceVertex.X *= 0.01f;
                faceVertex += i < 2 ? start : end;
                //faceVertex = Vector3.Transform(faceVertex, rotation);

                data.vertices[vertexOffset++] = origin.X + faceVertex.X;
                data.vertices[vertexOffset++] = origin.Y + faceVertex.Y;
                data.vertices[vertexOffset++] = origin.Z + faceVertex.Z;

                //data.m_vertices[vertexOffset++] = m_uvs[i].X;
                //data.m_vertices[vertexOffset++] = m_uvs[i].Y;
            }

            data.indicies[triangleIndex] = vertexIndex + 3;
            data.indicies[triangleIndex + 1] = vertexIndex + 1;
            data.indicies[triangleIndex + 2] = vertexIndex + 2;
            data.indicies[triangleIndex + 3] = vertexIndex + 2;
            data.indicies[triangleIndex + 4] = vertexIndex + 1;
            data.indicies[triangleIndex + 5] = vertexIndex;

            triangleIndex += 6;
            vertexIndex += 4;
        }
    }
}
