using Fantasma.Physics;
using Fantasma.Framework;
using Fantasma.Generation;
using Fantasma.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Fantasma.Debug
{
    public class BoundingBox : FantasmaObject
    {
        public AABB m_bounds;
        public Renderable m_renderable;

        private Vector3 m_lastPosition;
        public void Create(AABB bounds)
        {
            m_bounds = bounds;

            m_renderable = BoxModel.CreateBox(m_transform, ShaderContainer.m_wireShader, 1.005f);
            m_renderable.mesh.m_primitiveType = PrimitiveType.Lines;
        }
        //public override void Update()
        //{
        //    if (m_bounds != null)
        //    {
        //        m_bounds.SetCenter(m_transform.position);
        //    }
        //}
    }
}
