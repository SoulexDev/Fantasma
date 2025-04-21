using OpenTK.Graphics.OpenGL4;

namespace Fantasma.Graphics
{
    public class StandardShader : Shader
    {
        private Texture2D m_mainTex;
        private Texture2D m_overlayTex;
        private Texture2D m_grassTex;

        protected override void Init()
        {
            m_mainTex = new Texture2D(Core.m_texturesPath + "blocks/mainAtlas.png");
            m_overlayTex = new Texture2D(Core.m_texturesPath + "blocks/overlayAtlas.png");
            m_grassTex = new Texture2D(Core.m_texturesPath + "blocks/grassAtlas.png");

            base.Init();
        }
        public override string GetVertexShaderSource()
        {
            return Core.m_shadersPath + "standard.vert";
        }
        public override string GetFragmentShaderSource()
        {
            return Core.m_shadersPath + "standard.frag";
        }
        public override void Use()
        {
            GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.CullFace);

            m_mainTex.Bind(TextureUnit.Texture0);
            SetInt("uMainTex", 0);

            m_overlayTex.Bind(TextureUnit.Texture1);
            SetInt("uOverlayTex", 1);

            m_grassTex.Bind(TextureUnit.Texture2);
            SetInt("uGrassTex", 2);

            base.Use();
        }
        public override void Dispose()
        {
            base.Dispose();
            m_mainTex.Dispose();
            m_overlayTex.Dispose();
            m_grassTex.Dispose();
        }
    }
}
