using Fantasma.Data;
using Fantasma.Globals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Data
{
    public class BlockDataManager
    {
        public static Dictionary<string, int[]> m_textureIndicies = new Dictionary<string, int[]>()
        {
            { "dirt", new int[] { 0 } },
        };
        public static Dictionary<BlockType, BlockData> m_blockData = new Dictionary<BlockType, BlockData>()
        {
            { BlockType.Nothing, new BlockData(BlockType.Nothing, 0, 0, true, true) },
            { BlockType.Air, new BlockData(BlockType.Air, 0, 0, false, true) },
            { BlockType.Dirt, new BlockData(BlockType.Dirt, 0, 1, true, true) },
            { BlockType.Grass, new BlockData(BlockType.Grass, 1, 2, 0, 1, true, true) },
            { BlockType.Stone, new BlockData(BlockType.Stone, 6, 3, true, true) },
            { BlockType.Sand, new BlockData(BlockType.Sand, 12, 1, true, true) },
            { BlockType.Wood, new BlockData(BlockType.Wood, 3, 4, 4, 2, true, true) },
            { BlockType.Glass, new BlockData(BlockType.Glass, 8, 1, false, true) },
            { BlockType.Water, new BlockData(BlockType.Water, 12, 0, false, true) },
            { BlockType.Lava, new BlockData(BlockType.Lava, 12, 0, true, true) },
            { BlockType.ShortGrass, new BlockData(BlockType.ShortGrass, 9, 0, false, false) }
        };
        public static Dictionary<BlockType, BlockData> m_blockDatas = new Dictionary<BlockType, BlockData>()
        {
            { BlockType.Nothing, new BlockData(BlockType.Nothing).SetOpaque() },
            { BlockType.Air, new BlockData(BlockType.Air) },
            { BlockType.Dirt, new BlockData(BlockType.Dirt).SetName("Dirt").SetLornName("dirt").SetOpaque().SetCullsSelf().SetUVs(0).SetToughness(1) },
            { BlockType.Grass, new BlockData(BlockType.Grass).SetName("Grass").SetLornName("grass").SetOpaque().SetCullsSelf().SetUVUp(2).SetUVSides(1).SetUVTopBottom(0).SetToughness(1) },
            { BlockType.Stone, new BlockData(BlockType.Stone, 8, 3, true, true) },
            { BlockType.Sand, new BlockData(BlockType.Sand, 12, 1, true, true) },
            { BlockType.Wood, new BlockData(BlockType.Wood, 3, 4, 4, 2, true, true) },
            { BlockType.Glass, new BlockData(BlockType.Glass, 10, 1, false, true) },
            { BlockType.Water, new BlockData(BlockType.Water, 12, 0, false, true) },
            { BlockType.Lava, new BlockData(BlockType.Lava, 12, 0, true, true) },
        };
    }
}
