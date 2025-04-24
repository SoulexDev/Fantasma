using Fantasma.Globals;

namespace Fantasma.Data
{
    public class BlockShortGrass : Block
    {
        public BlockShortGrass(BlockType blockType) : base(blockType) { }
        public override void OnBlockEvent(BlockEvent blockEvent)
        {
            if(HasBlockEvent(blockEvent))
            {
                blockEvent.affectedChunk.m_blocks[blockEvent.affectedBlockIndex] = BlockType.Air;
            }
        }
        public override bool HasBlockEvent(BlockEvent blockEvent)
        {
            return blockEvent.relativeEventCoordinate.Y == -1 && blockEvent.blockEventType == BlockEventType.Break;
        }
    }
}
