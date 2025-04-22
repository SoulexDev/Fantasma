using Fantasma.Debug;
using Fantasma.Framework;
using Fantasma.Generation;
using Fantasma.Globals;
using Fantasma.Graphics;
using Fantasma.Physics;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;

namespace Fantasma.Scripts
{
    public class PlayerInteraction : FantasmaObject
    {
        public BoundingBox m_spot;

        public override void Awake()
        {
            base.Awake();

            m_spot = new BoundingBox();
            m_spot.Create(new AABB(0, 1));
        }
        public override void Update()
        {
            bool hit = false;
            Ray ray = Core.m_currentCamera.GetViewportRay(1);
            Vector3i mapPos = CoordinateUtils.LocalVoxelCoord(ray.origin);
            Vector3 deltaDist = Vector3.Abs(new Vector3(ray.direction.Length) / ray.direction);
            Vector3i rayStep = ray.direction.Sign();
            Vector3 sideDist = (rayStep * (mapPos - ray.origin) + (rayStep.ToVector3() * 0.5f) + Vector3.One * 0.5f) * deltaDist;
            Bool3 mask;

            for (int i = 0; i < 8; i++)
            {
                BlockType b = WorldManager.m_instance.GetBlock(mapPos);
                mask = Bool3.LessThanEqualTo(sideDist, Vector3.ComponentMin(sideDist.Yzx, sideDist.Zxy));

                sideDist += (Vector3)mask * deltaDist;
                mapPos += (Vector3i)mask * rayStep;

                if ((b & BlockType.NoCollisionFlags) == 0)
                {
                    Console.WriteLine(b.ToString());
                    m_spot.m_transform.position = mapPos;
                    hit = true;
                    break;
                }
            }
            if (hit && Input.GetMouseDown(MouseButton.Left))
            {
                WorldManager.ChangeBlock(mapPos, BlockType.Air);
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
            }
            if (Input.GetMouseDown(MouseButton.Right))
            {

            }
        }
    }
}
