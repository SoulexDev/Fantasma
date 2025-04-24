using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Globals
{
    public class GlobalCoordinates
    {
        public static readonly Vector3i[] m_faceDirections =
        {
            Vector3i.UnitX,
            -Vector3i.UnitX,
            Vector3i.UnitY,
            -Vector3i.UnitY,
            Vector3i.UnitZ,
            -Vector3i.UnitZ,
        };
    }
}
