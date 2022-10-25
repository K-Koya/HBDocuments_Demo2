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
        Move = MoveByPlayer;
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
    void MoveByPlayer()
    {
        //移動入力
        if (_Param.Can.Move)
        {
            _Param.Direction = CalculateMoveDirection(InputUtility.GetMoveDirection);
        }
        else
        {
            _Param.Direction = Vector3.zero;
        }
            

        //入力があれば移動力の処理
        if (_Param.Direction.sqrMagnitude > 0)
        {
            //移動入力の大きさを取得
            _MoveInputRate = _Param.Direction.magnitude;
            //移動方向を取得
            _Param.Direction *= 1 / _MoveInputRate;
            //移動力指定
            _MovePower = _Param.LimitSpeedRun;
        }
        else
        {
            _MovePower = 0f;
            _MoveInputRate = 0f;
            _Param.Direction = Vector3.zero;
        }

        //重力方向以外で移動量成分があった場合、ブレーキ量を計算する
        bool isMoving = Vector3.SqrMagnitude(VelocityOnPlane) > 0.01f;
        if (isMoving)
        {
            _ForceOfBrake = -VelocityOnPlane.normalized * 0.2f;
        }

        
        _JumpFlag = false;
        _DoCombo = false;
        _DoDodge = false;
        

        //接地時
        if (IsGround)
        {
            switch (_Param.State.Kind)
            {
                //落下等の着地チェック
                case MotionState.StateKind.FallNoraml:
                case MotionState.StateKind.JumpNoraml:

                    _Param.State.Kind = MotionState.StateKind.Stay;
                    _Param.State.Process = MotionState.ProcessKind.Playing;
                    break;

                //回避時の移動力チェック
                case MotionState.StateKind.ShiftSlide:
                case MotionState.StateKind.LongTrip:
                    if (_Param.State.Process == MotionState.ProcessKind.Interval)
                    {
                        _Rb.velocity = Vector3.Project(_Rb.velocity, -GravityDirection);
                        _Param.State.Process = MotionState.ProcessKind.Preparation;
                    }
                    break;
            }



            //短距離回避処理
            if (_Param.Can.ShiftSlide && InputUtility.GetDodge && InputUtility.GetMoveDown)
            {
                _MovePower = 0f;
                _DoDodge = true;
                _Rb.velocity = Vector3.Project(_Rb.velocity, -GravityDirection);
                _Param.Direction = CalculateMoveDirection(InputUtility.GetMoveDirection).normalized;
                _Rb.AddForce(_Param.Direction * 6f, ForceMode.VelocityChange);
                _Param.State.Kind = MotionState.StateKind.ShiftSlide;
                _Param.State.Process = MotionState.ProcessKind.Playing;
            }
            //長距離回避距離
            else if(_Param.Can.LongTrip && InputUtility.GetMoveDirection.sqrMagnitude > 0f && InputUtility.GetDownDodge)
            {
                _MovePower = 0f;
                _DoDodge = true;
                _Param.Direction = CalculateMoveDirection(InputUtility.GetMoveDirection).normalized;
                _Rb.AddForce(_Param.Direction * 8f, ForceMode.VelocityChange);
                _Param.State.Kind = MotionState.StateKind.LongTrip;
                _Param.State.Process = MotionState.ProcessKind.Playing;
            }
            //ジャンプ処理
            else if (_Param.Can.Jump && InputUtility.GetDownJump)
            {
                _Rb.AddForce(-GravityDirection * 7f, ForceMode.VelocityChange);
                _JumpFlag = true;
                _Param.State.Kind = MotionState.StateKind.JumpNoraml;
                _Param.State.Process = MotionState.ProcessKind.Playing;
            }
            //コンボ攻撃処理
            else if (_Param.Can.ComboNormal && InputUtility.GetDownAttack)
            {
                _DoCombo = true;
                _Param.State.Kind = MotionState.StateKind.ComboNormal;
                _Param.State.Process = MotionState.ProcessKind.Preparation;
                Debug.Log("コンボ!");
            }
        }
        //空中時
        else
        {
            //ジャンプ力減衰
            if (!InputUtility.GetJump && Vector3.Angle(-GravityDirection, _Rb.velocity) < 90f)
            {
                _Rb.velocity = Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection);
                _Param.State.Kind = MotionState.StateKind.FallNoraml;
                _Param.State.Process = MotionState.ProcessKind.Playing;
            }
            //コンボ攻撃処理
            else if (_Param.Can.ComboNormal && InputUtility.GetDownAttack)
            {
                _DoCombo = true;
                _Param.State.Kind = MotionState.StateKind.ComboNormal;
                _Param.State.Process = MotionState.ProcessKind.Preparation;
                Debug.Log("空中コンボ!");
            }
        }
    }
}
