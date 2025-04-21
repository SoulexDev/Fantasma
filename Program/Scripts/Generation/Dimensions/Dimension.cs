using Fantasma.Globals;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Generation
{
    public abstract class Dimension
    {
        public abstract void Initiate();
        public abstract BlockType GetDimensionBlock(Vector3 position);
    }
}
