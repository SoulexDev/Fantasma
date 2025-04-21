using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using System;
using System.IO;

namespace Fantasma.Graphics
{
    public class Cubemap : IDisposable
    {
        public int m_handle { get; private set; }
        private string m_path;

        private static readonly string[] m_faces =
        {
            "_right.png",
            "_left.png",
            "_top.png",
            "_bottom.png",
            "_front.png",
            "_back.png",
        };
        public Cubemap(string texturePath)
        {
            m_path = texturePath;
            m_handle = GL.GenTexture();
            Bind();

            StbImage.stbi_set_flip_vertically_on_load(1);
            for (int i = 0; i < m_faces.Length; i++)
            {
                using (Stream stream = File.OpenRead(m_path + m_faces[i]))
                {
                    ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba,
                    image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
                }
            }

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
        }
        public void Bind(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, m_handle);
        }
        private bool _disposed;
        ~Cubemap()
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
