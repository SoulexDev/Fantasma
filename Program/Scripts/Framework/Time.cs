using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Framework
{
    public class Time
    {
        public static float m_deltaTime { get; private set; }
        public static float m_fixedDeltaTime { get; private set; }
        private static float m_currentTime;
        private static float m_lastTime;

        public void SetDeltaTime(float deltaTime)
        {
            m_deltaTime = deltaTime;
        }
        public void SetFixedDeltaTime(float deltaTime)
        {
            m_fixedDeltaTime = deltaTime;
        }
    }
}
