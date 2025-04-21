using Fantasma.Globals;
using Fantasma.Graphics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Generation
{
    public class BillboardModel
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
            new Vector3(0.1f, 0, 0.1f),
            new Vector3(0.8f, 0, 0.8f),
            new Vector3(0.1f, 0.8f, 0.1f),
            new Vector3(0.8f, 0.8f, 0.8f),
            new Vector3(0.1f, 0, 0.8f),
            new Vector3(0.8f, 0, 0.1f),
            new Vector3(0.1f, 0.8f, 0.8f),
            new Vector3(0.8f, 0.8f, 0.1f)
        };
        public static void Build(Vector3i position, MeshData data,
            ref int vertexOffset, ref int triangleIndex, ref int vertexIndex, Vector2i uv)
        {
            Vector3 faceVertex;
            //Vector3i aoIndicies;
            //float aoValue;

            //int[] aos = new int[4];

            //for (int i = 0; i < 4; i++)
            //{
            //    aoIndicies = m_faceIndicies[i];
            //    BlockData side1 = worldManager.GetBlockData(m_faceAos[dir][aoIndicies.X] + position);
            //    BlockData side2 = worldManager.GetBlockData(m_faceAos[dir][aoIndicies.Z] + position);
            //    BlockData corner = worldManager.GetBlockData(m_faceAos[dir][aoIndicies.Y] + position);

            //    int aoState = GetAOState(
            //        side1.m_opaque ? 1 : 0,
            //        side2.m_opaque ? 1 : 0,
            //        corner.m_opaque ? 1 : 0
            //    );

            //    aos[i] = aoState;
            //}
            for (int i = 0; i < 4; i++)
            {
                faceVertex = m_vertices[i];
                //aoValue = m_aoValues[aos[i]];

                /// Vertex Position
                data.m_vertices[vertexOffset++] = position.X + faceVertex.X;
                data.m_vertices[vertexOffset++] = position.Y + faceVertex.Y;
                data.m_vertices[vertexOffset++] = position.Z + faceVertex.Z;


                /// Vertex AO
                data.m_vertices[vertexOffset++] = 1;
                data.m_vertices[vertexOffset++] = 1;
                data.m_vertices[vertexOffset++] = 1;

                /// Vertex UV
                data.m_vertices[vertexOffset++] = (uv.X + m_uvs[i].X) * TextureAtlasUtils.m_textureScale;
                data.m_vertices[vertexOffset++] = (uv.Y + m_uvs[i].Y) * TextureAtlasUtils.m_textureScale;
            }
            data.m_indicies[triangleIndex] = vertexIndex + 3;
            data.m_indicies[triangleIndex + 1] = vertexIndex + 1;
            data.m_indicies[triangleIndex + 2] = vertexIndex + 2;
            data.m_indicies[triangleIndex + 3] = vertexIndex + 2;
            data.m_indicies[triangleIndex + 4] = vertexIndex + 1;
            data.m_indicies[triangleIndex + 5] = vertexIndex;

            triangleIndex += 6;
            vertexIndex += 4;

            for (int i = 0; i < 4; i++)
            {
                faceVertex = m_vertices[i + 4];
                //aoValue = m_aoValues[aos[i]];

                /// Vertex Position
                data.m_vertices[vertexOffset++] = position.X + faceVertex.X;
                data.m_vertices[vertexOffset++] = position.Y + faceVertex.Y;
                data.m_vertices[vertexOffset++] = position.Z + faceVertex.Z;

                /// Vertex AO
                data.m_vertices[vertexOffset++] = 1;
                data.m_vertices[vertexOffset++] = 1;
                data.m_vertices[vertexOffset++] = 1;

                /// Vertex UV
                data.m_vertices[vertexOffset++] = (uv.X + m_uvs[i].X) * TextureAtlasUtils.m_textureScale;
                data.m_vertices[vertexOffset++] = (uv.Y + m_uvs[i].Y) * TextureAtlasUtils.m_textureScale;
            }

            data.m_indicies[triangleIndex] = vertexIndex + 3;
            data.m_indicies[triangleIndex + 1] = vertexIndex + 1;
            data.m_indicies[triangleIndex + 2] = vertexIndex + 2;
            data.m_indicies[triangleIndex + 3] = vertexIndex + 2;
            data.m_indicies[triangleIndex + 4] = vertexIndex + 1;
            data.m_indicies[triangleIndex + 5] = vertexIndex;

            triangleIndex += 6;
            vertexIndex += 4;
        }
        //public static int GetAOState(int side1, int side2, int corner)
        //{
        //    if (side1 + side2 == 2)
        //        return 0;

        //    return 3 - (side1 + side2 + corner);
        //}
    }
}
