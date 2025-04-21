using Fantasma.Generation;
using Fantasma.Globals;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Data
{
    public struct HitResult
    {
        public bool m_hit;
        public BlockType m_block;
        public Vector3i m_hitPos;
        public Vector3i m_normal;
        public int m_blockIndex;
        public SubChunk m_chunk;
    }
}
