using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Player
{
    public class PlayerController
    {
        private Vector3 m_position;
        private Vector3 m_cameraTarget = Vector3.Zero;
        private Vector3 m_cameraDirection;
        private Vector3 m_cameraUp;
        private Vector3 m_cameraRight;
        private Vector3 m_forward = new Vector3(0, 0, 1);
        private Matrix4 m_view;

        public void OnLoad()
        {
            m_cameraDirection = Vector3.Normalize(m_cameraTarget - m_position);
            m_cameraRight = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, m_cameraDirection));
            m_cameraUp = Vector3.Normalize(Vector3.Cross(m_cameraDirection, m_cameraRight)); 
        }
        public void OnUpdateFrame(FrameEventArgs args, bool IsFocused, KeyboardState input)
        {
            if (!IsFocused)
                return;

            if (input.IsKeyDown(Keys.W))
            {
                m_position += m_forward * 100.5f;
            }
            if (input.IsKeyDown(Keys.S))
            {
                m_position -= m_forward * 100.5f;
            }
            if (input.IsKeyDown(Keys.A))
            {
                m_position -= m_cameraRight * 100.5f;
            }
            if (input.IsKeyDown(Keys.D))
            {
                m_position += m_cameraRight * 100.5f;
            }
            if (input.IsKeyDown(Keys.Space))
            {
                m_position += m_cameraUp * 100.5f;
            }
            if (input.IsKeyDown(Keys.W))
            {
                m_position -= m_cameraUp * 100.5f;
            }

            m_view = Matrix4.LookAt(m_position, m_position + m_forward, m_cameraUp);
        }
    }
}
