using OpenTK.Graphics.OpenGL4;

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
            //GL.Disable(EnableCap.CullFace);
            GL.LineWidth(5);

            base.Use();
        }
    }
}
