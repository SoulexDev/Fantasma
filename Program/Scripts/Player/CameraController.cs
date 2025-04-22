using Fantasma.Framework;
using OpenTK.Mathematics;

namespace Fantasma.Scripts
{
    public class CameraController : FantasmaObject
    {
        public Camera m_camera = new Camera();
        public float m_mouseX, m_mouseY;

        public override void Awake()
        {
            base.Awake();
            Cursor.Locked = true;
            m_camera.m_transform.SetParent(m_transform);
        }
        public override void Update()
        {
            m_mouseX -= Input.MouseX * 0.5f;
            m_mouseY += Input.MouseY * 0.5f;

            m_mouseY = MathHelper.Clamp(m_mouseY, -90, 90);

            m_transform.localRotation = Quaternion.FromEulerAngles(0, MathHelper.DegToRad * m_mouseX, 0) *
                Quaternion.FromEulerAngles(MathHelper.DegToRad * m_mouseY, 0, 0);
        }
    }
}
