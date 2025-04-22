using Fantasma.Framework;
using Fantasma.Globals;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasma.Scripts
{
    //public class Player_Sprint : State<PlayerController>
    //{
    //    public override void EnterState(PlayerController ctx)
    //    {
            
    //    }

    //    public override void ExitState(PlayerController ctx)
    //    {
            
    //    }

    //    public override void UpdateState(PlayerController ctx)
    //    {
    //        ctx.m_physicsBody.AddForce(ctx.m_moveVector * ctx.m_sprintSpeed);

    //        if (!ctx.m_usingInput)
    //        {
    //            ctx.SwitchState(PlayerStates.Idle);
    //            return;
    //        }
    //        if (!ctx.m_grounded)
    //        {
    //            ctx.SwitchState(PlayerStates.Airborne);
    //            return;
    //        }
    //        if (Input.GetKeyDown(Keys.Space))
    //        {
    //            ctx.SwitchState(PlayerStates.Jump);
    //            return;
    //        }
    //    }
    //}
}
