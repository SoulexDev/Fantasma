using Fantasma.Globals;
using OpenTK.Graphics.OpenGL4;
using System;

namespace Fantasma.Graphics
{
    public struct MeshData
    {
        public float[] m_vertices;
        public int[] m_indicies;
        public VertexAttribute[] m_attributes;

        public MeshData(float[] vertices, int[] indicies, params VertexAttribute[] attributes)
        {
            m_vertices = vertices;
            m_indicies = indicies;
            m_attributes = attributes;
        }
    }
    public class Mesh
    {
        private int m_vertexBufferObject;
        private int m_vertexArrayObject;
        private int m_elementBufferObject;

        private int m_floatSize = sizeof(float);

        private float[] m_vertices;
        private int[] m_indicies;

        private bool m_generated = false;

        public PrimitiveType m_primitiveType = PrimitiveType.Triangles;
        public void Set(MeshData data)
        {
            m_generated = false;
            if (data.m_attributes.Length == 0)
            {
                Console.WriteLine("Mesh attributes count is 0. Mesh was not created");
                return;
            }
            m_vertices = data.m_vertices;
            m_indicies = data.m_indicies;

            byte stride = 0;
            byte offset = 0;

            m_vertexBufferObject = GL.GenBuffer();
            m_vertexArrayObject = GL.GenVertexArray();

            GL.BindBuffer(BufferTarget.ArrayBuffer, m_vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, m_vertices.Length * m_floatSize, m_vertices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(m_vertexArrayObject);

            for (int i = 0; i < data.m_attributes.Length; i++)
            {
                stride += MeshHelpers.m_vertexAttributeSizes[data.m_attributes[i]];
            }
            for (int i = 0; i < data.m_attributes.Length; i++)
            {
                GL.EnableVertexAttribArray(i);
                switch (data.m_attributes[i])
                {
                    case VertexAttribute.Position:
                        GL.VertexAttribPointer(i, 3, VertexAttribPointerType.Float, false, stride, offset);
                        offset += (byte)(3 * m_floatSize);
                        break;
                    case VertexAttribute.Normal:
                        GL.VertexAttribPointer(i, 3, VertexAttribPointerType.Float, false, stride, offset);
                        offset += (byte)(3 * m_floatSize);
                        break;
                    case VertexAttribute.UV:
                        GL.VertexAttribPointer(i, 2, VertexAttribPointerType.Float, false, stride, offset);
                        offset += (byte)(2 * m_floatSize);
                        break;
                    case VertexAttribute.Color:
                        GL.VertexAttribPointer(i, 3, VertexAttribPointerType.Float, false, stride, offset);
                        offset += (byte)(3 * m_floatSize);
                        break;
                    default:
                        break;
                }
            }

            m_elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, m_indicies.Length * sizeof(int), m_indicies, BufferUsageHint.StaticDraw);

            m_generated = true;
            //GL.BindVertexArray(0);

            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
        public void Render()
        {
            if (!m_generated)
                return;
            GL.BindVertexArray(m_vertexArrayObject);
            GL.DrawElements(m_primitiveType, m_indicies.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }
        //public void Dispose()
        //{
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        //    GL.DeleteBuffer(m_vertexBufferObject);

        //    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        //    GL.DeleteBuffer(m_elementBufferObject);
        //}
    }
}
