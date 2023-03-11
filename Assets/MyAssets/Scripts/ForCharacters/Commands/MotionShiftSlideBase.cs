using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionShiftSlideBase
{
    /// <summary>短距離回避（シフトスライド）処理</summary>
    /// <param name="param">該当キャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="gravityDirection">重力方向</param>
    public virtual void ShiftSlideOrder(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, ref AnimationKind animKind)
    {
        rb.velocity = Vector3.Project(rb.velocity, gravityDirection);
        rb.AddForce(param.MoveDirection * 6f, ForceMode.VelocityChange);
        param.State.Kind = MotionState.StateKind.ShiftSlide;
        param.State.Process = MotionState.ProcessKind.Playing;
    }

    /// <summary>短距離回避（シフトスライド）完了処理</summary>
    /// <param name="param">該当キャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="gravityDirection">重力方向</param>
    public virtual void ShiftSlidePostProcess(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection)
    {
        if (param.State.Process == MotionState.ProcessKind.Interval)
        {
            rb.velocity = Vector3.Project(rb.velocity, -gravityDirection);
            param.State.Process = MotionState.ProcessKind.Preparation;
        }
    }
}
