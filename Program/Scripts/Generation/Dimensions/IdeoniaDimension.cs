using Fantasma.Globals;
using Icaria.Engine.Procedural;
using OpenTK.Mathematics;

namespace Fantasma.Generation
{
    public class IdeoniaDimension : Dimension
    {
        public override void Initiate()
        {
            
        }
        public override BlockType GetDimensionBlock(Vector3 position)
        {
            int height = (int)(8 + (IcariaNoise.GradientNoise(position.X * 0.07f, position.Z * 0.07f, Core.m_seed) * 0.5f + 1) * 24);

            if (Core.m_random.Next(0, 100) > 70 && position.Y == height + 1)
                return BlockType.ShortGrass;
            else if (position.Y > height)
                return BlockType.Air;
            else if (position.Y == height)
                return BlockType.Grass;
            else if (position.Y > height - 5)
                return BlockType.Dirt;
            else
                return BlockType.Stone;
        }
    }
}
