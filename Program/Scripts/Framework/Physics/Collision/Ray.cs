using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Physics
{
    public struct Ray
    {
        public Vector3 origin;
        public Vector3 direction;
        public Vector3 invDirection;

        public Ray(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            this.direction = direction;
            invDirection = Vector3.One / direction;
        }
        public AABB GetRayBounds()
        {
            Vector3 size = origin - direction;
            return new AABB((origin + direction) * 0.5f, MathF.Abs(size.X), MathF.Abs(size.Y), MathF.Abs(size.Z));
        }
    }
}
