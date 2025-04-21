using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Globals
{
    public static class Extensions
    {
        public static Vector3i FromVector3(this Vector3i v, Vector3 a)
        {
            v.X = (int)MathF.Floor(a.X);
            v.Y = (int)MathF.Floor(a.Y);
            v.Z = (int)MathF.Floor(a.Z);
            return v;
        }
    }
}
