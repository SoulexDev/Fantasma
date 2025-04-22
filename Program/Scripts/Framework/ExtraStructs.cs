using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Framework
{
    public struct Bool3
    {
        public bool x;
        public bool y;
        public bool z;

        public Bool3(bool v)
        {
            x = v;
            y = v;
            z = v;
        }
        public Bool3(bool x, bool y, bool z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Bool3(Bool3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }
        public static Bool3 LessThanEqualTo(Vector3 a, Vector3 b)
        {
            return new Bool3(a.X <= b.X, a.Y <= b.Y, a.Z <= b.Z);
        }
        public static explicit operator Vector3(Bool3 v)
        {
            return new Vector3(v.x ? 1 : 0, v.y ? 1 : 0, v.z ? 1 : 0);
        }
        public static explicit operator Vector3i(Bool3 v)
        {
            return new Vector3i(v.x ? 1 : 0, v.y ? 1 : 0, v.z ? 1 : 0);
        }
    }
}
