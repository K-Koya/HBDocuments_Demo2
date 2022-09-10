using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : CharacterMove
{
    /// <summary>MainCameraの位置等の情報</summary>
    Transform _MainCameraTransform = default;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _MainCameraTransform = Camera.main.transform;
        Move = MoveOnPlane;
    }

    // Update is called once per frame
    protected override void Update()
    {

        base.Update();
    }

    /// <summary>移動方向を入力値から計算して求める</summary>
    /// <param name="input">入力値</param>
    /// <returns>移動方向</returns>
    Vector3 CalculateMoveDirection(Vector2 input)
    {
        //前方向と右方向を取得し、移動入力値を反映
        Vector3 vertical = Vector3.ProjectOnPlane(_MainCameraTransform.forward, -GravityDirection);
        vertical = vertical.normalized * input.y;
        Vector3 horizontal = Vector3.ProjectOnPlane(_MainCameraTransform.right, -GravityDirection);
        horizontal = horizontal.normalized * input.x;

        return vertical + horizontal;
    }

    /// <summary>重力方向の逐一変化をしない移動メソッド</summary>
    void MoveOnPlane()
    {
        //移動入力
        _Param.Direction = CalculateMoveDirection(InputUtility.GetMoveDirection);

        //入力があれば移動力の処理
        if (_Param.Direction.sqrMagnitude > 0)
        {
            //移動入力の大きさを取得
            _CurrentMovePower = _Param.Direction.magnitude;
            //移動方向を取得
            _Param.Direction *= 1 / _CurrentMovePower;
        }
        else
        {
            _CurrentMovePower = 0f;
            _Param.Direction = Vector3.zero;
        }

        //重力方向以外で移動量成分があった場合、ブレーキ量を計算する
        bool isMoving = Vector3.SqrMagnitude(VelocityOnPlane) > 0.01f;
        if (isMoving)
        {
            _ForceOfBrake = -VelocityOnPlane.normalized * 0.2f;
        }


        //ジャンプ力減衰
        if (InputUtility.GetJump && !IsGround && Vector3.Angle(-GravityDirection, _Rb.velocity) < 90f)
        {
            _Rb.velocity = Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection);
        }

        //着地時にジャンプ処理
        _JumpFlag = false;
        if (IsGround && InputUtility.GetDownJump)
        {
            _Rb.AddForce(-GravityDirection * 7f, ForceMode.VelocityChange);
            _JumpFlag = true;
        }
    }
}
