using Fantasma.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Framework
{
    public class FantasmaObject
    {
        public Transform m_transform;
        public MeshRenderer m_meshRenderer;

        public FantasmaObject()
        {
            Core.m_objects.Add(this);
            m_transform = new Transform();
            Awake();
        }
        public virtual void Awake()
        {

        }
        public virtual void Start()
        {

        }
        public virtual void Update()
        {

        }
        public virtual void OnRender()
        {

        }
        public virtual void Dispose(bool removeFromCoreObjects = true)
        {
            if(removeFromCoreObjects)
                Core.m_objects.Remove(this);
        }
    }
}
