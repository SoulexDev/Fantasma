using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Physics
{
    public struct RaycastHit
    {
        public bool hit;
        public Vector3 normal;
        public Vector3 hitPoint;

        public RaycastHit(Vector3 normal, Vector3 hitPoint)
        {
            this.normal = normal;
            this.hitPoint = hitPoint;
            hit = true;
        }
    }
}
