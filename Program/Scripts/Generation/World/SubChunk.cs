using Fantasma.Data;
using Fantasma.Framework;
using Fantasma.Globals;
using Fantasma.Graphics;
using OpenTK.Mathematics;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fantasma.Generation
{
    public class SubChunk : FantasmaObject
    {
        public bool m_dataGenerated;
        public bool m_meshed;
        public BlockType[] m_blocks;
        private WorldManager m_worldManager;

        private Vector3i m_position;
        private bool m_allAir = true;

        private Renderable m_opaque;
        private Renderable m_transparent;

        private MeshData m_opaqueMeshData;
        private MeshData m_transparentMeshData;

        public bool readyToSetMesh;

        public SubChunk(Vector3i position, WorldManager worldManager)
        {
            m_meshed = false;
            m_position = position;

            m_blocks = new BlockType[WorldParameters.m_chunkSizeCbd];
            m_worldManager = worldManager;

            m_meshRenderer = new MeshRenderer();

            m_opaque = RenderableFactory.RegisterRenderable(m_transform, new Mesh(), ShaderContainer.m_standardShader, RenderableType.Opaque);
            m_transparent = RenderableFactory.RegisterRenderable(m_transform, new Mesh(), ShaderContainer.m_standardTransparentShader, RenderableType.Transparent);
        }
        public void ForceGenerate()
        {
            for (int i = 0; i < WorldParameters.m_chunkSizeCbd; i++)
            {
                m_blocks[i] = WorldManager.m_currentDimension.GetDimensionBlock(
                    CoordinateUtils.IndexToThree(i, WorldParameters.m_chunkSize) + m_position);

                if (m_blocks[i] != BlockType.Air)
                    m_allAir = false;
            }
        }
        public void ChangeBlock(Vector3i voxelPos, BlockType blockType)
        {
            m_blocks[CoordinateUtils.ThreeToIndex(voxelPos, WorldParameters.m_chunkSize)] = blockType;

            Vector3i otherVoxelPos;

            //Console.WriteLine($"touched block pos: {voxelPos}");

            for (int i = 0; i < 6; i++)
            {
                otherVoxelPos = m_position + voxelPos + GlobalCoordinates.m_faceDirections[i];

                BlockChunkPair pair = m_worldManager.GetBlockChunkPair(otherVoxelPos);
                if(!pair.exists)
                    continue;

                //Console.WriteLine($"block type: {pair.block.m_blockType}, position: {otherVoxelPos}");
                pair.block.SetBlockEvent(pair.subChunk.m_blocks, m_position + voxelPos, otherVoxelPos, pair.blockIndex);
            }

            if (blockType != BlockType.Air)
                m_allAir = false;

            MeshChunk(null);

            Vector3i relativeNeighborPos;
            relativeNeighborPos.X = voxelPos.X == 15 ? 1 : voxelPos.X == 0 ? -1 : 0;
            relativeNeighborPos.Y = voxelPos.Y == 15 ? 1 : voxelPos.Y == 0 ? -1 : 0;
            relativeNeighborPos.Z = voxelPos.Z == 15 ? 1 : voxelPos.Z == 0 ? -1 : 0;

            if (relativeNeighborPos == Vector3i.Zero)
                return;

            Vector3i thisPos = m_position / WorldParameters.m_chunkSize;

            if(relativeNeighborPos.X != 0)
            {
                SubChunk chunk = WorldManager.GetChunk(new Vector3i(thisPos.X + relativeNeighborPos.X, thisPos.Y, thisPos.Z));
                if (chunk != null)
                    chunk.MeshChunk(null);
            }
            if (relativeNeighborPos.Y != 0)
            {
                SubChunk chunk = WorldManager.GetChunk(new Vector3i(thisPos.X, thisPos.Y + relativeNeighborPos.Y, thisPos.Z));
                if (chunk != null)
                    chunk.MeshChunk(null);
            }
            if (relativeNeighborPos.Z != 0)
            {
                SubChunk chunk = WorldManager.GetChunk(new Vector3i(thisPos.X, thisPos.Y, thisPos.Z + relativeNeighborPos.Z));
                if (chunk != null)
                    chunk.MeshChunk(null);
            }
            if(relativeNeighborPos.X != 0 && relativeNeighborPos.Y != 0)
            {
                SubChunk chunk = WorldManager.GetChunk(new Vector3i(thisPos.X + relativeNeighborPos.X, thisPos.Y + relativeNeighborPos.Y, thisPos.Z));
                if (chunk != null)
                    chunk.MeshChunk(null);
            }
            if(relativeNeighborPos.X != 0 && relativeNeighborPos.Z != 0)
            {
                SubChunk chunk = WorldManager.GetChunk(new Vector3i(thisPos.X + relativeNeighborPos.X, thisPos.Y, thisPos.Z + relativeNeighborPos.Z));
                if (chunk != null)
                    chunk.MeshChunk(null);
            }
            if(relativeNeighborPos.Y != 0 && relativeNeighborPos.Z != 0)
            {
                SubChunk chunk = WorldManager.GetChunk(new Vector3i(thisPos.X, thisPos.Y + relativeNeighborPos.Y, thisPos.Z + relativeNeighborPos.Z));
                if (chunk != null)
                    chunk.MeshChunk(null);
            }
            if(relativeNeighborPos.X != 0 && relativeNeighborPos.Y != 0 && relativeNeighborPos.Z != 0)
            {
                SubChunk chunk = WorldManager.GetChunk(new Vector3i(thisPos.X + relativeNeighborPos.X, thisPos.Y + relativeNeighborPos.Y, thisPos.Z + relativeNeighborPos.Z));
                if (chunk != null)
                    chunk.MeshChunk(null);
            }
        }
        public async Task GenerateData()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(GenerateDataThreaded));

            while (!m_dataGenerated) await Task.Yield();
        }
        private void GenerateDataThreaded(object state)
        {
            m_dataGenerated = false;
            for (int i = 0; i < WorldParameters.m_chunkSizeCbd; i++)
            {
                m_blocks[i] = BlockType.Dirt;
            }
            m_dataGenerated = true;
        }
        public void MeshChunk(object state)
        {
            if (m_allAir)
                return;

            m_meshed = false;

            MeshData m_opaqueMeshData = new MeshData(new float[WorldParameters.m_chunkSizeCbd * WorldParameters.VOXEL_FACE_COUNT *
                WorldParameters.VERTICIES_PER_FACE * WorldParameters.VOXEL_ATTRIBUTES_LENGTH],
                new int[WorldParameters.m_chunkSizeCbd * WorldParameters.VOXEL_FACE_COUNT * WorldParameters.INDICIES_PER_FACE], 
                VertexAttribute.Position, VertexAttribute.Color, VertexAttribute.UV);

            MeshData m_transparentMeshData = new MeshData(new float[WorldParameters.m_chunkSizeCbd * WorldParameters.VOXEL_FACE_COUNT *
                WorldParameters.VERTICIES_PER_FACE * WorldParameters.VOXEL_ATTRIBUTES_LENGTH],
                new int[WorldParameters.m_chunkSizeCbd * WorldParameters.VOXEL_FACE_COUNT * WorldParameters.INDICIES_PER_FACE],
                VertexAttribute.Position, VertexAttribute.Color, VertexAttribute.UV);

            Vector3i pos = new Vector3i();
            Vector3i worldPos = new Vector3i();

            int vertexOffset = 0;
            int vertexIndex = 0;
            int triangleIndex = 0;

            Direction opaqueBitmask;
            Direction checkDir;
            for (int z = 0; z < WorldParameters.m_chunkSize; z++)
            {
                for (int y = 0; y < WorldParameters.m_chunkSize; y++)
                {
                    for (int x = 0; x < WorldParameters.m_chunkSize; x++)
                    {
                        opaqueBitmask = 0;
                        pos.X = x;
                        pos.Y = y;
                        pos.Z = z;

                        BlockType thisBlock = m_blocks[CoordinateUtils.ThreeToIndex(pos, WorldParameters.m_chunkSize)];

                        if (thisBlock == BlockType.Air || thisBlock == BlockType.Nothing)
                            continue;

                        Block thisData = Block.m_blocks[thisBlock];
                        worldPos = m_position + pos;

                        if ((thisBlock & BlockType.BillboardFlags) == thisBlock)
                        {
                            BillboardModel.Build(worldPos, m_transparentMeshData, ref vertexOffset, ref triangleIndex, ref vertexIndex,
                                thisData.m_uvs[0]);
                            continue;
                        }

                        Block dataForward = m_worldManager.GetBlock(worldPos.X, worldPos.Y, worldPos.Z + 1);
                        Block dataRight = m_worldManager.GetBlock(worldPos.X + 1, worldPos.Y, worldPos.Z);
                        Block dataBack = m_worldManager.GetBlock(worldPos.X, worldPos.Y, worldPos.Z - 1);
                        Block dataLeft = m_worldManager.GetBlock(worldPos.X - 1, worldPos.Y, worldPos.Z);
                        Block dataUp = m_worldManager.GetBlock(worldPos.X, worldPos.Y + 1, worldPos.Z);
                        Block dataDown = m_worldManager.GetBlock(worldPos.X, worldPos.Y - 1, worldPos.Z);

                        opaqueBitmask |= (dataForward.m_opaque || (dataForward.m_cullsSelf && dataForward.m_blockType == thisBlock)) ? Direction.Forward : 0;
                        opaqueBitmask |= (dataRight.m_opaque || (dataRight.m_cullsSelf && dataRight.m_blockType == thisBlock)) ? Direction.Right : 0;
                        opaqueBitmask |= (dataBack.m_opaque || (dataBack.m_cullsSelf && dataBack.m_blockType == thisBlock)) ? Direction.Back : 0;
                        opaqueBitmask |= (dataLeft.m_opaque || (dataLeft.m_cullsSelf && dataLeft.m_blockType == thisBlock)) ? Direction.Left : 0;
                        opaqueBitmask |= (dataUp.m_opaque || (dataUp.m_cullsSelf && dataUp.m_blockType == thisBlock)) ? Direction.Up : 0;
                        opaqueBitmask |= (dataDown.m_opaque || (dataDown.m_cullsSelf && dataDown.m_blockType == thisBlock)) ? Direction.Down : 0;

                        if (opaqueBitmask == Direction.All)
                            continue;

                        if ((thisBlock & BlockType.TransparentFlags) == 0)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                checkDir = (Direction)(1 << i);
                                if ((opaqueBitmask & checkDir) == 0)
                                {
                                    VoxelModel.BuildFace(worldPos, m_opaqueMeshData, ref vertexOffset, ref triangleIndex, ref vertexIndex, 
                                        thisData.m_uvs[i], i, m_worldManager);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                checkDir = (Direction)(1 << i);
                                if ((opaqueBitmask & checkDir) == 0)
                                {
                                    VoxelModel.BuildFace(worldPos, m_transparentMeshData, ref vertexOffset, ref triangleIndex, ref vertexIndex, 
                                        thisData.m_uvs[i], i, m_worldManager);
                                }
                            }
                        }
                    }
                }
            }

            m_opaque.mesh.Set(m_opaqueMeshData);
            m_transparent.mesh.Set(m_transparentMeshData);
            m_meshed = true;
        }
        //public void SetMesh()
        //{
        //    if (m_allAir)
        //        return;

        //    if(m_opaqueMeshData.notEmpty)
        //        m_opaque.mesh.Set(m_opaqueMeshData);

        //    if (m_transparentMeshData.notEmpty)
        //        m_transparent.mesh.Set(m_transparentMeshData);
        //    m_meshed = true;
        //}
        public void Dispose()
        {
            RenderableFactory.UnRegisterRenderable(m_opaque, RenderableType.Opaque);
            RenderableFactory.UnRegisterRenderable(m_transparent, RenderableType.Transparent);

            m_opaque.mesh.Dispose();
        }
    }
}
