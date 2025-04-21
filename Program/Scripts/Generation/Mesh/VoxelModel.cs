using Fantasma.Data;
using Fantasma.Globals;
using Fantasma.Graphics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace Fantasma.Generation
{
    public class VoxelModel
    {
        public static Vector2[] m_uvs =
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
        };
        public static Vector3i[] m_faceIndicies =
        {
            new Vector3i(7, 6, 5),
            new Vector3i(5, 4, 3),
            new Vector3i(1, 0, 7),
            new Vector3i(3, 2, 1)
        };
        public static float[] m_aoValues =
        {
            0.1f,
            0.25f,
            0.5f,
            1f
        };
        public static Vector3i[][] m_faceAos =
        {
            new Vector3i[]
            {
                new Vector3i(1, 1, 1),
                new Vector3i(0, 1, 1),
                new Vector3i(-1, 1, 1),
                new Vector3i(-1, 0, 1),
                new Vector3i(-1, -1, 1),
                new Vector3i(0, -1, 1),
                new Vector3i(1, -1, 1),
                new Vector3i(1, 0, 1),
            },
            new Vector3i[]
            {
                new Vector3i(1, 1, -1),
                new Vector3i(1, 1, 0),
                new Vector3i(1, 1, 1),
                new Vector3i(1, 0, 1),
                new Vector3i(1, -1, 1),
                new Vector3i(1, -1, 0),
                new Vector3i(1, -1, -1),
                new Vector3i(1, 0, -1),
            },
            new Vector3i[]
            {
                new Vector3i(-1, 1, -1),
                new Vector3i(0, 1, -1),
                new Vector3i(1, 1, -1),
                new Vector3i(1, 0, -1),
                new Vector3i(1, -1, -1),
                new Vector3i(0, -1, -1),
                new Vector3i(-1, -1, -1),
                new Vector3i(-1, 0, -1),
            },
            new Vector3i[]
            {
                new Vector3i(-1, 1, 1),
                new Vector3i(-1, 1, 0),
                new Vector3i(-1, 1, -1),
                new Vector3i(-1, 0, -1),
                new Vector3i(-1, -1, -1),
                new Vector3i(-1, -1, 0),
                new Vector3i(-1, -1, 1),
                new Vector3i(-1, 0, 1),
            },
            new Vector3i[]{
                new Vector3i(-1, 1, 1),
                new Vector3i(0, 1, 1),
                new Vector3i(1, 1, 1),
                new Vector3i(1, 1, 0),
                new Vector3i(1, 1, -1),
                new Vector3i(0, 1, -1),
                new Vector3i(-1, 1, -1),
                new Vector3i(-1, 1, 0),
            },
            new Vector3i[]
            {
                new Vector3i(-1, -1, -1),
                new Vector3i(0, -1, -1),
                new Vector3i(1, -1, -1),
                new Vector3i(1, -1, 0),
                new Vector3i(1, -1, 1),
                new Vector3i(0, -1, 1),
                new Vector3i(-1, -1, 1),
                new Vector3i(-1, -1, 0),
            }
        };
        public static Vector3[] m_vertices =
        {
            new Vector3(1, 1, 1),//0
            new Vector3(0, 1, 1),//1
            new Vector3(0, 0, 1),//2
            new Vector3(1, 0, 1),//3
            new Vector3(0, 1, 0),//4
            new Vector3(1, 1, 0),//5
            new Vector3(1, 0, 0),//6
            new Vector3(0, 0, 0),//7
        };
        public static int[][] m_triangles =
        {
            new int[] { 3, 2, 0, 1 },
            new int[] { 6, 3, 5, 0 },
            new int[] { 7, 6, 4, 5 },
            new int[] { 2, 7, 1, 4 },
            new int[] { 4, 5, 1, 0 },
            new int[] { 2, 3, 7, 6 }
        };
        public static void BuildFace(Vector3i position, MeshData data, 
            ref int vertexOffset, ref int triangleIndex, ref int vertexIndex, Vector2i uv, int dir, WorldManager worldManager)
        {
            Vector3 faceVertex;
            Vector3i aoIndicies;
            float aoValue;

            int[] aos = new int[4];

            for (int i = 0; i < 4; i++)
            {
                aoIndicies = m_faceIndicies[i];
                BlockData side1 = worldManager.GetBlockData(m_faceAos[dir][aoIndicies.X] + position);
                BlockData side2 = worldManager.GetBlockData(m_faceAos[dir][aoIndicies.Z] + position);
                BlockData corner = worldManager.GetBlockData(m_faceAos[dir][aoIndicies.Y] + position);

                int aoState = GetAOState(
                    side1.m_opaque ? 1 : 0,
                    side2.m_opaque ? 1 : 0,
                    corner.m_opaque ? 1 : 0
                );

                aos[i] = aoState;
            }

            HandleFlip(aos, data.m_indicies, triangleIndex, vertexIndex);

            for (int i = 0; i < 4; i++)
            {
                faceVertex = m_vertices[m_triangles[dir][i]];
                aoValue = m_aoValues[aos[i]];

                /// Vertex Position
                data.m_vertices[vertexOffset++] = position.X + faceVertex.X;
                data.m_vertices[vertexOffset++] = position.Y + faceVertex.Y;
                data.m_vertices[vertexOffset++] = position.Z + faceVertex.Z;

                //Console.WriteLine("vertex pos: " + position + faceVertex);

                /// Vertex AO
                data.m_vertices[vertexOffset++] = aoValue;
                data.m_vertices[vertexOffset++] = aoValue;
                data.m_vertices[vertexOffset++] = aoValue;

                /// Vertex UV
                data.m_vertices[vertexOffset++] = (uv.X + m_uvs[i].X) * TextureAtlasUtils.m_textureScale;
                data.m_vertices[vertexOffset++] = (uv.Y + m_uvs[i].Y) * TextureAtlasUtils.m_textureScale;
            }
            triangleIndex += 6;
            vertexIndex += 4;
        }
        public static void HandleFlip(int[] aos, int[] indicies, int triangleIndex, int vertexIndex)
        {
            if (MathF.Min(aos[0], aos[3]) > MathF.Min(aos[1], aos[2]))
            {
                //flipped
                indicies[triangleIndex] = vertexIndex + 2;
                indicies[triangleIndex + 1] = vertexIndex + 3;
                indicies[triangleIndex + 2] = vertexIndex;
                indicies[triangleIndex + 3] = vertexIndex + 3;
                indicies[triangleIndex + 4] = vertexIndex + 1;
                indicies[triangleIndex + 5] = vertexIndex;
            }
            else
            {
                //normal
                indicies[triangleIndex] = vertexIndex + 3;
                indicies[triangleIndex + 1] = vertexIndex + 1;
                indicies[triangleIndex + 2] = vertexIndex + 2;
                indicies[triangleIndex + 3] = vertexIndex + 2;
                indicies[triangleIndex + 4] = vertexIndex + 1;
                indicies[triangleIndex + 5] = vertexIndex;
            }
        }
        public static void HandleFlip(int[] aos, List<int> indicies, int triangleIndex, int vertexIndex)
        {
            if (MathF.Min(aos[0], aos[3]) > MathF.Min(aos[1], aos[2]))
            {
                //flipped
                indicies.Add(vertexIndex + 2);
                indicies.Add(vertexIndex + 3);
                indicies.Add(vertexIndex);
                indicies.Add(vertexIndex + 3);
                indicies.Add(vertexIndex + 1);
                indicies.Add(vertexIndex);
            }
            else
            {
                //normal
                indicies.Add(vertexIndex + 3);
                indicies.Add(vertexIndex + 1);
                indicies.Add(vertexIndex + 2);
                indicies.Add(vertexIndex + 2);
                indicies.Add(vertexIndex + 1);
                indicies.Add(vertexIndex);
            }
        }
        public static int GetAOState(int side1, int side2, int corner)
        {
            if (side1 + side2 == 2)
                return 0;

            return 3 - (side1 + side2 + corner);
        }
    }
}
