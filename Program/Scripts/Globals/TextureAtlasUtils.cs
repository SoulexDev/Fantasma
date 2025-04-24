using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Globals
{
    public class TextureAtlasUtils
    {
        public static int m_textureSize = 64;
        public static int m_atlasSize = 4;

        public static float m_textureScale = (float)m_atlasSize / m_textureSize;
        public static int m_tileSize = m_textureSize / m_atlasSize;
    }
}
