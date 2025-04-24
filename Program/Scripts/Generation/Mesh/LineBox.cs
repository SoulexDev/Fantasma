using Fantasma.Framework;
using Fantasma.Globals;
using Fantasma.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Generation
{
    public class LineBox : FantasmaObject
    {
        //private static Vector3[] m_vertices =
        //{
        //    new Vector3(1, 1, 1),//0
        //    new Vector3(0, 1, 1),//1
        //    new Vector3(0, 0, 1),//2
        //    new Vector3(1, 0, 1),//3
        //    new Vector3(0, 1, 0),//4
        //    new Vector3(1, 1, 0),//5
        //    new Vector3(1, 0, 0),//6
        //    new Vector3(0, 0, 0),//7
        //};
        public Renderable Create()
        {
            Mesh mesh = new Mesh();
            MeshData meshData = new MeshData(new float[12 * 4 * 3], new int[12 * 6], VertexAttribute.Position);

            int vertexOffset = 0;
            int triangleIndex = 0;
            int vertexIndex = 0;

            Quaternion rotationA = Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegToRad * 90);
            Quaternion rotationB = Quaternion.FromAxisAngle(Vector3.UnitZ, MathHelper.DegToRad * 90);

            LineModel.Build(m_transform.position, new Vector3(0, 0, 0), new Vector3(0, 1, 0), Quaternion.Identity, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);
            LineModel.Build(m_transform.position, new Vector3(1, 0, 0), new Vector3(1, 1, 0), Quaternion.Identity, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);
            LineModel.Build(m_transform.position, new Vector3(1, 0, 1), new Vector3(1, 1, 0), Quaternion.Identity, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);
            LineModel.Build(m_transform.position, new Vector3(0, 0, 1), new Vector3(0, 1, 1), Quaternion.Identity, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);

            LineModel.Build(m_transform.position, new Vector3(0, 0, 0), new Vector3(0, 1, 0), rotationA, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);
            LineModel.Build(m_transform.position, new Vector3(1, 0, 0), new Vector3(1, 1, 0), rotationA, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);
            LineModel.Build(m_transform.position, new Vector3(1, 0, 1), new Vector3(1, 1, 0), rotationA, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);
            LineModel.Build(m_transform.position, new Vector3(0, 0, 1), new Vector3(0, 1, 1), rotationA, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);

            LineModel.Build(m_transform.position, new Vector3(0, 0, 0), new Vector3(0, 1, 0), rotationB, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);
            LineModel.Build(m_transform.position, new Vector3(1, 0, 0), new Vector3(1, 1, 0), rotationB, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);
            LineModel.Build(m_transform.position, new Vector3(1, 0, 1), new Vector3(1, 1, 0), rotationB, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);
            LineModel.Build(m_transform.position, new Vector3(0, 0, 1), new Vector3(0, 1, 1), rotationB, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);
            //LineModel.Build(m_transform.position, m_vertices[7], m_vertices[2], rotationA, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);
            //LineModel.Build(m_transform.position, m_vertices[6], m_vertices[3], rotationA, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);
            //LineModel.Build(m_transform.position, m_vertices[5], m_vertices[0], rotationA, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);
            //LineModel.Build(m_transform.position, m_vertices[4], m_vertices[1], rotationA, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);

            //LineModel.Build(m_transform.position, m_vertices[7], m_vertices[6], rotationB, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);
            //LineModel.Build(m_transform.position, m_vertices[2], m_vertices[3], rotationB, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);
            //LineModel.Build(m_transform.position, m_vertices[1], m_vertices[0], rotationB, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);
            //LineModel.Build(m_transform.position, m_vertices[4], m_vertices[5], rotationB, meshData, ref vertexOffset, ref triangleIndex, ref vertexIndex);

            mesh.Set(meshData);
            mesh.m_primitiveType = PrimitiveType.Triangles;
            return RenderableFactory.RegisterRenderable(m_transform, mesh, ShaderContainer.m_standardShader, RenderableType.Opaque);
        }
    }
}
