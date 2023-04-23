using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionJumpBase
{
    /// <summary>ジャンプ処理</summary>
    /// <param name="param">該当キャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="gravityDirection">重力方向</param>
    /// <param name="animKind">要求するアニメーションの種類</param>
    public virtual void JumpOrder(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, ref AnimationKind animKind)
    {
        rb.AddForce(-gravityDirection * 7f, ForceMode.VelocityChange);
        param.State.Kind = MotionState.StateKind.JumpNoraml;
        param.State.Process = MotionState.ProcessKind.Playing;
    }



    /// <summary>ジャンプ後の空中処理</summary>
    /// <param name="param">該当キャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="gravityDirection">重力方向</param>
    public virtual void JumpOrderOnAir(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection)
    {
        rb.velocity = Vector3.ProjectOnPlane(rb.velocity, -gravityDirection);
        param.State.Kind = MotionState.StateKind.FallNoraml;
        param.State.Process = MotionState.ProcessKind.Playing;
    }

    /// <summary>着地時の処理</summary>
    /// <param name="param">該当キャラクターのパラメータ</param>
    public virtual void LandingProcess(CharacterParameter param)
    {
        param.State.Kind = MotionState.StateKind.Stay;
        param.State.Process = MotionState.ProcessKind.Playing;
    }
}
