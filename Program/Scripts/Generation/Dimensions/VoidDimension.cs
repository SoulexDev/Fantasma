using Fantasma.Globals;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Generation
{
    public class VoidDimension : Dimension
    {
        public override void Initiate()
        {

        }
        public override BlockType GetDimensionBlock(Vector3 position)
        {
            return BlockType.Grass;
        }
    }
}
