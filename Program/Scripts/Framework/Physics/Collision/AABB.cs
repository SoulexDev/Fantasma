using OpenTK.Mathematics;
using System;
using System.Runtime.CompilerServices;

namespace Fantasma.Physics
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
        public AABB(float minValue, float maxValue)
        {
            m_minX = minValue;
            m_minY = minValue;
            m_minZ = minValue;

            m_maxX = maxValue;
            m_maxY = maxValue;
            m_maxZ = maxValue;
        }
        public AABB(Vector3 center, float xSize, float ySize, float zSize)
        {
            m_minX = center.X - xSize * 0.5f;
            m_maxX = center.X + xSize * 0.5f;

            m_minY = center.Y - ySize * 0.5f;
            m_maxY = center.Y + ySize * 0.5f;

            m_minZ = center.Z - zSize * 0.5f;
            m_maxZ = center.Z + zSize * 0.5f;
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
        public Vector3 GetSize()
        {
            return new Vector3(GetWidth(), GetHeight(), GetDepth());
        }
        public Vector3 GetCenter()
        {
            return new Vector3(m_minX + m_maxX, m_minY + m_maxY, m_minZ + m_maxZ) * 0.5f;
        }
        public float GetClipX(AABB against, float deltaX)
        {
            if(IntersectsY(against) && IntersectsZ(against))
            {
                //if (!MoveIntersectsX(against, deltaX))
                //    return deltaX;

                if(deltaX > 0 && m_maxX <= against.m_minX)
                {
                    float clip = against.m_minX - m_maxX;
                    if (deltaX > clip)
                        deltaX = clip;
                }
                if (deltaX < 0 && m_minX >= against.m_maxX)
                {
                    float clip = against.m_maxX - m_minX;
                    if (deltaX < clip)
                        deltaX = clip;
                }
                return deltaX;
            }
            return deltaX;
        }
        public float GetClipY(AABB against, float deltaY)
        {
            if (IntersectsX(against) && IntersectsZ(against))
            {
                //if (!MoveIntersectsY(against, deltaY))
                //    return deltaY;

                if (deltaY > 0 && m_maxY <= against.m_minY)
                {
                    float clip = against.m_minY - m_maxY;
                    if (deltaY > clip)
                        deltaY = clip;
                }
                if (deltaY < 0 && m_minY >= against.m_maxY)
                {
                    float clip = against.m_maxY - m_minY;
                    if (deltaY < clip)
                        deltaY = clip;
                }
                return deltaY;
            }
            return deltaY;
        }
        public float GetClipZ(AABB against, float deltaZ)
        {
            if (IntersectsX(against) && IntersectsY(against))
            {
                //if (!MoveIntersectsZ(against, deltaZ))
                //    return deltaZ;

                if (deltaZ > 0 && m_maxZ <= against.m_minZ)
                {
                    float clip = against.m_minZ - m_maxZ;
                    if (deltaZ > clip)
                        deltaZ = clip;
                }
                if (deltaZ < 0 && m_minZ >= against.m_maxZ)
                {
                    float clip = against.m_maxZ - m_minZ;
                    if (deltaZ < clip)
                        deltaZ = clip;
                }
                return deltaZ;
            }
            return deltaZ;
        }
        public RaycastHit GetRayHit(Ray ray)
        {
            RaycastHit returnHit;

            float tmin;
            float tmax;

            float tx1 = (m_minX - ray.origin.X) * ray.invDirection.X;
            float tx2 = (m_maxX - ray.origin.X) * ray.invDirection.X;

            tmin = MathF.Min(tx1, tx2);
            tmax = MathF.Max(tx1, tx2);

            float ty1 = (m_minY - ray.origin.Y) * ray.invDirection.Y;
            float ty2 = (m_maxY - ray.origin.Y) * ray.invDirection.Y;

            tmin = MathF.Min(tmin, MathF.Min(ty1, ty2));
            tmax = MathF.Max(tmax, MathF.Max(ty1, ty2));

            float tz1 = (m_minZ - ray.origin.Z) * ray.invDirection.Z;
            float tz2 = (m_maxZ - ray.origin.Z) * ray.invDirection.Z;

            tmin = MathF.Min(tmin, MathF.Min(tz1, tz2));
            tmax = MathF.Max(tmax, MathF.Max(tz1, tz2));

            if (tmax >= tmin)
            {
                Vector3 hitPoint = (tmin < 0 ? tmax : tmin) * ray.direction + ray.origin;
                Vector3 normal = Vector3.Zero;

                Vector3 dif = (hitPoint - GetCenter()) / GetSize();

                float x = MathF.Abs(dif.X);
                float y = MathF.Abs(dif.Y);
                float z = MathF.Abs(dif.Z);

                if(x >= y && x >= z)
                {
                    normal.X = MathF.Sign(dif.X);
                }
                else if (y >= z && y >= x)
                {
                    normal.Y = MathF.Sign(dif.Y);
                }
                else if(z >= y && z >= x)
                {
                    normal.Z = MathF.Sign(dif.Z);
                }

                returnHit = new RaycastHit(normal, hitPoint);
            }
            else
                returnHit = new RaycastHit();

            return returnHit;
        }
        public bool Intersects(AABB against)
        {
            return IntersectsX(against) && IntersectsY(against) && IntersectsZ(against);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IntersectsX(AABB against)
        {
            return m_minX < against.m_maxX && m_maxX > against.m_minX;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IntersectsY(AABB against)
        {
            return m_minY < against.m_maxY && m_maxY > against.m_minY;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IntersectsZ(AABB against)
        {
            return m_minZ < against.m_maxZ && m_maxZ > against.m_minZ;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveIntersectsX(AABB against, float deltaX)
        {
            return (m_minX + deltaX) < against.m_maxX && (m_maxX + deltaX) > against.m_minX;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveIntersectsY(AABB against, float deltaY)
        {
            return (m_minY + deltaY) < against.m_maxY && (m_maxY + deltaY) > against.m_minY;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveIntersectsZ(AABB against, float deltaZ)
        {
            return (m_minZ + deltaZ) < against.m_maxZ && (m_maxZ + deltaZ) > against.m_minZ;
        }
        private AABB Copy(AABB from)
        {
            return new AABB(from.m_minX, from.m_maxX, from.m_minY, from.m_maxY, from.m_minZ, from.m_maxZ);
        }
    }
}
