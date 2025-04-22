using Fantasma.Physics;
using OpenTK.Mathematics;

namespace Fantasma.Framework
{
    public class Camera : FantasmaObject
    {
        public Matrix4 m_viewMatrix;
        public Matrix4 m_projectionMatrix;

        public float m_fov = 85;
        public float m_near = 0.1f;
        public float m_far = 1000f;

        public override void OnRender()
        {
            SetMatrices();
        }
        public void SetMatrices()
        {
            m_viewMatrix = Matrix4.LookAt(m_transform.position, m_transform.position + m_transform.forward, m_transform.up);
            m_projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegToRad * m_fov, Core.m_aspect, m_near, m_far);
        }
        public Ray GetViewportRay()
        {
            return new Ray(m_transform.position, m_transform.position + m_transform.forward);
        }
    }
}
