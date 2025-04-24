using Fantasma.Data;
using Fantasma.Globals;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fantasma.Generation
{
    public class WorldWorkManager
    {
        private static WorldWorkManager m_instance;
        public List<ChunkColumn> m_generationQueue = new List<ChunkColumn>();
        public List<SubChunk> m_meshingQueue = new List<SubChunk>();
        public List<SubChunk> m_setMeshQueue = new List<SubChunk>();

        private List<BlockEvent> m_blockEvents = new List<BlockEvent>();

        private bool m_chunkGenerationToBeDone = false;
        private bool m_chunkMeshingToBeDone = false;
        private bool m_blockEventWorkToBeDone = false;

        private bool m_canDoBlockEvents = false;

        public WorldWorkManager()
        {
            m_instance = this;
            Core.OnTick += HandleBlockEvents;
        }
        public static void DoSourceBlockEvent(BlockEventType blockEventType, Vector3i eventChunkPos, Vector3i eventVoxelPos, SubChunk eventChunk, int eventBlockIndex)
        {
            DoChainBlockEvent(blockEventType, eventChunkPos, eventVoxelPos, eventChunk, eventBlockIndex);
            m_instance.m_canDoBlockEvents = true;
        }
        public static void DoChainBlockEvent(BlockEventType blockEventType, Vector3i eventChunkPos, Vector3i eventVoxelPos, SubChunk eventChunk, int eventBlockIndex)
        {
            Vector3i otherVoxelPos;

            for (int i = 0; i < 6; i++)
            {
                otherVoxelPos = eventChunkPos + eventVoxelPos + GlobalCoordinates.m_faceDirections[i];

                BlockChunkPair pair = WorldManager.m_instance.GetBlockChunkPair(otherVoxelPos);
                if (!pair.exists)
                    continue;

                BlockEvent blockEvent = new BlockEvent(blockEventType, 0, eventVoxelPos, otherVoxelPos, eventChunk, pair.subChunk,
                    Block.m_blockRegistry[eventChunk.m_blocks[eventBlockIndex]], pair.block, eventBlockIndex, pair.blockIndex);

                pair.block.QueueAnyBlockEvents(blockEvent);
                //QueueBlockEvent(blockEvent);
            }
        }
        public void HandleBlockEvents()
        {
            if (m_blockEventWorkToBeDone && m_canDoBlockEvents)
            {
                m_blockEvents.ForEach(blockEvent =>
                {
                    if (blockEvent.remainingTicks == 0)
                        blockEvent.affectedBlock.OnBlockEvent(blockEvent);
                    else
                        blockEvent.remainingTicks--;
                });
                if (m_blockEvents.Count == 0)
                {
                    m_blockEventWorkToBeDone = false;
                    m_canDoBlockEvents = false;
                }
            }
        }
        public void HandleChunkGeneration()
        {
            while (m_chunkGenerationToBeDone)
            {
                Task a = m_generationQueue[0].GenerateData();
                while(!a.IsCompleted) { }
                WorldManager.AddChunk(m_generationQueue[0]);
                m_generationQueue[0].m_chunks.ForEach(c=> QueueChunkMeshing(c));
                m_generationQueue.RemoveAt(0);
                if (m_generationQueue.Count == 0)
                    m_chunkGenerationToBeDone = false;
            }
        }
        public void HandleChunkMeshing()
        {
            while (m_chunkMeshingToBeDone)
            {
                Task a = Task.Run(()=>m_meshingQueue[0].MeshChunk());
                while(!a.IsCompleted) { }
                m_setMeshQueue.Add(m_meshingQueue[0]);
                m_meshingQueue.RemoveAt(0);
                if (m_meshingQueue.Count == 0)
                    m_chunkMeshingToBeDone = false;
            }
            m_setMeshQueue.ForEach(c=>c.SetMesh());
            m_setMeshQueue.Clear();
        }
        public static void QueueBlockEvent(BlockEvent blockEvent)
        {
            m_instance.InternalQueueBlockEvent(blockEvent);
        }
        public void InternalQueueBlockEvent(BlockEvent blockEvent)
        {
            m_blockEvents.Add(blockEvent);

            if (!m_blockEventWorkToBeDone)
                m_blockEventWorkToBeDone = true;
        }
        public static void QueueChunkGeneration(ChunkColumn chunkColumn)
        {
            m_instance.InternalQueueChunkGeneration(chunkColumn);
        }
        public void InternalQueueChunkGeneration(ChunkColumn chunkColumn)
        {
            m_generationQueue.Add(chunkColumn);
            if (!m_chunkGenerationToBeDone)
            {
                m_chunkGenerationToBeDone = true;
                HandleChunkGeneration();
            }
        }
        public static void QueueChunkMeshing(SubChunk chunk)
        {
            m_instance.InternalQueueChunkMeshing(chunk);
        }
        public static void QueueChunkMeshingPriority(SubChunk chunk)
        {
            m_instance.InternalQueueChunkMeshingPriority(chunk);
        }
        public void InternalQueueChunkMeshing(SubChunk chunk)
        {
            m_meshingQueue.Add(chunk);
            if (!m_chunkMeshingToBeDone)
            {
                m_chunkMeshingToBeDone = true;
                HandleChunkMeshing();
            }
        }
        public void InternalQueueChunkMeshingPriority(SubChunk chunk)
        {
            m_meshingQueue.Insert(0, chunk);
            if (!m_chunkMeshingToBeDone)
            {
                m_chunkMeshingToBeDone = true;
                HandleChunkMeshing();
            }
        }
    }
}
