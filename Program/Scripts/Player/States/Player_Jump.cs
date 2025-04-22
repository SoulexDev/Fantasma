using Fantasma.Framework;
using Fantasma.Globals;
using System;

namespace Fantasma.Scripts
{
    //public class Player_Jump : State<PlayerController>
    //{
    //    private float m_jumpEnterY;
    //    private float m_jumpTimer;
    //    public override void EnterState(PlayerController ctx)
    //    {
    //        m_jumpTimer = 0;
    //        m_jumpEnterY = ctx.m_transform.position.Y;

    //        ctx.m_gravity = MathF.Sqrt(ctx.m_jumpHeight * -2 * Physics.Physics.Gravity) * Time.m_deltaTime;
    //    }

    //    public override void ExitState(PlayerController ctx)
    //    {
            
    //    }

    //    public override void UpdateState(PlayerController ctx)
    //    {
    //        ctx.m_physicsBody.AddForce(ctx.m_moveVector * ctx.m_walkSpeed);

    //        m_jumpTimer += Time.m_deltaTime;

    //        if (m_jumpTimer > 0.1f && ctx.m_grounded)
    //        {
    //            ctx.SwitchState(PlayerStates.Idle);
    //            return;
    //        }
    //        if(ctx.m_transform.position.Y < m_jumpEnterY)
    //        {
    //            ctx.SwitchState(PlayerStates.Airborne);
    //            return;
    //        }
    //    }
    //}
}
