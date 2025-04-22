using Fantasma.Generation;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Fantasma.Globals
{
    public class CoordinateUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ThreeToIndex(int x, int y, int z, int size)
        {
            return x + y * size + z * size * size;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ThreeToIndex(int x, int y, int z, int sizeX, int sizeY)
        {
            return x + y * sizeX + z * sizeX * sizeY;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ThreeToIndex(Vector3i v, int size)
        {
            return v.X + v.Y * size + v.Z * size * size;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ThreeToIndex(Vector3i v, int sizeX, int sizeY)
        {
            return v.X + v.Y * sizeX + v.Z * sizeX * sizeY;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i IndexToThree(int index, int size)
        {
            return new Vector3i(index % size, index / size % size, index / (size * size));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i IndexToThree(int index, int sizeX, int sizeY)
        {
            return new Vector3i(index % sizeX, index / sizeX % sizeY, index / (sizeX * sizeY));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2i IndexToTwo(int index, int size)
        {
            return new Vector2i(index % size, index / size);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i LocalChunkCoord(Vector3 pos)
        {
            return LocalChunkCoord(new Vector3i().FromVector3(pos));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i LocalChunkCoord(Vector3i pos)
        {
            Vector3i returnValue = pos;
            returnValue.X = (int)MathF.Floor((float)pos.X / WorldParameters.m_chunkSize);
            returnValue.Y = (int)MathF.Floor((float)pos.Y / WorldParameters.m_chunkSize);
            returnValue.Z = (int)MathF.Floor((float)pos.Z / WorldParameters.m_chunkSize);
            return returnValue;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3i LocalVoxelCoord(Vector3 pos)
        {
            return (Vector3i)pos.Floor();
        }
    }
}
