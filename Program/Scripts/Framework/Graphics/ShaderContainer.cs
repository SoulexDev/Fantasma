using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Graphics
{
    public class ShaderContainer
    {
        public static StandardShader m_standardShader = new StandardShader();
        public static StandardShaderTransparent m_standardTransparentShader = new StandardShaderTransparent();
        public static WireShader m_wireShader = new WireShader();
    }
}
