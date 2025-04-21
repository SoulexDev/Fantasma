using Fantasma.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Generation
{
    public class WorldParameters
    {
        public static int m_chunkSize = 16;
        public static int m_chunkSizeSqrd = m_chunkSize * m_chunkSize;
        public static int m_chunkSizeCbd = m_chunkSizeSqrd * m_chunkSize;

        public static int VOXEL_FACE_COUNT = 6;
        public static int VERTICIES_PER_FACE = 4;
        public static int INDICIES_PER_FACE = 6;
        public static int VOXEL_ATTRIBUTES_LENGTH = 8;
    }
}
