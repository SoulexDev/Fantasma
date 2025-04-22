using Fantasma.Framework;
using Fantasma.Generation;
using Fantasma.Globals;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace Fantasma.Physics
{
    public class PhysicsBody : FantasmaObject
    {
        public bool m_useGravity = true;
        public Vector3 m_position = Vector3.Zero;
        public Vector3 m_velocity = Vector3.Zero;

        public AABB m_aabb;
        public CollisionFlags m_collisionFlags;

        public PhysicsBody(Vector3 position, float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {
            m_aabb = new AABB(minX, maxX, minY, maxY, minZ, maxZ);
            m_aabb.Move(position);
        }
        public void SetVelocity(float x, float y, float z)
        {
            m_velocity.X = x;
            m_velocity.Y = y;
            m_velocity.Z = z;
        }
        public void SetVelocity(Vector3 velocity)
        {
            m_velocity = velocity;
        }
        public void AddForce(Vector3 force)
        {
            m_velocity += force;
        }
        public override void Update()
        {
            m_velocity.Y += Physics.Gravity * Time.m_deltaTime * Time.m_deltaTime;
            Move();
        }
        public void Move()
        {
            Vector3 moveVector = m_velocity;
            Vector3 originalVector = moveVector;

            List<AABB> collisionChecks = WorldManager.GetColliders(m_aabb.Expand(m_velocity.X, m_velocity.Y, m_velocity.Z).Grow(1, 1, 1));

            foreach (AABB collider in collisionChecks)
            {
                moveVector.Y = m_aabb.GetClipY(collider, moveVector.Y);
            }

            m_aabb.Move(0, moveVector.Y, 0);

            foreach (AABB collider in collisionChecks)
            {
                moveVector.X = m_aabb.GetClipX(collider, moveVector.X);
            }

            m_aabb.Move(moveVector.X, 0, 0);

            foreach (AABB collider in collisionChecks)
            {
                moveVector.Z = m_aabb.GetClipZ(collider, moveVector.Z);
            }

            m_aabb.Move(0, 0, moveVector.Z);

            if (moveVector.X != originalVector.X)
            {
                //if (originalVector.X > 0)
                //    m_collisionFlags |= CollisionFlags.CollisionRight;
                //else
                //    m_collisionFlags |= CollisionFlags.CollisionLeft;

                m_velocity.X = 0;
            }

            if (moveVector.Y != originalVector.Y)
            {
                //Console.WriteLine("i hit shit");
                //if (originalVector.Y > 0)
                //    m_collisionFlags |= CollisionFlags.CollisionUp;
                //else
                //    m_collisionFlags |= CollisionFlags.CollisionDown;

                m_velocity.Y = 0;
            }

            m_collisionFlags = moveVector.Y != originalVector.Y && originalVector.Y < 0 ? CollisionFlags.CollisionDown : 0;

            if (moveVector.Z != originalVector.Z)
            {
                //if (originalVector.Z > 0)
                //    m_collisionFlags |= CollisionFlags.CollisionForward;
                //else
                //    m_collisionFlags |= CollisionFlags.CollisionBack;

                m_velocity.Z = 0;
            }

            m_position = m_aabb.GetCenter();
        }
    }
}
