using Fantasma.Globals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Graphics
{
    public class MeshHelpers
    {
        public static Dictionary<VertexAttribute, byte> m_vertexAttributeSizes = new Dictionary<VertexAttribute, byte>()
        {
            { VertexAttribute.Position, 12 },
            { VertexAttribute.Normal, 12 },
            { VertexAttribute.UV, 8 },
            { VertexAttribute.Color, 12 }
        };
    }
}
