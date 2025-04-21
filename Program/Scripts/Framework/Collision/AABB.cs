using Fantasma.Graphics;
using OpenTK.Mathematics;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Fantasma.Collision
{
    public class AABB
    {
        public float m_minX;
        public float m_maxX;
        public float m_minY;
        public float m_maxY;
        public float m_minZ;
        public float m_maxZ;

        public AABB(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {
            m_minX = minX;
            m_maxX = maxX;
            m_minY = minY;
            m_maxY = maxY;
            m_minZ = minZ;
            m_maxZ = maxZ;
        }
        /// <summary>
        /// Creates unit bounding box
        /// </summary>
        /// <param name="minValue">Minimum bounds</param>
        /// <param name="maxValue">Maximum bounds</param>
        public AABB(float minValue, float maxValue)
        {
            m_minX = minValue;
            m_minY = minValue;
            m_minZ = minValue;

            m_maxX = maxValue;
            m_maxY = maxValue;
            m_maxZ = maxValue;
        }
        public float GetWidth()
        {
            return m_maxX - m_minX;
        }
        public float GetHeight()
        {
            return m_maxY - m_minY;
        }
        public float GetDepth()
        {
            return m_maxZ - m_minZ;
        }
        public AABB Expand(float valueX, float valueY, float valueZ)
        {
            AABB returnValue = Copy(this);

            if (valueX > 0)
                returnValue.m_maxX += valueX;
            else
                returnValue.m_minX += valueX;

            if (valueY > 0)
                returnValue.m_maxY += valueY;
            else
                returnValue.m_minY += valueY;

            if (valueZ > 0)
                returnValue.m_maxZ += valueZ;
            else
                returnValue.m_minZ += valueZ;

            return returnValue;
        }
        public AABB Grow(float valueX, float valueY, float valueZ)
        {
            AABB returnValue = Copy(this);

            returnValue.m_minX -= valueX;
            returnValue.m_minY -= valueY;
            returnValue.m_minZ -= valueZ;

            returnValue.m_maxX += valueX;
            returnValue.m_maxY += valueY;
            returnValue.m_maxZ += valueZ;

            return returnValue;
        }
        public void Move(float valueX, float valueY, float valueZ)
        {
            m_minX += valueX;
            m_maxX += valueX;
            m_minY += valueY;
            m_maxY += valueY;
            m_minZ += valueZ;
            m_maxZ += valueZ;
        }
        public void Move(Vector3 value)
        {
            m_minX += value.X;
            m_maxX += value.X;
            m_minY += value.Y;
            m_maxY += value.Y;
            m_minZ += value.Z;
            m_maxZ += value.Z;
        }
        public Vector3 GetCenter()
        {
            return new Vector3(m_minX + m_maxX, m_minY + m_maxY, m_minZ + m_maxZ) * 0.5f;
        }
        public CollisionResponse Collide(AABB against, Vector3 velocity)
        {
            float xInvEntry, yInvEntry, zInvEntry;
            float xInvExit, yInvExit, zInvExit;

            if(velocity.X > 0)
            {
                xInvEntry = against.m_minX - GetWidth();
                xInvExit = against.GetWidth() - m_minX;
            }
            else
            {
                xInvEntry = against.GetWidth() - m_minX;
                xInvExit = against.m_minX - GetWidth();
            }
            if (velocity.Y > 0)
            {
                yInvEntry = against.m_minY - GetHeight();
                yInvExit = against.GetHeight() - m_minY;
            }
            else
            {
                yInvEntry = against.GetHeight() - m_minY;
                yInvExit = against.m_minY - GetHeight();
            }
            if (velocity.Z > 0)
            {
                zInvEntry = against.m_minZ - GetDepth();
                zInvExit = against.GetDepth() - m_minZ;
            }
            else
            {
                zInvEntry = against.GetDepth() - m_minZ;
                zInvExit = against.m_minZ - GetDepth();
            }

            float xEntry, yEntry, zEntry;
            float xExit, yExit, zExit;

            if(velocity.X == 0)
            {
                xEntry = float.NegativeInfinity;
                xExit = float.PositiveInfinity;
            }
            else
            {
                xEntry = xInvEntry / velocity.X;
                xExit = xInvExit / velocity.X;
            }
            if (velocity.Y == 0)
            {
                yEntry = float.NegativeInfinity;
                yExit = float.PositiveInfinity;
            }
            else
            {
                yEntry = xInvEntry / velocity.Y;
                yExit = xInvExit / velocity.Y;
            }
            if (velocity.Z == 0)
            {
                zEntry = float.NegativeInfinity;
                zExit = float.PositiveInfinity;
            }
            else
            {
                zEntry = xInvEntry / velocity.Z;
                zExit = xInvExit / velocity.Z;
            }

            float entryTime = MathF.Max(MathF.Max(xEntry, yEntry), zEntry);
            float exitTime = MathF.Min(MathF.Min(xExit, yExit), zExit);

            Vector3 normal;

            if(entryTime > exitTime || xEntry < 0 && yEntry < 0 && zEntry < 0 || xEntry > 1 || yEntry > 1 || zEntry > 1)
            {
                return new CollisionResponse(false);
            }
            else
            {
                if(xEntry > yEntry)
                {
                    if(xInvEntry < 0)
                    {
                        normal = new Vector3(1, 0, 0);
                    }
                    else
                    {
                        normal = new Vector3(-1, 0, 0);
                    }
                }
                else if(yEntry > zEntry)
                {
                    if(yInvEntry < 0)
                    {
                        normal = new Vector3(0, 1, 0);
                    }
                    else
                    {
                        normal = new Vector3(0, -1, 0);
                    }
                }
                else
                {
                    if (zInvEntry < 0)
                    {
                        normal = new Vector3(0, 0, 1);
                    }
                    else
                    {
                        normal = new Vector3(0, 0, -1);
                    }
                }
            }

            float dotProduct = (velocity.X * normal.Y + velocity.Y * normal.Z + velocity.Z * normal.X) * (1 - entryTime);
            velocity.X = dotProduct * normal.Y;
            velocity.Y = dotProduct * normal.Z;
            velocity.Z = dotProduct * normal.X;

            return new CollisionResponse(normal, normal * dotProduct);
        }
        public float GetClipX(AABB against, float deltaX)
        {
            if(IntersectsY(against) && IntersectsZ(against))
            {
                if(deltaX > 0 && m_maxX <= against.m_minX)
                {
                    float clip = against.m_minX - m_maxX;
                    if (deltaX > clip)
                        deltaX = -clip;
                }
                if (deltaX < 0 && m_minX >= against.m_maxX)
                {
                    float clip = against.m_maxX - m_minX;
                    if (deltaX < clip)
                        deltaX = -clip;
                }
                return deltaX;
            }
            return deltaX;
        }
        public float GetClipY(AABB against, float deltaY)
        {
            if (IntersectsX(against) && IntersectsZ(against))
            {
                if (deltaY > 0 && m_maxY <= against.m_minY)
                {
                    float clip = against.m_minY - m_maxY;
                    if (deltaY > clip)
                        deltaY = -clip;
                }
                if (deltaY < 0 && m_minY >= against.m_maxY)
                {
                    float clip = against.m_maxY - m_minY;
                    if (deltaY < clip)
                        deltaY = -clip;
                }
                return deltaY;
            }
            return deltaY;
        }
        public float GetClipZ(AABB against, float deltaZ)
        {
            if (IntersectsX(against) && IntersectsY(against))
            {
                if (deltaZ > 0 && m_maxZ <= against.m_minZ)
                {
                    float clip = against.m_minZ - m_maxZ;
                    if (deltaZ > clip)
                        deltaZ = -clip;
                }
                if (deltaZ < 0 && m_minZ >= against.m_maxZ)
                {
                    float clip = against.m_maxZ - m_minZ;
                    if (deltaZ < clip)
                        deltaZ = -clip;
                }
                return deltaZ;
            }
            return deltaZ;
        }
        public float GetPointClipX(AABB against, float deltaX)
        {
            against = Grow(against.GetWidth() * 0.5f, against.GetHeight() * 0.5f, against.GetDepth() * 0.5f);
            Vector3 point = GetCenter();
            if (PointIntersectsY(point) && PointIntersectsZ(point))
            {
                if(deltaX > 0 && point.X < against.m_minX)
                {
                    float clip = against.m_minX - m_maxX;
                    if (deltaX > clip)
                        deltaX = -clip;

                    return deltaX;
                }
                if (deltaX < 0 && point.X > against.m_maxX)
                {
                    float clip = against.m_maxX - point.X;
                    if (deltaX < clip)
                        deltaX = -clip;

                    return deltaX;
                }
            }
            return deltaX;
        }
        public float GetPointClipY(AABB against, float deltaY)
        {
            against = Grow(against.GetWidth() * 0.5f, against.GetHeight() * 0.5f, against.GetDepth() * 0.5f);
            Vector3 point = GetCenter();
            if (PointIntersectsY(point) && PointIntersectsZ(point))
            {
                if (deltaY > 0 && point.Y < against.m_minY)
                {
                    float clip = against.m_minY - m_maxY;
                    if (deltaY > clip)
                        deltaY = -clip;

                    return deltaY;
                }
                if (deltaY < 0 && point.Y > against.m_maxY)
                {
                    float clip = against.m_maxY - point.Y;
                    if (deltaY < clip)
                        deltaY = -clip;

                    return deltaY;
                }
            }
            return deltaY;
        }
        public float GetPointClipZ(AABB against, float deltaZ)
        {
            against = Grow(against.GetWidth() * 0.5f, against.GetHeight() * 0.5f, against.GetDepth() * 0.5f);
            Vector3 point = GetCenter();
            if (PointIntersectsY(point) && PointIntersectsZ(point))
            {
                if (deltaZ > 0 && point.Z < against.m_minZ)
                {
                    float clip = against.m_minZ - m_maxZ;
                    if (deltaZ > clip)
                        deltaZ = -clip;

                    return deltaZ;
                }
                if (deltaZ < 0 && point.Z > against.m_maxZ)
                {
                    float clip = against.m_maxZ - point.Z;
                    if (deltaZ < clip)
                        deltaZ = -clip;

                    return deltaZ;
                }
            }
            return deltaZ;
        }
        public bool PointIntersects(Vector3 point)
        {
            return PointIntersectsX(point) && PointIntersectsY(point) && PointIntersectsZ(point);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool PointIntersectsX(Vector3 point)
        {
            return point.X > m_minX && point.X < m_maxX;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool PointIntersectsY(Vector3 point)
        {
            return point.Y > m_minY && point.Y < m_maxY;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool PointIntersectsZ(Vector3 point)
        {
            return point.Z > m_minZ && point.Z < m_maxZ;
        }
        public bool Intersects(AABB against)
        {
            return IntersectsX(against) && IntersectsY(against) && IntersectsZ(against);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IntersectsX(AABB against)
        {
            return m_minX <= against.m_maxX && m_maxX >= against.m_minX;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IntersectsY(AABB against)
        {
            return m_minY <= against.m_maxY && m_maxY >= against.m_minY;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IntersectsZ(AABB against)
        {
            return m_minZ <= against.m_maxZ && m_maxZ >= against.m_minZ;
        }
        private AABB Copy(AABB from)
        {
            return new AABB(from.m_minX, from.m_maxX, from.m_minY, from.m_maxY, from.m_minZ, from.m_maxZ);
        }
    }
}
