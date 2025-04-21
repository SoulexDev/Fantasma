using Fantasma.Framework;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Graphics
{
    public class Transform : Component
    {
        public Vector3 position = Vector3.Zero;
        public Quaternion rotation = Quaternion.Identity;
        public Vector3 scale = Vector3.One;

        public Vector3 m_right
        {
            get { return Vector3.Transform(Vector3.UnitX, rotation); }
            private set { }
        }
        public Vector3 m_up
        {
            get { return Vector3.Transform(Vector3.UnitY, rotation); }
            private set { }
        }
        public Vector3 m_forward
        {
            get { return Vector3.Transform(Vector3.UnitZ, rotation); }
            private set { }
        }
    }
}
