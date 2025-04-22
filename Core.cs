using Fantasma.Framework;
using Fantasma.Generation;
using Fantasma.Graphics;
using Fantasma.Scripts;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace Fantasma
{
    public class Core : GameWindow
    {
        private static string m_resourcePack = "default";
        public static string m_shadersPath = "../../../Program/Shaders/";
        public static string m_texturesPath = "../../../Program/Assets/resourcepacks/" + m_resourcePack + "/textures/";
        public static float m_aspect;

        public static GameWindow m_window { get; private set; }

        public static List<FantasmaObject> m_objects = new List<FantasmaObject>();
        public static Camera m_currentCamera;
        public static Random m_random;
        public static int m_seed;

        private Time m_time;
        private Input m_input;
        private WorldManager m_worldManager;
        private PlayerController m_playerController;
        private PlayerInteraction m_playerInteraction;

        public Core(int width, int height, string title) :
            base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
        {
            m_aspect = (float)width / height;
            m_window = this;
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.1f, 0.3f, 1, 1);
            GL.Enable(EnableCap.DepthTest);

            ThreadPool.SetMaxThreads(16, 32);

            m_seed = DateTime.Now.Millisecond;
            m_random = new Random(m_seed);

            m_time = new Time();
            m_input = new Input();

            m_worldManager = new WorldManager();
            m_worldManager.GenerateAll();

            m_playerController = new PlayerController();
            m_playerInteraction = new PlayerInteraction();
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            m_time.SetDeltaTime((float)args.Time);
            m_input.SetKeyboardState(KeyboardState);
            m_input.SetMouseState(MouseState);
            m_input.SetInputVariables();

            for (int i = 0; i < m_objects.Count; i++)
            {
                m_objects[i].Update();
            }
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.CornflowerBlue);
            GL.CullFace(TriangleFace.Back);
            GL.DepthFunc(DepthFunction.Lequal);

            for (int i = 0; i < m_objects.Count; i++)
            {
                m_objects[i].OnRender();
            }

            MeshRenderer.Render(RenderableFactory.m_opaqueRenderables);
            MeshRenderer.Render(RenderableFactory.m_transparentRenderables);

            SwapBuffers();

            base.OnRenderFrame(args);
        }
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);

            m_aspect = (float)e.Width / e.Height;
        }
        protected override void OnUnload()
        {
            base.OnUnload();
            ShaderContainer.m_standardShader.Dispose();
            ShaderContainer.m_standardTransparentShader.Dispose();
            ShaderContainer.m_wireShader.Dispose();

            m_objects.ForEach(o=>o.Dispose(false));
            m_objects.Clear();
        }
    }
}
