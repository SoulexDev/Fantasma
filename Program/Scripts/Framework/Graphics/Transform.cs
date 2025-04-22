using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Fantasma.Graphics
{
    public class Transform
    {
        private Transform m_parent;
        private List<Transform> m_children = new List<Transform>();

        public Vector3 localPosition = Vector3.Zero;
        public Quaternion localRotation = Quaternion.Identity;
        public Vector3 localScale = Vector3.One;

        public Vector3 position
        {
            get
            {
                if (m_parent == null)
                    return localPosition;

                return m_parent.position + localPosition;
            }
            set
            {
                if (m_parent == null)
                {
                    localPosition = value;
                    return;
                }

                localPosition = value - m_parent.position;
            }
        }
        public Quaternion rotation
        {
            get
            {
                if(m_parent == null)
                    return localRotation;

                return m_parent.rotation * localRotation;
            }
            set
            {
                if (m_parent == null)
                {
                    localRotation = value;
                    return;
                }

                localRotation = value * m_parent.rotation.Inverted();
            }
        }
        public Vector3 scale
        {
            get
            {
                if (m_parent == null)
                    return localScale;

                return m_parent.scale * localScale;
            }
            set
            {
                if (m_parent == null)
                {
                    localScale = value;
                    return;
                }

                if (m_parent.scale.LengthSquared == 0)
                    return;

                localScale = value / m_parent.scale;
            }
        }
        public Vector3 right
        {
            get { return Vector3.Transform(Vector3.UnitX, rotation); }
            private set { }
        }
        public Vector3 up
        {
            get { return Vector3.Transform(Vector3.UnitY, rotation); }
            private set { }
        }
        public Vector3 forward
        {
            get { return Vector3.Transform(Vector3.UnitZ, rotation); }
            private set { }
        }
        public void SetParent(Transform transform, bool preserveWorld = false)
        {
            if (preserveWorld)
            {
                if(transform == null)
                {
                    localPosition += m_parent.position;
                    localRotation *= m_parent.rotation;

                    if (m_parent == null)
                        return;

                    m_parent.m_children.Remove(this);
                    m_parent = null;
                    return;
                }

                localPosition -= m_parent.position;
                localRotation *= m_parent.rotation.Inverted();

                m_parent = transform;
                m_parent.m_children.Add(this);
            }
            else
            {
                if(transform == null)
                {
                    if (m_parent == null)
                        return;

                    m_parent.m_children.Remove(this);
                    m_parent = null;
                }
                else
                {
                    m_parent = transform;
                    m_parent.m_children.Add(this);
                }
            }
        }
    }
}
