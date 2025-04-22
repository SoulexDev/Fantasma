using Fantasma.Data;
using Fantasma.Framework;
using Fantasma.Globals;
using Fantasma.Graphics;
using OpenTK.Mathematics;
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
                //m_blocks[i] = BlockType.Dirt;
            }
        }
        public void ChangeBlock(Vector3i voxelPos, BlockType block)
        {
            m_blocks[CoordinateUtils.ThreeToIndex(voxelPos, WorldParameters.m_chunkSize)] = block;
            MeshChunk(null);
        }
        public async Task GenerateData()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(GenerateDataThreaded));

            while (!m_dataGenerated) await Task.Yield();

            ThreadPool.QueueUserWorkItem(new WaitCallback(MeshChunk));
        }
        private void GenerateDataThreaded(object state)
        {
            m_dataGenerated = false;
            for (int i = 0; i < WorldParameters.m_chunkSizeCbd; i++)
            {
                //m_blocks[i] = WorldManager.m_currentDimension.GetDimensionBlock(CoordinateUtils.IndexToThree(i, WorldParameters.m_chunkSize));
                m_blocks[i] = BlockType.Dirt;
            }
            m_dataGenerated = true;
        }
        public void MeshChunk(object state)
        {
            if (m_allAir)
                return;
            m_meshed = false;

            MeshData opaqueData = new MeshData(new float[WorldParameters.m_chunkSizeCbd * WorldParameters.VOXEL_FACE_COUNT *
                WorldParameters.VERTICIES_PER_FACE * WorldParameters.VOXEL_ATTRIBUTES_LENGTH],
                new int[WorldParameters.m_chunkSizeCbd * WorldParameters.VOXEL_FACE_COUNT * WorldParameters.INDICIES_PER_FACE], 
                VertexAttribute.Position, VertexAttribute.Color, VertexAttribute.UV);

            MeshData transparentData = new MeshData(new float[WorldParameters.m_chunkSizeCbd * WorldParameters.VOXEL_FACE_COUNT *
                WorldParameters.VERTICIES_PER_FACE * WorldParameters.VOXEL_ATTRIBUTES_LENGTH],
                new int[WorldParameters.m_chunkSizeCbd * WorldParameters.VOXEL_FACE_COUNT * WorldParameters.INDICIES_PER_FACE],
                VertexAttribute.Position, VertexAttribute.Color, VertexAttribute.UV);

            Vector3i pos = new Vector3i();
            Vector3i worldPos = new Vector3i();
            //Vector3i posInt = (Vector3i)m_transform.position;

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

                        BlockData thisData = BlockDataManager.m_blockData[thisBlock];
                        worldPos = m_position + pos;

                        if ((thisBlock & BlockType.BillboardFlags) == thisBlock)
                        {
                            BillboardModel.Build(worldPos, transparentData, ref vertexOffset, ref triangleIndex, ref vertexIndex,
                                thisData.m_uvs[0]);
                            continue;
                        }

                        BlockData dataForward = m_worldManager.GetBlockData(worldPos.X, worldPos.Y, worldPos.Z + 1);
                        BlockData dataRight = m_worldManager.GetBlockData(worldPos.X + 1, worldPos.Y, worldPos.Z);
                        BlockData dataBack = m_worldManager.GetBlockData(worldPos.X, worldPos.Y, worldPos.Z - 1);
                        BlockData dataLeft = m_worldManager.GetBlockData(worldPos.X - 1, worldPos.Y, worldPos.Z);
                        BlockData dataUp = m_worldManager.GetBlockData(worldPos.X, worldPos.Y + 1, worldPos.Z);
                        BlockData dataDown = m_worldManager.GetBlockData(worldPos.X, worldPos.Y - 1, worldPos.Z);

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
                                    VoxelModel.BuildFace(worldPos, opaqueData, ref vertexOffset, ref triangleIndex, ref vertexIndex, 
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
                                    VoxelModel.BuildFace(worldPos, transparentData, ref vertexOffset, ref triangleIndex, ref vertexIndex, 
                                        thisData.m_uvs[i], i, m_worldManager);
                                }
                            }
                        }
                    }
                }
            }

            m_opaque.m_mesh.Set(opaqueData);
            m_transparent.m_mesh.Set(transparentData);
            m_meshed = true;
        }
    }
}
