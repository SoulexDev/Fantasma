using Fantasma.Data;
using Fantasma.Globals;
using Fantasma.Physics;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Fantasma.Generation
{
    public class WorldManager
    {
        public static WorldManager m_instance;
        public static Dimension m_currentDimension;

        private IdeoniaDimension m_ideonia;
        private VoidDimension m_void;

        public Dictionary<Vector2i, ChunkColumn> m_chunkColumns = new Dictionary<Vector2i, ChunkColumn>();
        public WorldManager()
        {
            m_instance = this;

            m_ideonia = new IdeoniaDimension();
            m_void = new VoidDimension();

            SetDimension(DimensionType.Ideonia);
        }
        public void SetDimension(DimensionType dimensionType)
        {
            switch (dimensionType)
            {
                case DimensionType.Ideonia:
                    m_currentDimension = m_ideonia;
                    break;
                case DimensionType.Void:
                    m_currentDimension = m_void;
                    break;
                default:
                    break;
            }
        }
        public void GenerateAll()
        {
            Vector3i pos = Vector3i.Zero;
            for (int z = -2; z < 2; z++)
            {
                for (int x = -2; x < 2; x++)
                {
                    pos.X = x;
                    pos.Z = z;

                    AddChunk(pos * WorldParameters.m_chunkSize, pos.Xz);
                }
            }
            foreach (var item in m_chunkColumns)
            {
                item.Value.MeshAll();
            }
        }
        public void AddChunk(Vector3i position, Vector2i chunkPosition)
        {
            ChunkColumn column = new ChunkColumn();
            m_chunkColumns.Add(chunkPosition, column);
            column.AggresiveGenerate(position);
        }
        public static List<AABB> GetColliders(AABB bounds)
        {
            List<AABB> colliders = new List<AABB>();
            for (int z = (int)bounds.m_minZ; z <= (int)bounds.m_maxZ; z++)
            {
                for (int y = (int)bounds.m_minY; y <= (int)bounds.m_maxY; y++)
                {
                    for (int x = (int)bounds.m_minX; x <= (int)bounds.m_maxX; x++)
                    {
                        if((m_instance.GetBlock(x, y, z) & BlockType.NoCollisionFlags) == 0)
                        {
                            colliders.Add(new AABB(x, x + 1, y, y + 1, z, z + 1));
                        }
                    }
                }
            }

            return colliders;
        }
        public static void ChangeBlock(Vector3i worldPos, BlockType block)
        {
            Vector3i chunkPos = CoordinateUtils.LocalChunkCoord(worldPos);
            SubChunk chunk = GetChunk(chunkPos);
            if (chunk == null)
                return;

            Vector3i voxelPos = worldPos - chunkPos * WorldParameters.m_chunkSize;

            chunk.ChangeBlock(voxelPos, block);
        }
        public BlockData GetBlockData(Vector3i pos)
        {
            return BlockDataManager.m_blockData[GetBlock(pos)];
        }
        public BlockData GetBlockData(int x, int y, int z)
        {
            Vector3i pos = new Vector3i(x, y, z);

            return BlockDataManager.m_blockData[GetBlock(pos)];
        }
        public BlockType GetBlock(int x, int y, int z)
        {
            Vector3i pos = new Vector3i(x, y, z);

            return GetBlock(pos);
        }
        public static HitResult GetHitResult(Vector3i pos)
        {
            HitResult result = new HitResult();
            result.m_hitPos = pos;
            return SetResult(pos, result);
        }
        public static HitResult SetResult(Vector3i pos, HitResult result)
        {
            Vector3 normalized = pos.ToVector3() / WorldParameters.m_chunkSize;
            Vector3i chunkPos = new Vector3i().FromVector3(normalized);

            if (ChunkExists(chunkPos))
            {
                return SetResult(chunkPos, pos - chunkPos * WorldParameters.m_chunkSize, result);
            }
            return result;
        }
        public static HitResult SetResult(Vector3i chunkPos, Vector3i voxelPos, HitResult result)
        {
            result.m_chunk = GetChunk(chunkPos);
            result.m_blockIndex = CoordinateUtils.ThreeToIndex(voxelPos.X, voxelPos.Y, voxelPos.Z, WorldParameters.m_chunkSize);
            result.m_block = result.m_chunk.m_blocks[result.m_blockIndex];

            if ((result.m_block & BlockType.NoCollisionFlags) != result.m_block)
                result.m_hit = true;
            return result;
        }
        public BlockType GetBlock(Vector3i pos)
        {
            Vector3i chunkPos = CoordinateUtils.LocalChunkCoord(pos);

            return GetBlock(chunkPos, pos - chunkPos * WorldParameters.m_chunkSize);
        }
        public BlockType GetBlock(Vector3i chunkPos, Vector3i voxelPos)
        {
            if (ChunkExists(chunkPos))
                return m_chunkColumns[chunkPos.Xz].GetChunk(chunkPos.Y).m_blocks[CoordinateUtils.ThreeToIndex(voxelPos, WorldParameters.m_chunkSize)];
            else
                return BlockType.Nothing;
        }
        public static bool ChunkExists(Vector3i pos)
        {
            return m_instance.m_chunkColumns.ContainsKey(pos.Xz) && m_instance.m_chunkColumns[pos.Xz].ContainsChunk(pos.Y);
        }
        public static SubChunk GetChunk(Vector3i pos)
        {
            if (ChunkExists(pos))
                return m_instance.m_chunkColumns[pos.Xz].GetChunk(pos.Y);
            return null;
        }
        public void Render()
        {
            foreach (var pair in m_chunkColumns)
            {
                pair.Value.Render();
            }
        }
    }
}
