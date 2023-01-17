using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : CharacterMove
{
    /// <summary>MainCamera�̈ʒu���̏��</summary>
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

    /// <summary>�ړ���������͒l����v�Z���ċ��߂�</summary>
    /// <param name="input">���͒l</param>
    /// <returns>�ړ�����</returns>
    Vector3 CalculateMoveDirection(Vector2 input)
    {
        //�O�����ƉE�������擾���A�ړ����͒l�𔽉f
        Vector3 vertical = Vector3.ProjectOnPlane(_MainCameraTransform.forward, -GravityDirection);
        vertical = vertical.normalized * input.y;
        Vector3 horizontal = Vector3.ProjectOnPlane(_MainCameraTransform.right, -GravityDirection);
        horizontal = horizontal.normalized * input.x;

        return vertical + horizontal;
    }

    /// <summary>�W���̈ړ����상�\�b�h</summary>
    void MoveByPlayer()
    {
        //�ړ�����
        if (_Param.Can.Move)
        {
            _Param.Direction = CalculateMoveDirection(InputUtility.GetMoveDirection);
        }
        else
        {
            _Param.Direction = Vector3.zero;
        }

        //���͂�����Έړ��͂̏���
        if (_Param.Direction.sqrMagnitude > 0)
        {
            //�ړ����͂̑傫�����擾
            _MoveInputRate = _Param.Direction.magnitude;
            //�ړ��������擾
            _Param.Direction *= 1 / _MoveInputRate;
            //�ړ��͎w��
            _MovePower = _Param.LimitSpeedRun;
        }
        else
        {
            _MovePower = 0f;
            _MoveInputRate = 0f;
            _Param.Direction = Vector3.zero;
        }

        //�d�͕����ȊO�ňړ��ʐ������������ꍇ�A�u���[�L�ʂ��v�Z����
        bool isMoving = Vector3.SqrMagnitude(VelocityOnPlane) > 0.01f;
        if (isMoving)
        {
            _ForceOfBrake = -VelocityOnPlane.normalized * 0.2f;
        }
    }

    /// <summary>�W���̍s�����상�\�b�h</summary>
    void ActByPlayer()
    {
        _JumpFlag = false;
        _DoAction = false;


        //�ڒn��
        if (IsGround)
        {
            switch (_Param.State.Kind)
            {
                //�ړ���ԃ`�F�b�N
                case MotionState.StateKind.Stay:
                case MotionState.StateKind.Walk:
                case MotionState.StateKind.Run:

                    //�ړ����͂�����
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

                    //��L�̓��쒆
                    _Param.State.Process = MotionState.ProcessKind.Playing;

                    break;

                //�������̒��n�`�F�b�N
                case MotionState.StateKind.FallNoraml:
                case MotionState.StateKind.JumpNoraml:

                    _CommandHolder.Jump.LandingProcess(_Param);
                    break;

                //������̈ړ��̓`�F�b�N
                case MotionState.StateKind.ShiftSlide:

                    _CommandHolder.ShiftSlide.ShiftSlidePostProcess(_Param, _Rb.component, GravityDirection);
                    break;
                case MotionState.StateKind.LongTrip:

                    _CommandHolder.LongTrip.LongTripPostProcess(_Param, _Rb.component, GravityDirection);
                    break;
            }



            //�Z�����������
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
            //�������������
            else if (_Param.Can.LongTrip && InputUtility.GetMoveDirection.sqrMagnitude > 0f && InputUtility.GetDownDodge)
            {
                _MovePower = 0f;
                _AnimKind = AnimationKind.LongTrip;
                _Param.Direction = CalculateMoveDirection(InputUtility.GetMoveDirection).normalized;
                _CommandHolder.LongTrip.LongTripOrder(_Param, _Rb.component, ref _AnimKind);
                _DoAction = true;
            }
            //�W�����v����
            else if (_Param.Can.Jump && InputUtility.GetDownJump)
            {
                _AnimKind = AnimationKind.Jump;
                _CommandHolder.Jump.JumpOrder(_Param, _Rb.component, GravityDirection, ref _AnimKind);
                _JumpFlag = true;
            }
            //�R���{�U������
            else if (_Param.Can.ComboNormal && InputUtility.GetDownAttack)
            {
                PlayerParameter pp = _Param as PlayerParameter;
                _CommandHolder.Combo.ComboGroundOrder(_Param, _Rb.component, GravityDirection, pp.ReticlePoint - pp.EyePoint.position, ref _AnimKind);
                _DoAction = true;
            }
        }
        //�󒆎�
        else
        {
            //�W�����v�͌���
            if (!InputUtility.GetJump && Vector3.Dot(GravityDirection, _Rb.velocity) < 0f)
            {
                _CommandHolder.Jump.JumpOrderOnAir(_Param, _Rb.component, GravityDirection);
            }
            //�R���{�U������
            else if (_Param.Can.ComboNormal && InputUtility.GetDownAttack)
            {
                _Param.State.Kind = MotionState.StateKind.ComboNormal;
                _Param.State.Process = MotionState.ProcessKind.Preparation;
                Debug.Log("�󒆃R���{!");
                _DoAction = true;
            }
        }
    }
}
