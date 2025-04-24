using Fantasma.Globals;
using System.Collections.Generic;

namespace Fantasma.Graphics
{
    public class RenderableFactory
    {
        public static List<Renderable> m_opaqueRenderables = new List<Renderable>();
        public static List<Renderable> m_transparentRenderables = new List<Renderable>();

        public static Renderable RegisterRenderable(Transform transform, Mesh mesh, Shader shader, RenderableType renderableType)
        {
            Renderable rend = new Renderable(transform, mesh, shader);

            switch (renderableType)
            {
                case RenderableType.Opaque:
                    m_opaqueRenderables.Add(rend);
                    break;
                case RenderableType.Transparent:
                    m_transparentRenderables.Add(rend);
                    break;
                default:
                    break;
            }

            return rend;
        }
        public static void UnRegisterRenderable(Renderable renderable, RenderableType renderableType)
        {
            switch (renderableType)
            {
                case RenderableType.Opaque:
                    m_opaqueRenderables.Remove(renderable);
                    break;
                case RenderableType.Transparent:
                    m_transparentRenderables.Remove(renderable);
                    break;
                default:
                    break;
            }
        }
        public static void Dispose()
        {
            m_opaqueRenderables.ForEach(r=>r.mesh.Dispose());
            m_transparentRenderables.ForEach(r=>r.mesh.Dispose());
        }
    }
}
