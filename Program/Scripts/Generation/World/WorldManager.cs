using Fantasma.Data;
using Fantasma.Framework;
using Fantasma.Globals;
using Fantasma.Physics;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Fantasma.Generation
{
    public struct BlockChunkPair
    {
        public Block block;
        public SubChunk subChunk;
        public int blockIndex;

        public bool exists;

        public BlockChunkPair(Block block, SubChunk subChunk, int blockIndex)
        {
            this.block = block;
            this.subChunk = subChunk;
            this.blockIndex = blockIndex;
            exists = true;
        }
    }
    public class WorldManager
    {
        public static WorldManager m_instance;
        public static Dimension m_currentDimension;

        private IdeoniaDimension m_ideonia;
        private VoidDimension m_void;

        public Dictionary<Vector2i, ChunkColumn> m_chunkColumns = new Dictionary<Vector2i, ChunkColumn>();

        public List<ChunkColumn> m_visibleColumns = new List<ChunkColumn>();
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
        //public void GenerateAll()
        //{
        //    Vector3i pos = Vector3i.Zero;
        //    for (int z = -2; z < 2; z++)
        //    {
        //        for (int x = -2; x < 2; x++)
        //        {
        //            pos.X = x;
        //            pos.Z = z;

        //            AddChunk(pos * WorldParameters.m_chunkSize, pos.Xz);
        //        }
        //    }
        //    foreach (var item in m_chunkColumns)
        //    {
        //        item.Value.MeshAll();
        //    }
        //}
        public ChunkColumn AddChunk(Vector3i position, Vector2i chunkPosition)
        {
            ChunkColumn column = new ChunkColumn();
            m_chunkColumns.Add(chunkPosition, column);
            column.AggresiveGenerate(position);

            return column;
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
                        if((m_instance.GetBlockType(x, y, z) & BlockType.NoCollisionFlags) == 0)
                        {
                            colliders.Add(new AABB(x, x + 1, y, y + 1, z, z + 1));
                        }
                    }
                }
            }

            return colliders;
        }
        public static void UpdateVisibility(Vector3i pos)
        {
            int viewDistance = 4;
            Vector3i chunkPos = Vector3i.Zero;

            List<ChunkColumn> newVisibleChunkColumns = new List<ChunkColumn>();
            for (int z = -viewDistance; z <= viewDistance; z++)
            {
                for (int x = -viewDistance; x <= viewDistance; x++)
                {
                    chunkPos.X = x + pos.X;
                    chunkPos.Z = z + pos.Z;

                    if (m_instance.m_chunkColumns.ContainsKey(chunkPos.Xz))
                    {
                        newVisibleChunkColumns.Add(m_instance.m_chunkColumns[chunkPos.Xz]);
                        continue;
                    }

                    ChunkColumn column = m_instance.AddChunk(chunkPos * WorldParameters.m_chunkSize, chunkPos.Xz);
                    newVisibleChunkColumns.Add(column);
                }
            }

            m_instance.m_visibleColumns.ForEach(c =>
            {
                if (!newVisibleChunkColumns.Contains(c))
                    c.Dispose();
            });
            m_instance.m_visibleColumns.Clear();

            m_instance.m_visibleColumns = newVisibleChunkColumns;
            foreach (var column in m_instance.m_visibleColumns)
            {
                if (!column.m_generated)
                    column.MeshAll();

                float timer = 0;
                while (timer < 0.2f)
                {
                    timer += Time.m_deltaTime;
                }
            }
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
        public static RaycastHit GetRayHit(Ray ray)
        {
            BlockType blockType = 0;
            Vector3i mapPos = CoordinateUtils.LocalVoxelCoord(ray.origin);

            blockType = m_instance.GetBlockType(mapPos);
            if ((blockType & BlockType.CantBreakFlags) == 0)
                return new RaycastHit(Vector3.Zero, mapPos, mapPos, 0, blockType);

            Vector3i rayStep = ray.direction.Sign();
            Vector3 deltaDist = Vector3.Abs(new Vector3(ray.direction.Length) / ray.direction);
            Vector3 sideDist = (rayStep * (mapPos - ray.origin) + (rayStep.ToVector3() * 0.5f) + Vector3.One * 0.5f) * deltaDist;
            Bool3 mask;

            for (int i = 0; i < 8; i++)
            {
                mask = Bool3.LessThanEqualTo(sideDist, Vector3.ComponentMin(sideDist.Yzx, sideDist.Zxy));

                sideDist += (Vector3)mask * deltaDist;
                mapPos += (Vector3i)mask * rayStep;

                blockType = m_instance.GetBlockType(mapPos);
                if ((blockType & BlockType.CantBreakFlags) == 0)
                {
                    Vector3 normal = (-rayStep) * (Vector3)mask;
                    float distance = Vector3.Dot(normal, mapPos + Vector3.ComponentMax(Vector3.Zero, normal) - ray.origin) / Vector3.Dot(normal, ray.direction);

                    return new RaycastHit(normal, mapPos, ray.origin + ray.direction * distance, distance, blockType);
                }
            }
            return new RaycastHit();
        }
        public BlockChunkPair GetBlockChunkPair(Vector3i pos)
        {
            Vector3i chunkPos = CoordinateUtils.LocalChunkCoord(pos);
            Vector3i voxelPos = pos - chunkPos * WorldParameters.m_chunkSize;

            if (ChunkExists(chunkPos))
            {
                SubChunk subChunk = m_chunkColumns[chunkPos.Xz].GetChunk(chunkPos.Y);
                int blockIndex = CoordinateUtils.ThreeToIndex(voxelPos, WorldParameters.m_chunkSize);
                BlockType blockType = subChunk.m_blocks[blockIndex];

                return new BlockChunkPair(Block.m_blocks[blockType], subChunk, blockIndex);
            }
            else
                return new BlockChunkPair();
        }
        public Block GetBlock(Vector3i pos)
        {
            return Block.m_blocks[GetBlockType(pos)];
        }
        public Block GetBlock(int x, int y, int z)
        {
            Vector3i pos = new Vector3i(x, y, z);

            return Block.m_blocks[GetBlockType(pos)];
        }
        public BlockType GetBlockType(int x, int y, int z)
        {
            Vector3i pos = new Vector3i(x, y, z);

            return GetBlockType(pos);
        }
        public BlockType GetBlockType(Vector3i pos)
        {
            Vector3i chunkPos = CoordinateUtils.LocalChunkCoord(pos);

            return GetBlockType(chunkPos, pos - chunkPos * WorldParameters.m_chunkSize);
        }
        public BlockType GetBlockType(Vector3i chunkPos, Vector3i voxelPos)
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
    }
}
