using Fantasma.Globals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Data
{
    public class BlockDirt : Block
    {
        public BlockDirt(BlockType blockType) : base(blockType) { }
        public override void OnBlockEvent(BlockEvent blockEvent)
        {
            Console.WriteLine($"relevent coord: {blockEvent.relativeEventCoordinate}, event block type: {blockEvent.eventChunk.m_blocks[blockEvent.eventBlockIndex]}");
            if(blockEvent.relativeEventCoordinate.Y == 1 && blockEvent.eventChunk.m_blocks[blockEvent.eventBlockIndex] == m_blockType)
            {
                blockEvent.affectedChunk.m_blocks[blockEvent.affectedBlockIndex] = BlockType.Air;
            }
        }
    }
}
