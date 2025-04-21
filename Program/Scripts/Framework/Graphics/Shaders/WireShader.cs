using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Graphics
{
    public class WireShader : Shader
    {
        public override string GetVertexShaderSource()
        {
            return Core.m_shadersPath + "wire.vert";
        }
        public override string GetFragmentShaderSource()
        {
            return Core.m_shadersPath + "wire.frag";
        }
        public override void Use()
        {
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.CullFace);

            base.Use();
        }
    }
}
