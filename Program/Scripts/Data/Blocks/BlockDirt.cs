using Fantasma.Generation;
using Fantasma.Globals;

namespace Fantasma.Data
{
    public class BlockDirt : Block
    {
        public BlockDirt(BlockType blockType) : base(blockType) { }
        public override void QueueAnyBlockEvents(BlockEvent blockEvent)
        {
            if (HasBlockEvent(blockEvent))
            {
                WorldWorkManager.DoChainBlockEvent(BlockEventType.Break, blockEvent.affectedChunk.m_position, blockEvent.affectedCoordinate, blockEvent.affectedChunk, blockEvent.affectedBlockIndex);
            }
        }
        public override void OnBlockEvent(BlockEvent blockEvent)
        {
            if(HasBlockEvent(blockEvent))
            {
                blockEvent.affectedChunk.m_blocks[blockEvent.affectedBlockIndex] = BlockType.Air;
            }
        }
        public override bool HasBlockEvent(BlockEvent blockEvent)
        {
            return blockEvent.relativeEventCoordinate.Y == 1 && blockEvent.eventChunk.m_blocks[blockEvent.eventBlockIndex] == m_blockType && 
                blockEvent.blockEventType == BlockEventType.Break;
        }
    }
}
