using Fantasma.Physics;
using Fantasma.Framework;
using Fantasma.Generation;
using Fantasma.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Fantasma.Debug
{
    public class BoundingBox : FantasmaObject
    {
        private AABB m_bounds;
        public void Create(AABB bounds)
        {
            Renderable renderable = BoxModel.CreateBox(m_transform, ShaderContainer.m_wireShader);
            renderable.m_mesh.m_primitiveType = PrimitiveType.Lines;
        }
        public override void Update()
        {
            if(m_bounds != null)
            {
                m_transform.localScale.X = m_bounds.GetWidth();
                m_transform.localScale.Y = m_bounds.GetHeight();
                m_transform.localScale.Z = m_bounds.GetDepth();
            }
        }
    }
}
