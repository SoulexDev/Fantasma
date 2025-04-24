using Fantasma.Generation;
using Fantasma.Globals;
using OpenTK.Mathematics;

namespace Fantasma.Data
{
    public struct BlockEvent
    {
        public int remainingTicks;

        public Vector3i eventCoordinate;
        public Vector3i affectedCoordinate;

        public SubChunk eventChunk;
        public SubChunk affectedChunk;

        public Block eventBlock;
        public Block affectedBlock;

        public int eventBlockIndex;
        public int affectedBlockIndex;

        public BlockEventType blockEventType;

        public Vector3i relativeEventCoordinate => eventCoordinate - affectedCoordinate;

        public BlockEvent(BlockEventType blockEventType, int remainingTicks, Vector3i eventCoordinate, Vector3i affectedCoordinate, SubChunk eventChunk, SubChunk affectedChunk, 
            Block eventBlock, Block affectedBlock, int eventBlockIndex, int affectedBlockIndex)
        {
            this.blockEventType = blockEventType;
            this.remainingTicks = remainingTicks;
            this.eventCoordinate = eventCoordinate;
            this.affectedCoordinate = affectedCoordinate;
            this.eventChunk = eventChunk;
            this.affectedChunk = affectedChunk;
            this.eventBlock = eventBlock;
            this.affectedBlock = affectedBlock;
            this.eventBlockIndex = eventBlockIndex;
            this.affectedBlockIndex = affectedBlockIndex;
        }
    }
}
