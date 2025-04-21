using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Framework
{
    public class Time
    {
        public static float m_deltaTime { get; private set; }

        public void SetDeltaTime(float deltaTime)
        {
            Time.m_deltaTime = deltaTime;
        }
    }
}
