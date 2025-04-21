using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using System;
using System.IO;

namespace Fantasma.Graphics
{
    public class Texture2D : IDisposable
    {
        public int m_handle { get; private set; }
        public bool m_generateMipMaps;
        public TextureMagFilter m_filterMode = TextureMagFilter.Nearest;
        private string m_path;

        public Texture2D(string texturePath)
        {
            m_path = texturePath;
            m_handle = GL.GenTexture();
            Bind();

            StbImage.stbi_set_flip_vertically_on_load(1);

            using (Stream stream = File.OpenRead(m_path))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)m_filterMode);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            if (m_generateMipMaps)
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
        public void Bind(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, m_handle);
        }
        private bool _disposed;
        ~Texture2D()
        {
            Dispose(false);
        }
        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                GL.DeleteTexture(m_handle);
                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
