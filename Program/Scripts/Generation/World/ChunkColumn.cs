using Fantasma.Framework;
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

        public bool m_generating;
        public bool m_generated;

        public bool m_meshed;

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
        public void GenerateAllData()
        {
            
        }
        public void GenerateData()
        {

        }
        public void MeshSubChunk()
        {
            
        }
        public async Task AggresiveGenerate(Vector3i position)
        {
            for (int i = 0; i < 4; i++)
            {
                SubChunk chunk = new SubChunk(position, WorldManager.m_instance);
                m_chunks.Add(chunk);
                await chunk.ForceGenerate();

                position.Y += WorldParameters.m_chunkSize;
            }
            m_generated = true;
        }
        public void MeshAll()
        {
            foreach (SubChunk chunk in m_chunks)
            {
                chunk.MeshChunk(null);
            }
            m_meshed = true;
        }
        //public void SetAllMeshes()
        //{
        //    foreach (SubChunk chunk in m_chunks)
        //    {
        //        chunk.SetMesh();
        //    }
        //}
        public void Dispose()
        {
            foreach (SubChunk chunk in m_chunks)
            {
                chunk.Dispose();
            }
        }
    }
}
