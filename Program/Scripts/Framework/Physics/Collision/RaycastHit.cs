using Fantasma.Globals;
using OpenTK.Mathematics;

namespace Fantasma.Physics
{
    public struct RaycastHit
    {
        public bool hit;
        public Vector3 normal;
        public Vector3i voxelHitPoint;
        public Vector3 hitPoint;
        public float distance;
        public BlockType blockType;

        public RaycastHit(Vector3 normal, Vector3i voxelHitPoint, Vector3 hitPoint, float distance, BlockType blockType)
        {
            this.normal = normal;
            this.voxelHitPoint = voxelHitPoint;
            this.hitPoint = hitPoint;
            this.distance = distance;
            this.blockType = blockType;
            hit = true;
        }
    }
}
