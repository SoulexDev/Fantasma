using Fantasma.Globals;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Data
{
    public class BlockData
    {
        public string m_name { get; private set; }
        public string m_lornName { get; private set; }
        public BlockType m_blockType { get; private set; }
        public Vector2i[] m_uvs = new Vector2i[6];
        public int m_toughness { get; private set; }
        public bool m_opaque { get; private set; }
        public bool m_cullsSelf { get; private set; }

        public BlockData(BlockType blockType)
        {
            m_blockType = blockType;
        }
        #region old
        public BlockData(BlockType blockType, int uvIndex, int toughness, bool opaque, bool cullsSelf)
        {
            m_blockType = blockType;
            m_name = blockType.ToString();
            m_uvs[0] = new Vector2i(uvIndex % TextureAtlasUtils.m_atlasSize, uvIndex / TextureAtlasUtils.m_atlasSize);
            m_uvs[1] = new Vector2i(uvIndex % TextureAtlasUtils.m_atlasSize, uvIndex / TextureAtlasUtils.m_atlasSize);
            m_uvs[2] = new Vector2i(uvIndex % TextureAtlasUtils.m_atlasSize, uvIndex / TextureAtlasUtils.m_atlasSize);
            m_uvs[3] = new Vector2i(uvIndex % TextureAtlasUtils.m_atlasSize, uvIndex / TextureAtlasUtils.m_atlasSize);
            m_uvs[4] = new Vector2i(uvIndex % TextureAtlasUtils.m_atlasSize, uvIndex / TextureAtlasUtils.m_atlasSize);
            m_uvs[5] = new Vector2i(uvIndex % TextureAtlasUtils.m_atlasSize, uvIndex / TextureAtlasUtils.m_atlasSize);
            m_toughness = toughness;
            m_opaque = opaque;
            m_cullsSelf = cullsSelf;
        }
        public BlockData(BlockType blockType, int uvif, int uvir, int uvib, int uvil, int uviu, int uvid, int toughness, bool opaque, bool cullsSelf)
        {
            m_blockType = blockType;
            m_name = blockType.ToString();
            m_uvs[0] = new Vector2i(uvif % TextureAtlasUtils.m_atlasSize, uvif / TextureAtlasUtils.m_atlasSize);
            m_uvs[1] = new Vector2i(uvir % TextureAtlasUtils.m_atlasSize, uvir / TextureAtlasUtils.m_atlasSize);
            m_uvs[2] = new Vector2i(uvib % TextureAtlasUtils.m_atlasSize, uvib / TextureAtlasUtils.m_atlasSize);
            m_uvs[3] = new Vector2i(uvil % TextureAtlasUtils.m_atlasSize, uvil / TextureAtlasUtils.m_atlasSize);
            m_uvs[4] = new Vector2i(uviu % TextureAtlasUtils.m_atlasSize, uviu / TextureAtlasUtils.m_atlasSize);
            m_uvs[5] = new Vector2i(uvid % TextureAtlasUtils.m_atlasSize, uvid / TextureAtlasUtils.m_atlasSize);
            m_toughness = toughness;
            m_opaque = opaque;
            m_cullsSelf = cullsSelf;
        }
        public BlockData(BlockType blockType, int uvIndexSides, int uvIndexTop, int uvIndexBottom, int toughness, bool opaque, bool cullsSelf)
        {
            m_blockType = blockType;
            m_name = blockType.ToString();
            m_uvs[0] = new Vector2i(uvIndexSides % TextureAtlasUtils.m_atlasSize, uvIndexSides / TextureAtlasUtils.m_atlasSize);
            m_uvs[1] = new Vector2i(uvIndexSides % TextureAtlasUtils.m_atlasSize, uvIndexSides / TextureAtlasUtils.m_atlasSize);
            m_uvs[2] = new Vector2i(uvIndexSides % TextureAtlasUtils.m_atlasSize, uvIndexSides / TextureAtlasUtils.m_atlasSize);
            m_uvs[3] = new Vector2i(uvIndexSides % TextureAtlasUtils.m_atlasSize, uvIndexSides / TextureAtlasUtils.m_atlasSize);
            m_uvs[4] = new Vector2i(uvIndexTop % TextureAtlasUtils.m_atlasSize, uvIndexTop / TextureAtlasUtils.m_atlasSize);
            m_uvs[5] = new Vector2i(uvIndexBottom % TextureAtlasUtils.m_atlasSize, uvIndexBottom / TextureAtlasUtils.m_atlasSize);
            m_toughness = toughness;
            m_opaque = opaque;
            m_cullsSelf = cullsSelf;
        }
        #endregion
        //public void SetUVs(Direction directions, int uvIndex)
        //{
        //    for (int i = 0; i < 6; i++)
        //    {
        //        Direction dir = (Direction)(1 << i);
        //        if((directions & dir) == dir)
        //        {
        //            m_uvs[i] = UVFromIndex(uvIndex);
        //        }
        //    }
        //}
        private Vector2i UVFromIndex(int uvIndex)
        {
            return new Vector2i(uvIndex % TextureAtlasUtils.m_atlasSize, uvIndex / TextureAtlasUtils.m_atlasSize);
        }
        public BlockData SetUVForward(int uvIndex)
        {
            m_uvs[0] = UVFromIndex(uvIndex);
            return this;
        }
        public BlockData SetUVRight(int uvIndex)
        {
            m_uvs[1] = UVFromIndex(uvIndex);
            return this;
        }
        public BlockData SetUVBack(int uvIndex)
        {
            m_uvs[2] = UVFromIndex(uvIndex);
            return this;
        }
        public BlockData SetUVLeft(int uvIndex)
        {
            m_uvs[3] = UVFromIndex(uvIndex);
            return this;
        }
        public BlockData SetUVUp(int uvIndex)
        {
            m_uvs[4] = UVFromIndex(uvIndex);
            return this;
        }
        public BlockData SetUVDown(int uvIndex)
        {
            m_uvs[5] = UVFromIndex(uvIndex);
            return this;
        }
        public BlockData SetUVSides(int uvIndex)
        {
            Vector2i uv = UVFromIndex(uvIndex);
            m_uvs[0] = uv;
            m_uvs[1] = uv;
            m_uvs[2] = uv;
            m_uvs[3] = uv;
            return this;
        }
        public BlockData SetUVTopBottom(int uvIndex)
        {
            Vector2i uv = UVFromIndex(uvIndex);
            m_uvs[4] = uv;
            m_uvs[5] = uv;
            return this;
        }
        public BlockData SetUVs(int uvIndex)
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
        public BlockData SetToughness(int toughness)
        {
            m_toughness = toughness;
            return this;
        }
        public BlockData SetOpaque()
        {
            m_opaque = true;
            return this;
        }
        public BlockData SetCullsSelf()
        {
            m_cullsSelf = true;
            return this;
        }
        public BlockData SetName(string name)
        {
            m_name = name;
            return this;
        }
        public BlockData SetLornName(string lornName)
        {
            m_lornName = lornName;
            return this;
        }
    }
}
