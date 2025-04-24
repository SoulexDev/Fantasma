using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Fantasma.Graphics
{
    public struct Renderable
    {
        public Matrix4 modelMatrix;
        public Transform transform;
        public Shader shader;
        public Mesh mesh;

        public Renderable(Transform transform, Mesh mesh, Shader shader)
        {
            this.transform = transform;
            this.mesh = mesh;
            this.shader = shader;
            modelMatrix = new Matrix4();
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
            if (renderable.shader == null || renderable.mesh == null)
                return;

            renderable.modelMatrix = Matrix4.CreateFromQuaternion(renderable.transform.rotation);
            renderable.modelMatrix *= Matrix4.CreateTranslation(renderable.transform.position);
            renderable.modelMatrix *= Matrix4.CreateScale(renderable.transform.scale);

            renderable.shader.SetMatrix4("uModel", renderable.modelMatrix);
            renderable.shader.Use();

            renderable.mesh.Render();
        }
    }
}
