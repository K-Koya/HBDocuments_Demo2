using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : CharacterMove
{
    /// <summary>MainCameraの位置等の情報</summary>
    Transform _MainCameraTransform = null;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _MainCameraTransform = Camera.main.transform;
        Move = MoveByPlayer;
        Act = ActByPlayer;
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

    /// <summary>標準の移動操作メソッド</summary>
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
    }

    /// <summary>標準の行動操作メソッド</summary>
    void ActByPlayer()
    {
        _JumpFlag = false;
        _DoAction = false;


        //接地時
        if (IsGround)
        {
            switch (_Param.State.Kind)
            {
                //移動状態チェック
                case MotionState.StateKind.Stay:
                case MotionState.StateKind.Walk:
                case MotionState.StateKind.Run:

                    //移動入力がある
                    if(_Param.Direction.sqrMagnitude > 0)
                    {
                        if(_Speed > _Param.LimitSpeedWalk)
                        {
                            _Param.State.Kind = MotionState.StateKind.Run;
                        }
                        else
                        {
                            _Param.State.Kind = MotionState.StateKind.Walk;
                        }
                    }
                    else
                    {
                        _Param.State.Kind = MotionState.StateKind.Stay;
                    }

                    //上記の動作中
                    _Param.State.Process = MotionState.ProcessKind.Playing;

                    break;

                //落下等の着地チェック
                case MotionState.StateKind.FallNoraml:
                case MotionState.StateKind.JumpNoraml:

                    _CommandHolder.Jump.LandingProcess(_Param);
                    break;

                //回避時の移動力チェック
                case MotionState.StateKind.ShiftSlide:

                    _CommandHolder.ShiftSlide.ShiftSlidePostProcess(_Param, _Rb.component, GravityDirection);
                    break;
                case MotionState.StateKind.LongTrip:

                    _CommandHolder.LongTrip.LongTripPostProcess(_Param, _Rb.component, GravityDirection);
                    break;
            }



            //短距離回避処理
            if (_Param.Can.ShiftSlide && InputUtility.GetDodge && InputUtility.GetMoveDown)
            {
                _MovePower = 0f;
                _Param.Direction = CalculateMoveDirection(InputUtility.GetMoveDirection).normalized;

                float fowardCheck = Vector3.Dot(transform.forward, MoveDirection);
                float rightCheck = Vector3.Dot(transform.right, MoveDirection);
                _AnimKind = AnimationKind.ShiftSlideBack;
                if (Mathf.Abs(fowardCheck) > Mathf.Abs(rightCheck))
                {
                    if (fowardCheck > 0f) _AnimKind = AnimationKind.ShiftSlideFoward;
                }
                else
                {
                    if (rightCheck > 0f) _AnimKind = AnimationKind.ShiftSlideRight;
                    else _AnimKind = AnimationKind.ShiftSlideLeft;
                }

                _CommandHolder.ShiftSlide.ShiftSlideOrder(_Param, _Rb.component, GravityDirection, ref _AnimKind);
                _DoAction = true;
            }
            //長距離回避処理
            else if (_Param.Can.LongTrip && InputUtility.GetMoveDirection.sqrMagnitude > 0f && InputUtility.GetDownDodge)
            {
                _MovePower = 0f;
                _AnimKind = AnimationKind.LongTrip;
                _Param.Direction = CalculateMoveDirection(InputUtility.GetMoveDirection).normalized;
                _CommandHolder.LongTrip.LongTripOrder(_Param, _Rb.component, ref _AnimKind);
                _DoAction = true;
            }
            //ジャンプ処理
            else if (_Param.Can.Jump && InputUtility.GetDownJump)
            {
                _AnimKind = AnimationKind.Jump;
                _CommandHolder.Jump.JumpOrder(_Param, _Rb.component, GravityDirection, ref _AnimKind);
                _JumpFlag = true;
            }
            //コンボ攻撃処理
            else if (_Param.Can.ComboNormal && InputUtility.GetDownAttack)
            {
                PlayerParameter pp = _Param as PlayerParameter;
                _CommandHolder.Combo.ComboGroundOrder(_Param, _Rb.component, GravityDirection, pp.ReticlePoint - pp.EyePoint.position, ref _AnimKind);
                _DoAction = true;
            }
        }
        //空中時
        else
        {
            //ジャンプ力減衰
            if (!InputUtility.GetJump && Vector3.Dot(GravityDirection, _Rb.velocity) < 0f)
            {
                _CommandHolder.Jump.JumpOrderOnAir(_Param, _Rb.component, GravityDirection);
            }
            //コンボ攻撃処理
            else if (_Param.Can.ComboNormal && InputUtility.GetDownAttack)
            {
                _Param.State.Kind = MotionState.StateKind.ComboNormal;
                _Param.State.Process = MotionState.ProcessKind.Preparation;
                Debug.Log("空中コンボ!");
                _DoAction = true;
            }
        }
    }
}
