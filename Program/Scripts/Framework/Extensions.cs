using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Framework
{
    public static class Extensions
    {
        public static Vector3i Sign(this Vector3 v)
        {
            return new Vector3i(MathF.Sign(v.X), MathF.Sign(v.Y), MathF.Sign(v.Z));
        }
    }
}
