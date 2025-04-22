using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
        public static void Render(List<Renderable> renderables)
        {
            for (int i = 0; i < renderables.Count; i++)
            {
                Renderable renderable = renderables[i];
                Render(renderable);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Render(Renderable renderable)
        {
            if (renderable.m_shader == null || renderable.m_mesh == null)
                return;

            renderable.m_modelMatrix = Matrix4.CreateFromQuaternion(renderable.m_transform.localRotation);
            renderable.m_modelMatrix *= Matrix4.CreateTranslation(renderable.m_transform.localPosition);
            renderable.m_modelMatrix *= Matrix4.CreateScale(renderable.m_transform.localScale);

            renderable.m_shader.SetMatrix4("uModel", renderable.m_modelMatrix);
            renderable.m_shader.Use();

            renderable.m_mesh.Render();
        }
    }
}
