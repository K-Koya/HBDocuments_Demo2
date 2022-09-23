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
        Attack = AttackByNormalCombo;
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
        if (!_Param.Can._Move) return Vector3.zero;

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
            _MoveInputRate = _Param.Direction.magnitude;
            //移動方向を取得
            _Param.Direction *= 1 / _MoveInputRate;
            //移動力指定
            MovePower = _Param.LimitSpeedRun;
        }
        else
        {
            MovePower = 0f;
            _MoveInputRate = 0f;
            _Param.Direction = Vector3.zero;
        }

        //重力方向以外で移動量成分があった場合、ブレーキ量を計算する
        bool isMoving = Vector3.SqrMagnitude(VelocityOnPlane) > 0.01f;
        if (isMoving)
        {
            _ForceOfBrake = -VelocityOnPlane.normalized * 0.2f;
        }

        //ジャンプ力減衰
        if (!InputUtility.GetJump && !IsGround && Vector3.Angle(-GravityDirection, _Rb.velocity) < 90f)
        {
            _Rb.velocity = Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection);
        }

        //接地時にジャンプ処理
        _JumpFlag = false;
        if (_Param.Can._Jump && IsGround && InputUtility.GetDownJump)
        {
            _Rb.AddForce(-GravityDirection * 7f, ForceMode.VelocityChange);
            _JumpFlag = true;
        }
    }

    /// <summary>通常コンボによる攻撃メソッド</summary>
    void AttackByNormalCombo()
    {
        _DoCombo = false;

        if (_Param.Can._ComboNormal)
        {
            switch (_Param.State.State)
            {
                case MotionState.StateKind.Stay:
                case MotionState.StateKind.Walk:
                case MotionState.StateKind.Run:
                    if (_IsAttackEnd && InputUtility.GetDownAttack)
                    {
                        _DoCombo = true;
                        _IsAttackEnd = false;
                        _Param.AttackAreas.Add(new AttackAreaCapsule(1f, transform.position, transform.position + transform.forward * _Param.AttackRangeMiddle));
                        _Param.State.State = MotionState.StateKind.ComboNormal;
                        _Param.State.Process = MotionState.ProcessKind.Preparation;
                        _Param.Can._Move = false;
                    }

                    break;
                case MotionState.StateKind.ComboNormal:

                    switch (_Param.State.Process)
                    {
                        case MotionState.ProcessKind.Interval:

                            if (_IsAttackEnd && InputUtility.GetDownAttack)
                            {
                                _DoCombo = true;
                                _IsAttackEnd = false;
                                _Param.AttackAreas.Add(new AttackAreaCapsule(1f, transform.position, transform.position + transform.forward * _Param.AttackRangeMiddle));
                                _Param.State.Process = MotionState.ProcessKind.Preparation;
                            }
                            break;

                        case MotionState.ProcessKind.NotPlaying:

                            _Param.State.State = MotionState.StateKind.Stay;
                            _Param.State.Process = MotionState.ProcessKind.Playing;
                            _Param.Can._Move = true;
                            break;

                        default: break;
                    }

                    break;
            }
        }
    }
}
