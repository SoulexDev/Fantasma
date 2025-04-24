using Fantasma.Globals;
using OpenTK.Mathematics;
using System;

namespace Fantasma.Data
{
    public class BlockShortGrass : Block
    {
        public BlockShortGrass(BlockType blockType) : base(blockType) { }
        public override void OnBlockEvent(BlockEvent blockEvent)
        {
            if(blockEvent.relativeEventCoordinate.Y == -1)
            {
                blockEvent.affectedChunk.m_blocks[blockEvent.affectedBlockIndex] = BlockType.Air;
            }
        }
    }
}
