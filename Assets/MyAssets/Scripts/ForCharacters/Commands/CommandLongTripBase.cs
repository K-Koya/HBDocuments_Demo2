using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommandLongTripBase : CommandBase
{
    public CommandLongTripBase()
    {
        _Name = "ロングトリップ";
        _Explain = "長距離の回避手段。地に足つけて走っているときに回避ボタンを押すと、走っていた方向に滑るように高速移動をする。";
    }

    /// <summary>長距離回避（ロングトリップ）処理</summary>
    /// <param name="param">該当キャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    public virtual void LongTripOrder(CharacterParameter param, Rigidbody rb)
    {
        rb.AddForce(param.Direction * 8f, ForceMode.VelocityChange);
        param.State.Kind = MotionState.StateKind.LongTrip;
        param.State.Process = MotionState.ProcessKind.Playing;
    }


    /// <summary>長距離回避（ロングトリップ）完了処理</summary>
    /// <param name="param">該当キャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="gravityDirection">重力方向</param>
    public virtual void LongTripPostProcess(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection)
    {
        if (param.State.Process == MotionState.ProcessKind.Interval)
        {
            rb.velocity = Vector3.Project(rb.velocity, -gravityDirection);
            param.State.Process = MotionState.ProcessKind.Preparation;
        }
    }
}
