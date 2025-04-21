using Fantasma.Collision;
using Fantasma.Debug;
using Fantasma.Framework;
using Fantasma.Generation;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Player
{
    public class Camera : FantasmaObject
    {
        public Matrix4 m_viewMatrix;
        public Matrix4 m_projectionMatrix;

        public float m_fov = 85;

        public float m_near = 0.1f;
        public float m_far = 1000f;

        private float m_mouseX, m_mouseY;

        private Vector3 m_moveVector = Vector3.Zero;
        private Vector3 m_forward;
        private float gravity;
        private bool m_grounded;
        private bool jumped;
        private float jumpTimer;

        private AABB m_collider;
        private BoundingBox m_boundingBox;

        public override void Awake()
        {
            base.Awake();

            m_collider = new AABB(0.35f, 0.7f, 0, 1.8f, 0.35f, 0.7f);
            m_collider.Move(0, 100, 0);
            m_boundingBox = new BoundingBox();
            m_boundingBox.Create(m_collider);
        }
        public override void Update()
        {
            DoRotation();

            if (jumped)
            {
                jumpTimer += Time.m_deltaTime;
                if (jumpTimer > 0.2f)
                {
                    jumped = false;
                    jumpTimer = 0;
                }
            }

            Move(m_moveVector);
            CalculateMoveVector();
            m_moveVector.X *= 0.5f;
            m_moveVector.Z *= 0.5f;

            SetMatrices();
        }
        private void Move(Vector3 moveVector)
        {
            Vector3 originalVector = moveVector;

            List<AABB> collisionChecks = WorldManager.GetColliders(m_collider.Expand(moveVector.X, moveVector.Y, moveVector.Z).Grow(1, 1, 1));

            foreach (AABB collider in collisionChecks)
            {
                moveVector.Y = m_collider.GetClipY(collider, moveVector.Y);
            }

            m_collider.Move(0, moveVector.Y, 0);

            m_grounded = moveVector.Y != originalVector.Y && originalVector.Y <= 0;
            //Console.WriteLine(m_grounded);

            foreach (AABB collider in collisionChecks)
            {
                moveVector.X = m_collider.GetClipX(collider, moveVector.X);
            }

            m_collider.Move(moveVector.X, 0, 0);

            foreach (AABB collider in collisionChecks)
            {
                moveVector.Z = m_collider.GetClipZ(collider, moveVector.Z);
            }

            m_collider.Move(0, 0, moveVector.Z);

            if (moveVector.X != originalVector.X)
                m_moveVector.X = 0;

            if (moveVector.Y != originalVector.Y)
                m_moveVector.Y = 0;

            if (moveVector.Z != originalVector.Z)
                m_moveVector.Z = 0;

            m_transform.position = m_collider.GetCenter() + Vector3.UnitY * 0.7f;
            m_boundingBox.m_transform.position = m_transform.position;
        }
        private void CalculateMoveVector()
        {
            Vector2 inputVector = new Vector2(Input.Horizontal, Input.Vertical);

            if (Input.GetKeyDown(Keys.Space))
            {
                jumped = true;
                gravity += 10 * Time.m_deltaTime;
            }

            if (m_grounded && !jumped)
                gravity = 0;
            else
                gravity -= 9.81f * Time.m_deltaTime * Time.m_deltaTime;

            Console.WriteLine(gravity);

            if (inputVector.X != 0 || inputVector.Y != 0)
            {
                m_moveVector += inputVector.X * -m_transform.m_right + inputVector.Y * m_forward;
                m_moveVector = m_moveVector.Normalized() * 5 * (Input.GetKey(Keys.LeftShift) ? 3 : 1) * Time.m_deltaTime;

                m_moveVector.Y = gravity;
            }
        }
        private void DoRotation()
        {
            if (Input.GetMouse(MouseButton.Left))
            {
                m_mouseX -= Input.MouseX * 0.5f;
                m_mouseY += Input.MouseY * 0.5f;
            }

            m_mouseY = MathHelper.Clamp(m_mouseY, -90, 90);

            m_forward = Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegToRad * m_mouseX) * Vector3.UnitZ;

            m_transform.rotation = Quaternion.FromEulerAngles(0, MathHelper.DegToRad * m_mouseX, 0) *
                Quaternion.FromEulerAngles(MathHelper.DegToRad * m_mouseY, 0, 0);
        }
        public void SetMatrices()
        {
            m_viewMatrix = Matrix4.LookAt(m_transform.position, m_transform.position + m_transform.m_forward, m_transform.m_up);
            m_projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegToRad * m_fov, Core.m_aspect, m_near, m_far);
        }
    }
}
