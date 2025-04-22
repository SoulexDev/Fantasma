using Fantasma.Framework;
using Fantasma.Globals;
using Fantasma.Graphics;
using Fantasma.Physics;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace Fantasma.Scripts
{
    public class PlayerController : FantasmaObject
    {
        private CameraController m_cameraController = new CameraController();
        private Transform m_camTransform;
        public PhysicsBody m_physicsBody;

        public bool m_moving;
        public bool m_grounded;

        public float m_walkSpeed = 3;
        public float m_sprintSpeed = 6;
        public float m_jumpHeight = 1.2f;

        public float m_gravity;
        public float m_groundSpeed = 0;

        private bool m_jumped;
        private float m_jumpBufferTimer = 0;

        private float mininiasdniasnd;

        private Vector3 m_forward;

        public override void Awake()
        {
            base.Awake();

            m_transform.position = Vector3.UnitY * 100;
            Core.m_currentCamera = m_cameraController.m_camera;
            m_camTransform = m_cameraController.m_transform;

            m_physicsBody = new PhysicsBody(m_transform.position, 0.35f, 0.7f, 0, 1.8f, 0.35f, 0.7f);
        }
        public override void Update()
        {
            if (m_jumped)
            {
                m_jumpBufferTimer += Time.m_deltaTime;
                if(m_jumpBufferTimer > 0.1f)
                {
                    m_jumpBufferTimer = 0;
                    m_jumped = false;
                }
            }
            Gravity();
            CalculateMoveVector();

            m_physicsBody.Move();

            m_physicsBody.m_velocity.X *= 0.99f;
            m_physicsBody.m_velocity.Z *= 0.99f;

            m_transform.position = m_physicsBody.m_position;
            m_cameraController.m_transform.position = m_transform.position + 0.7f * Vector3.UnitY;
        }
        private void CalculateMoveVector()
        {
            Vector2 inputVector = new Vector2(Input.Horizontal, Input.Vertical);
            Vector2 moveXZ = Vector2.Zero;

            m_forward = Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegToRad * m_cameraController.m_mouseX) * Vector3.UnitZ;

            if (inputVector.X != 0 || inputVector.Y != 0)
            {
                float speed = m_grounded ? (Input.GetKey(Keys.LeftShift) ? m_sprintSpeed : m_walkSpeed) : MathF.Max(m_groundSpeed, m_walkSpeed);

                m_physicsBody.m_velocity += inputVector.X * -m_camTransform.right + inputVector.Y * m_forward;

                moveXZ = m_physicsBody.m_velocity.Xz.Normalized() * speed * Time.m_deltaTime;

                m_physicsBody.m_velocity.X = moveXZ.X;
                m_physicsBody.m_velocity.Z = moveXZ.Y;
            }

            if (m_grounded && Input.GetKeyDown(Keys.Space))
            {
                m_physicsBody.m_velocity.Y = MathF.Sqrt(m_jumpHeight * -2 * Physics.Physics.Gravity) * 0.001f;
                m_jumped = false;
                m_jumpBufferTimer = 0;
            }

            if (m_grounded)
                m_groundSpeed = moveXZ.Length / Time.m_deltaTime;
        }
        private void Gravity()
        {
            m_grounded = (m_physicsBody.m_collisionFlags & CollisionFlags.CollisionDown) == CollisionFlags.CollisionDown;

            m_physicsBody.m_velocity.Y -= 0.00001f;

            float last = mininiasdniasnd;
            mininiasdniasnd = MathF.Min(mininiasdniasnd, m_physicsBody.m_velocity.Y);

            if (last != mininiasdniasnd)
                Console.WriteLine(mininiasdniasnd);
        }
    }
}
