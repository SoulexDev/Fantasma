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
        //public List<Component> m_components = new List<Component>();

        public FantasmaObject()
        {
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
        public virtual void Dispose()
        {
            m_meshRenderer.Dispose();
        }
    }
}
