using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Framework
{
    public class Input
    {
        private static KeyboardState m_keyboardState;
        private static MouseState m_mouseState;

        public static float Horizontal { get; private set; }
        public static float Vertical { get; private set; }
        public static float MouseX { get; private set; }
        public static float MouseY { get; private set; }
        public static Vector2 ScreenMousePosition { get; private set; }

        public void SetKeyboardState(KeyboardState state)
        {
            m_keyboardState = state;
        }
        public void SetMouseState(MouseState state)
        {
            m_mouseState = state;
        }
        public void SetInputVariables()
        {
            int horizontalPos = GetKey(Keys.D) ? 1 : 0;
            int horizontalNeg = GetKey(Keys.A) ? -1 : 0;

            int verticalPos = GetKey(Keys.W) ? 1 : 0;
            int verticalNeg = GetKey(Keys.S) ? -1 : 0;

            Horizontal = horizontalPos + horizontalNeg;
            Vertical = verticalPos + verticalNeg;

            MouseX = m_mouseState.Delta.X;
            MouseY = m_mouseState.Delta.Y;

            ScreenMousePosition = m_mouseState.Position;
        }
        public static bool GetKeyDown(Keys key)
        {
            return m_keyboardState.IsKeyPressed(key);
        }
        public static bool GetKeyUp(Keys key)
        {
            return m_keyboardState.IsKeyReleased(key);
        }
        public static bool GetKey(Keys key) 
        {
            return m_keyboardState.IsKeyDown(key);
        }
        public static bool GetMouseDown(MouseButton button)
        {
            return m_mouseState.IsButtonPressed(button);
        }
        public static bool GetMouseUp(MouseButton button)
        {
            return m_mouseState.IsButtonReleased(button);
        }
        public static bool GetMouse(MouseButton button)
        {
            return m_mouseState.IsButtonDown(button);
        }
    }
}
