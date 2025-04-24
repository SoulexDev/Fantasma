using Fantasma.Framework;
using Fantasma.Generation;
using Fantasma.Globals;
using Fantasma.Graphics;
using Fantasma.Physics;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Threading.Tasks;

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

        public float m_groundSpeed = 0;

        private bool m_jumped;
        private float m_jumpBufferTimer = 0;

        private Vector3 m_forward;
        private Vector2 m_inputVector;

        private Vector3i m_lastChunkPos;
        private Vector3i m_currentChunkPos;

        private bool m_canUpdateVisibility = true;
        public override void Awake()
        {
            base.Awake();

            m_transform.position = Vector3.UnitY * 100;
            Core.m_currentCamera = m_cameraController.m_camera;
            m_camTransform = m_cameraController.m_transform;

            m_physicsBody = new PhysicsBody(m_transform.position, 0.35f, 0.7f, 0, 1.8f, 0.35f, 0.7f);
            m_canUpdateVisibility = true;
        }
        public override void Update()
        {
            PlayerInput();
            if (m_jumped)
            {
                m_jumpBufferTimer += Time.m_deltaTime;
                if(m_jumpBufferTimer > 0.1f)
                {
                    m_jumpBufferTimer = 0;
                    m_jumped = false;
                }
            }

            m_transform.position = m_physicsBody.m_position;
            m_cameraController.m_transform.position = 
                Vector3.Lerp(m_cameraController.m_transform.position, m_transform.position + 0.7f * Vector3.UnitY, Time.m_deltaTime * 25);

            m_currentChunkPos = CoordinateUtils.LocalChunkCoord(m_transform.position);

            UpdateVisibility();
        }
        private void UpdateVisibility()
        {
            m_currentChunkPos = CoordinateUtils.LocalChunkCoord(m_transform.position);

            if (m_currentChunkPos != m_lastChunkPos)
            {
                if (!m_canUpdateVisibility)
                    return;

                m_canUpdateVisibility = false;

                WorldManager.UpdateVisibility(m_currentChunkPos);
                m_lastChunkPos = m_currentChunkPos;

                m_canUpdateVisibility = true;
            }
        }
        public override void FixedUpdate()
        {
            Grounded();
            CalculateMoveVector();
            m_physicsBody.Move();

            float interpSpeed = m_grounded ? 0 : (1 - Time.m_fixedDeltaTime);

            m_physicsBody.m_velocity.X *= interpSpeed;
            m_physicsBody.m_velocity.Z *= interpSpeed;
        }
        private void PlayerInput()
        {
            m_inputVector = new Vector2(Input.Horizontal, Input.Vertical);
            m_forward = Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegToRad * m_cameraController.m_mouseX) * Vector3.UnitZ;

            if (Input.GetKeyDown(Keys.Space))
            {
                m_jumped = true;
                m_jumpBufferTimer = 0;
            }
        }
        private void CalculateMoveVector()
        {
            if (m_inputVector.X != 0 || m_inputVector.Y != 0)
            {
                float speed = m_grounded ? (Input.GetKey(Keys.LeftShift) ? m_sprintSpeed : m_walkSpeed) : MathF.Max(m_groundSpeed, m_walkSpeed);

                m_physicsBody.m_velocity += m_inputVector.X * -m_camTransform.right + m_inputVector.Y * m_forward;

                m_physicsBody.m_velocity.Xz = m_physicsBody.m_velocity.Xz.Normalized() * speed * Time.m_fixedDeltaTime;
            }

            if (m_grounded && m_jumped)
            {
                m_physicsBody.m_velocity.Y = MathF.Sqrt(m_jumpHeight * -2 * Physics.Physics.Gravity) * 0.05f;
                m_jumped = false;
                m_jumpBufferTimer = 0;
            }

            if (m_grounded)
                m_groundSpeed = m_physicsBody.m_velocity.Xz.Length / Time.m_fixedDeltaTime;
        }
        private void Grounded()
        {
            m_grounded = (m_physicsBody.m_collisionFlags & CollisionFlags.CollisionDown) == CollisionFlags.CollisionDown;
        }
    }
}
