using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Fantasma.Generation
{
    public class ChunkColumn
    {
        public List<SubChunk> m_chunks = new List<SubChunk>();
        public Vector2i m_position;

        public bool m_generating;
        public bool m_generated;

        public bool m_meshed;

        public ChunkColumn(Vector2i position)
        {
            m_position = position;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SubChunk GetChunk(int i)
        {
            return m_chunks[i];
        }
        public int GetIndex(SubChunk chunk)
        {
            if (m_chunks.Contains(chunk))
                return m_chunks.IndexOf(chunk);
            else
                return -1;
        }
        public bool ContainsChunk(int i)
        {
            return i > -1 && i < m_chunks.Count;
        }
        public async Task GenerateData()
        {
            Vector3i chunkPosition = Vector3i.Zero;
            chunkPosition.X = m_position.X;
            chunkPosition.Z = m_position.Y;
            for (int i = 0; i < 4; i++)
            {
                SubChunk chunk = new SubChunk(chunkPosition, WorldManager.m_instance);
                m_chunks.Add(chunk);
                await Task.Run(() => chunk.GenerateChunkData());

                chunkPosition.Y = i * WorldParameters.m_chunkSize;
            }
            m_generated = true;
        }
        public void Dispose()
        {
            return;
            foreach (SubChunk chunk in m_chunks)
            {
                chunk.Dispose();
            }
        }
    }
}
