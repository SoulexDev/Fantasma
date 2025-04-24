using Fantasma.Globals;
using Fantasma.Graphics;
using OpenTK.Mathematics;

namespace Fantasma.Generation
{
    public class BoxModel
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
        public static Renderable CreateBox(Transform transform, Shader shader, float scale = 1)
        {
            Mesh m = new Mesh();
            MeshData data = new MeshData(new float[24 * 3], new int[36], VertexAttribute.Position);

            int vertexOffset = 0;
            int triangleIndex = 0;
            int vertexIndex = 0;

            for (int i = 0; i < 6; i++)
            {
                BuildFace(transform.position, data, ref vertexOffset, ref triangleIndex, ref vertexIndex, i, scale);
            }

            m.Set(data);
            return RenderableFactory.RegisterRenderable(transform, m, shader, RenderableType.Opaque);
        }
        private static void BuildFace(Vector3 position, MeshData data, ref int vertexOffset, ref int triangleIndex, ref int vertexIndex, 
            int direction, float scale)
        {
            Vector3 faceVertex;
            for (int i = 0; i < 4; i++)
            {
                faceVertex = m_vertices[m_triangles[direction][i]];

                /// Vertex Position
                data.vertices[vertexOffset++] = (position.X + faceVertex.X) * scale;
                data.vertices[vertexOffset++] = (position.Y + faceVertex.Y) * scale;
                data.vertices[vertexOffset++] = (position.Z + faceVertex.Z) * scale;
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
