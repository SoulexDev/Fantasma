using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Fantasma.Graphics
{
    public struct Renderable
    {
        public Matrix4 m_modelMatrix;
        public Transform m_transform;
        public Shader m_shader;
        public Mesh m_mesh;

        public Renderable(Transform transform, Mesh mesh, Shader shader)
        {
            m_transform = transform;
            m_mesh = mesh;
            m_shader = shader;
            m_modelMatrix = new Matrix4();
        }
    }
    public class MeshRenderer
    {
        //public Dictionary<string, Renderable> m_renderables = new Dictionary<string, Renderable>();

        //public Matrix4 m_modelMatrix;
        //public Shader m_shader;
        //public Mesh m_mesh;

        //public void Render(Transform transform)
        //{
        //    //if (m_shader == null || m_mesh == null)
        //    //    return;

        //    //m_modelMatrix = Matrix4.CreateFromQuaternion(transform.rotation);
        //    //m_modelMatrix *= Matrix4.CreateTranslation(transform.position);
        //    //m_modelMatrix *= Matrix4.CreateScale(transform.scale);

        //    //m_shader.SetMatrix4("uModel", m_modelMatrix);
        //    //m_shader.Use();

        //    //m_mesh.Render();

        //    for (int i = 0; i < m_renderables.Count; i++)
        //    {
        //        Renderable renderable = m_renderables[i];
        //        if (renderable.m_shader == null || renderable.m_mesh == null)
        //            return;

        //        renderable.m_modelMatrix = Matrix4.CreateFromQuaternion(transform.rotation);
        //        renderable.m_modelMatrix *= Matrix4.CreateTranslation(transform.position);
        //        renderable.m_modelMatrix *= Matrix4.CreateScale(transform.scale);

        //        renderable.m_shader.SetMatrix4("uModel", renderable.m_modelMatrix);
        //        renderable.m_shader.Use();

        //        renderable.m_mesh.Render();
        //    }
        //}
        public static void Render(List<Renderable> renderables)
        {
            //if (m_shader == null || m_mesh == null)
            //    return;

            //m_modelMatrix = Matrix4.CreateFromQuaternion(transform.rotation);
            //m_modelMatrix *= Matrix4.CreateTranslation(transform.position);
            //m_modelMatrix *= Matrix4.CreateScale(transform.scale);

            //m_shader.SetMatrix4("uModel", m_modelMatrix);
            //m_shader.Use();

            //m_mesh.Render();

            for (int i = 0; i < renderables.Count; i++)
            {
                Renderable renderable = renderables[i];
                if (renderable.m_shader == null || renderable.m_mesh == null)
                    return;

                renderable.m_modelMatrix = Matrix4.CreateFromQuaternion(renderable.m_transform.rotation);
                renderable.m_modelMatrix *= Matrix4.CreateTranslation(renderable.m_transform.position);
                renderable.m_modelMatrix *= Matrix4.CreateScale(renderable.m_transform.scale);

                renderable.m_shader.SetMatrix4("uModel", renderable.m_modelMatrix);
                renderable.m_shader.Use();

                renderable.m_mesh.Render();
            }
        }
        //public void AddRenderable(string name, Renderable renderable)
        //{
        //    m_renderables.Add(name, renderable);
        //}
        public void Dispose()
        {
            //foreach (var renderable in m_renderables.Values)
            //{
            //    renderable.m_shader.Dispose();
            //}
        }
    }
}
