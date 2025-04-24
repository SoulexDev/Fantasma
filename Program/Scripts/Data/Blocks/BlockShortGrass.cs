using Fantasma.Globals;
using OpenTK.Mathematics;
using System;

namespace Fantasma.Data
{
    public class BlockShortGrass : Block
    {
        public BlockShortGrass(BlockType blockType) : base(blockType) { }
        public override void OnBlockEvent(BlockType[] fromChunkArray, Vector3i relativeEventCoord, int thisIndex)
        {
            Console.WriteLine(relativeEventCoord);
            if(relativeEventCoord.Y == -1)
            {
                fromChunkArray[thisIndex] = BlockType.Air;
            }
        }
    }
}
