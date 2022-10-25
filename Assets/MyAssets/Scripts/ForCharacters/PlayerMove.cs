using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : CharacterMove
{
    /// <summary>MainCamera�̈ʒu���̏��</summary>
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

    /// <summary>�d�͕����̒���ω������Ȃ��ړ����\�b�h</summary>
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

        
        _JumpFlag = false;
        _DoCombo = false;
        _DoDodge = false;
        

        //�ڒn��
        if (IsGround)
        {
            switch (_Param.State.Kind)
            {
                //�������̒��n�`�F�b�N
                case MotionState.StateKind.FallNoraml:
                case MotionState.StateKind.JumpNoraml:

                    _Param.State.Kind = MotionState.StateKind.Stay;
                    _Param.State.Process = MotionState.ProcessKind.Playing;
                    break;

                //������̈ړ��̓`�F�b�N
                case MotionState.StateKind.ShiftSlide:
                case MotionState.StateKind.LongTrip:
                    if (_Param.State.Process == MotionState.ProcessKind.Interval)
                    {
                        _Rb.velocity = Vector3.Project(_Rb.velocity, -GravityDirection);
                        _Param.State.Process = MotionState.ProcessKind.Preparation;
                    }
                    break;
            }



            //�Z�����������
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
            //�������������
            else if(_Param.Can.LongTrip && InputUtility.GetMoveDirection.sqrMagnitude > 0f && InputUtility.GetDownDodge)
            {
                _MovePower = 0f;
                _DoDodge = true;
                _Param.Direction = CalculateMoveDirection(InputUtility.GetMoveDirection).normalized;
                _Rb.AddForce(_Param.Direction * 8f, ForceMode.VelocityChange);
                _Param.State.Kind = MotionState.StateKind.LongTrip;
                _Param.State.Process = MotionState.ProcessKind.Playing;
            }
            //�W�����v����
            else if (_Param.Can.Jump && InputUtility.GetDownJump)
            {
                _Rb.AddForce(-GravityDirection * 7f, ForceMode.VelocityChange);
                _JumpFlag = true;
                _Param.State.Kind = MotionState.StateKind.JumpNoraml;
                _Param.State.Process = MotionState.ProcessKind.Playing;
            }
            //�R���{�U������
            else if (_Param.Can.ComboNormal && InputUtility.GetDownAttack)
            {
                _DoCombo = true;
                _Param.State.Kind = MotionState.StateKind.ComboNormal;
                _Param.State.Process = MotionState.ProcessKind.Preparation;
                Debug.Log("�R���{!");
            }
        }
        //�󒆎�
        else
        {
            //�W�����v�͌���
            if (!InputUtility.GetJump && Vector3.Angle(-GravityDirection, _Rb.velocity) < 90f)
            {
                _Rb.velocity = Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection);
                _Param.State.Kind = MotionState.StateKind.FallNoraml;
                _Param.State.Process = MotionState.ProcessKind.Playing;
            }
            //�R���{�U������
            else if (_Param.Can.ComboNormal && InputUtility.GetDownAttack)
            {
                _DoCombo = true;
                _Param.State.Kind = MotionState.StateKind.ComboNormal;
                _Param.State.Process = MotionState.ProcessKind.Preparation;
                Debug.Log("�󒆃R���{!");
            }
        }
    }
}
