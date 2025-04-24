using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Globals
{
    public enum VertexAttribute 
    {
        Position = 1, //3 values, 12 bytes
        Normal = 2, //3 values, 12 bytes
        UV = 4, //2 values, 8 bytes
        Color = 8 //3 values, 12 bytes
    }
    public enum Direction
    {
        All = Forward | Right | Back | Left | Up | Down,
        Sides = Forward | Right | Back | Left,
        TopBottoms = Up | Down,
        Forward = 1, Right = 2, Back = 4, Left = 8, Up = 16, Down = 32
    }
    public enum BlockType 
    {
        NoCollisionFlags = Nothing | Air | Water | Lava | ShortGrass,
        TransparentFlags = Nothing | Air | Glass | Water | ShortGrass,
        CantBreakFlags = Nothing | Air | Water | Lava,
        ReplacableFlags = Nothing | Air | Water | Lava | ShortGrass,
        BillboardFlags = ShortGrass,

        Nothing = 1, Air = 2, Dirt = 4, Grass = 8, Stone = 16, Sand = 32, Wood = 64, Glass = 128, Water = 256, Lava = 512, ShortGrass = 1024, Planks = 2048, Cobblestone = 4096,
        Leaves = 8192,
    }
    public enum CollisionFlags
    {
        CollisionForward = 1, CollisionRight = 2, CollisionBack = 4, CollisionLeft = 8, CollisionUp = 16, CollisionDown = 32, 
        NoCollisionX = ~(CollisionLeft | CollisionRight), 
        NoCollisionY = ~(CollisionUp | CollisionDown), 
        NoCollisionZ = ~(CollisionForward | CollisionBack)
    }
    public enum BlockEventType { Place, Break, Power, UnPower, Update }
    public enum RenderableType { Opaque, Transparent }
    public enum DimensionType { Ideonia, Void }

    public enum PlayerStates { Idle, Walk, Sprint, Jump, Airborne }
}
