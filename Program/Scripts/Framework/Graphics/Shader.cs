using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Fantasma.Graphics
{
    public class Shader
    {
        protected ShaderProgram m_shaderProgram;
        protected Dictionary<string, int> m_uniformLocations = new Dictionary<string, int>();

        public Shader()
        {
            m_shaderProgram = new ShaderProgram(GetVertexShaderSource(), GetFragmentShaderSource());
            Init();
        }
        protected virtual void Init()
        {
            GL.GetProgram(m_shaderProgram.m_handle, GetProgramParameterName.ActiveUniforms, out int uniformsCount);

            for (int i = 0; i < uniformsCount; i++)
            {
                string key = GL.GetActiveUniform(m_shaderProgram.m_handle, i, out _, out _);
                int location = GL.GetUniformLocation(m_shaderProgram.m_handle, key);

                m_uniformLocations.Add(key, location);
            }
        }
        public virtual string GetVertexShaderSource()
        {
            return "";
        }
        public virtual string GetFragmentShaderSource()
        {
            return "";
        }
        public virtual void Use()
        {
            if (m_shaderProgram == null)
                return;

            SetMatrix4("uView", Core.m_currentCamera.m_viewMatrix);
            SetMatrix4("uProjection", Core.m_currentCamera.m_projectionMatrix);
            m_shaderProgram.Use();
        }
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(m_shaderProgram.m_handle, attribName);
        }
        public void SetInt(string name, int value)
        {
            GL.Uniform1(m_uniformLocations[name], value);
        }
        public void SetFloat(string name, float value)
        {
            GL.Uniform1(m_uniformLocations[name], value);
        }
        public void SetMatrix4(string name, Matrix4 value)
        {
            GL.UniformMatrix4(m_uniformLocations[name], true, ref value);
        }
        public virtual void Dispose()
        {
            if (m_shaderProgram != null)
                m_shaderProgram.Dispose();
        }
    }
}
