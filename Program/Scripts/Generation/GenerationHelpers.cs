using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Generation
{
    public class GenerationHelpers
    {
        public static Vector3i[] m_directionalOffsets =
        {
            new Vector3i(0, 0, 1),
            new Vector3i(1, 0, 0),
            new Vector3i(0, 0, -1),
            new Vector3i(-1, 0, 0),
            new Vector3i(0, 1, 0),
            new Vector3i(0, -1, 0)
        };
    }
}
