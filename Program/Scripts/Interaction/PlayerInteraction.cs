using Fantasma.Debug;
using Fantasma.Framework;
using Fantasma.Generation;
using Fantasma.Globals;
using Fantasma.Physics;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace Fantasma.Scripts
{
    public class PlayerInteraction : FantasmaObject
    {
        public BoundingBox m_hoverBox;

        public override void Awake()
        {
            base.Awake();

            m_hoverBox = new BoundingBox();
            m_hoverBox.Create(new AABB(-0.005f, 1.005f));
        }
        public override void Update()
        {
            RaycastHit rayHit = WorldManager.GetRayHit(Core.m_currentCamera.GetViewportRay(1));

            m_hoverBox.m_renderable.transform.position = rayHit.voxelHitPoint;

            //Console.WriteLine($"hover pos: {m_hoverBox.m_renderable.transform.position}, ray pos:{rayHit.voxelHitPoint}");

            if (rayHit.hit)
            {
                if (Input.GetMouseDown(MouseButton.Left))
                {
                    WorldManager.ChangeBlock(rayHit.voxelHitPoint, BlockType.Air);

                    #region for later
                    //Console.WriteLine();
                    //Ray ray = Core.m_currentCamera.GetViewportRay();
                    //ray.direction *= 8;

                    //List<AABB> colliders = WorldManager.GetColliders(ray.GetRayBounds());
                    //List<RaycastHit> rayHits = new List<RaycastHit>();

                    //foreach (var collider in colliders)
                    //{
                    //    RaycastHit hit = collider.GetRayHit(ray);
                    //    if (hit.hit)
                    //        rayHits.Add(hit);
                    //}
                    //float closest = float.PositiveInfinity;
                    //RaycastHit finalHit = new RaycastHit();
                    //for (int i = 0; i < rayHits.Count; i++)
                    //{
                    //    if (Vector3.DistanceSquared(rayHits[i].hitPoint, ray.origin) < closest)
                    //    {
                    //        closest = Vector3.DistanceSquared(rayHits[i].hitPoint, ray.origin);
                    //        finalHit = rayHits[i];
                    //    }
                    //}

                    //m_spot.m_transform.position = finalHit.hitPoint - Vector3.One * 0.5f;
                    #endregion
                }
                if (Input.GetMouseDown(MouseButton.Right))
                {
                    if ((rayHit.blockType & BlockType.ReplacableFlags) == rayHit.blockType)
                        WorldManager.ChangeBlock(rayHit.voxelHitPoint, BlockType.Wood);
                    else
                        WorldManager.ChangeBlock(rayHit.voxelHitPoint + (Vector3i)rayHit.normal, BlockType.Stone);
                }
            }
        }
    }
}
