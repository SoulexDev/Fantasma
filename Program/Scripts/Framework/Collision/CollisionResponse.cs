using OpenTK.Mathematics;

namespace Fantasma.Collision
{
    public struct CollisionResponse
    {
        public bool m_collided;
        public Vector3 m_normal;
        public Vector3 m_collisionDelta;

        public CollisionResponse(Vector3 normal, Vector3 collisionDelta)
        {
            m_normal = normal;
            m_collisionDelta = collisionDelta;

            m_collided = true;
        }
        public CollisionResponse(bool collided)
        {
            m_normal = Vector3.Zero;
            m_collisionDelta = Vector3.Zero;

            m_collided = collided;
        }
    }
}
