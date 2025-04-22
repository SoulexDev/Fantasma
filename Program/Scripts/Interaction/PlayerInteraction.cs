using Fantasma.Framework;
using Fantasma.Generation;
using Fantasma.Graphics;
using Fantasma.Physics;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Collections.Generic;

namespace Fantasma.Scripts
{
    public class PlayerInteraction : FantasmaObject
    {
        public Renderable m_spot;

        public override void Awake()
        {
            base.Awake();

            m_spot = BoxModel.CreateBox(new Transform(), ShaderContainer.m_standardShader);
        }
        public override void Update()
        {
            if (Input.GetMouseDown(MouseButton.Left))
            {
                Ray ray = Core.m_currentCamera.GetViewportRay();
                ray.direction *= 8;

                List<AABB> colliders = WorldManager.GetColliders(ray.GetRayBounds());
                List<RaycastHit> rayHits = new List<RaycastHit>();

                foreach (var collider in colliders)
                {
                    RaycastHit hit = collider.GetRayHit(ray);
                    if (hit.hit)
                        rayHits.Add(hit);
                }
                float closest = float.PositiveInfinity;
                RaycastHit finalHit = new RaycastHit();
                for (int i = 0; i < rayHits.Count; i++)
                {
                    if (Vector3.DistanceSquared(rayHits[i].hitPoint, ray.origin) < closest)
                    {
                        closest = Vector3.DistanceSquared(rayHits[i].hitPoint, ray.origin);
                        finalHit = rayHits[i];
                    }
                }

                m_spot.m_transform.position = finalHit.hitPoint - Vector3.One * 0.5f;
            }
            if (Input.GetMouseDown(MouseButton.Right))
            {

            }
        }
    }
}
