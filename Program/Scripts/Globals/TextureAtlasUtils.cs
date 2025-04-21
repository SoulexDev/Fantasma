using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Globals
{
    public class TextureAtlasUtils
    {
        public static int m_textureSize = 512;
        public static int m_atlasSize = 32;

        public static float m_textureScale = (float)m_atlasSize / m_textureSize;
    }
}
