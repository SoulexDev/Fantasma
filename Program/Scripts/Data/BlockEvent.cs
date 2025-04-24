using Fantasma.Generation;
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
        public int eventBlockIndex;
        public int affectedBlockIndex;

        public Vector3i relativeEventCoordinate => eventCoordinate - affectedCoordinate;

        public BlockEvent(int remainingTicks, Vector3i eventCoordinate, Vector3i affectedCoordinate, SubChunk eventChunk, SubChunk affectedChunk, int eventBlockIndex, int affectedBlockIndex)
        {
            this.remainingTicks = remainingTicks;
            this.eventCoordinate = eventCoordinate;
            this.affectedCoordinate = affectedCoordinate;
            this.eventChunk = eventChunk;
            this.affectedChunk = affectedChunk;
            this.eventBlockIndex = eventBlockIndex;
            this.affectedBlockIndex = affectedBlockIndex;
        }
    }
}
