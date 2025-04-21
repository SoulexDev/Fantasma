using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Graphics
{
    public class ShaderProgram : IDisposable
    {
        public int m_handle;
        //public bool m_isCompiled;
        public ShaderProgram(string vertexShaderPath, string fragmentShaderPath)
        {
            Console.WriteLine(vertexShaderPath);
            string vertexShaderSource = System.IO.File.ReadAllText(vertexShaderPath);
            string fragmentShaderSource = System.IO.File.ReadAllText(fragmentShaderPath);

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);

            int success;
            bool m_isCompiled = true;

            GL.CompileShader(vertexShader);

            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);
                Console.WriteLine(infoLog);

                m_isCompiled = false;
            }

            GL.CompileShader(fragmentShader);

            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(fragmentShader);
                Console.WriteLine(infoLog);

                m_isCompiled = false;
            }

            if (!m_isCompiled)
            {
                GL.DeleteShader(vertexShader);
                GL.DeleteShader(fragmentShader);
                return;
            }

            m_handle = GL.CreateProgram();

            GL.AttachShader(m_handle, vertexShader);
            GL.AttachShader(m_handle, fragmentShader);

            GL.LinkProgram(m_handle);

            GL.GetProgram(m_handle, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(m_handle);
                Console.WriteLine(infoLog);
            }

            GL.DetachShader(m_handle, vertexShader);
            GL.DetachShader(m_handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }
        public void Use()
        {
            GL.UseProgram(m_handle);
        }
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(m_handle);

                disposedValue = true;
            }
        }

        ~ShaderProgram()
        {
            if (disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
