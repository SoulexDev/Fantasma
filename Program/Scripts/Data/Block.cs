using Fantasma.Globals;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Fantasma.Data
{
    public class Block
    {
        public static readonly Dictionary<BlockType, Block> m_blocks = new Dictionary<BlockType, Block>();
        public string m_name { get; private set; }
        public string m_lornName { get; private set; }
        public BlockType m_blockType { get; private set; }
        public Vector2i[] m_uvs = new Vector2i[6];
        public int m_toughness { get; private set; }
        public bool m_opaque { get; private set; }
        public bool m_cullsSelf { get; private set; }

        public Block() { }
        public Block(BlockType blockType)
        {
            m_blockType = blockType;
        }
        public Block(int blockType)
        {
            m_blockType = (BlockType)(1 << blockType);
        }
        public void SetBlockEvent(BlockEvent blockEvent)
        {
            OnBlockEvent(blockEvent);
        }
        public virtual void OnBlockEvent(BlockEvent blockEvent)
        {

        }
        private Vector2i UVFromIndex(int uvIndex)
        {
            return new Vector2i(uvIndex % TextureAtlasUtils.m_tileSize, uvIndex / TextureAtlasUtils.m_tileSize);
        }
        public Block SetUVForward(int uvIndex)
        {
            m_uvs[0] = UVFromIndex(uvIndex);
            return this;
        }
        public Block SetUVRight(int uvIndex)
        {
            m_uvs[1] = UVFromIndex(uvIndex);
            return this;
        }
        public Block SetUVBack(int uvIndex)
        {
            m_uvs[2] = UVFromIndex(uvIndex);
            return this;
        }
        public Block SetUVLeft(int uvIndex)
        {
            m_uvs[3] = UVFromIndex(uvIndex);
            return this;
        }
        public Block SetUVUp(int uvIndex)
        {
            m_uvs[4] = UVFromIndex(uvIndex);
            return this;
        }
        public Block SetUVDown(int uvIndex)
        {
            m_uvs[5] = UVFromIndex(uvIndex);
            return this;
        }
        public Block SetUVSides(int uvIndex)
        {
            Vector2i uv = UVFromIndex(uvIndex);
            m_uvs[0] = uv;
            m_uvs[1] = uv;
            m_uvs[2] = uv;
            m_uvs[3] = uv;
            return this;
        }
        public Block SetUVUpDown(int uvIndex)
        {
            Vector2i uv = UVFromIndex(uvIndex);
            m_uvs[4] = uv;
            m_uvs[5] = uv;
            return this;
        }
        public Block SetUVs(int uvIndex)
        {
            Vector2i uv = UVFromIndex(uvIndex);
            m_uvs[0] = uv;
            m_uvs[1] = uv;
            m_uvs[2] = uv;
            m_uvs[3] = uv;
            m_uvs[4] = uv;
            m_uvs[5] = uv;
            return this;
        }
        public Block SetToughness(int toughness)
        {
            m_toughness = toughness;
            return this;
        }
        public Block SetOpaque()
        {
            m_opaque = true;
            return this;
        }
        public Block SetCullsSelf()
        {
            m_cullsSelf = true;
            return this;
        }
        public Block SetName(string name)
        {
            m_name = name;
            return this;
        }
        public Block SetLornName(string lornName)
        {
            m_lornName = lornName;
            return this;
        }
        private static void RegisterBlock(Block block)
        {
            m_blocks.Add(block.m_blockType, block);
        }
        public static void RegisterBlocks()
        {
            RegisterBlock(new Block(BlockType.Nothing).SetOpaque());
            RegisterBlock(new Block(BlockType.Air));
            RegisterBlock(new BlockDirt(BlockType.Dirt).SetOpaque().SetCullsSelf().SetUVs(0).SetToughness(1).SetLornName("dirt").SetName("Dirt"));
            RegisterBlock(new Block(BlockType.Grass).SetOpaque().SetCullsSelf().SetUVSides(1).SetUVUp(2).SetUVDown(0).SetToughness(1).SetLornName("grass").SetName("Grass"));
            RegisterBlock(new Block(BlockType.Wood).SetOpaque().SetCullsSelf().SetUVSides(3).SetUVUpDown(4).SetToughness(2).SetLornName("wood").SetName("Wood"));
            RegisterBlock(new Block(BlockType.Planks).SetOpaque().SetCullsSelf().SetUVs(5).SetToughness(2).SetLornName("woodplanks").SetName("Wood Planks"));
            RegisterBlock(new Block(BlockType.Stone).SetOpaque().SetCullsSelf().SetUVs(6).SetToughness(3).SetLornName("stone").SetName("Stone"));
            RegisterBlock(new Block(BlockType.Cobblestone).SetOpaque().SetCullsSelf().SetUVs(7).SetToughness(3).SetLornName("cobblestone").SetName("Cobblestone"));
            RegisterBlock(new Block(BlockType.Glass).SetCullsSelf().SetUVs(8).SetToughness(1).SetLornName("glass").SetName("Glass"));
            RegisterBlock(new BlockShortGrass(BlockType.ShortGrass).SetUVs(9).SetToughness(0).SetLornName("shortgrass").SetName("Short Grass"));
        }
    }
}
