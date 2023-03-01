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
        Movement = MoveByPlayer;
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
            _Param.MoveDirection = CalculateMoveDirection(InputUtility.GetMoveDirection);
            _Param.CharacterDirection = _Param.MoveDirection;
        }
        else
        {
            _Param.MoveDirection = Vector3.zero;
        }

        //���͂�����Έړ��͂̏���
        if (_Param.MoveDirection.sqrMagnitude > 0)
        {
            //�ړ����͂̑傫�����擾
            _MoveInputRate = _Param.MoveDirection.magnitude;
            //�ړ��������擾
            _Param.MoveDirection *= 1 / _MoveInputRate;
            //�ړ��͎w��
            _MovePower = _Param.Sub.LimitSpeedRun;
        }
        else
        {
            _MovePower = 0f;
            _MoveInputRate = 0f;
            _Param.MoveDirection = Vector3.zero;
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
                    if (_Param.MoveDirection.sqrMagnitude > 0)
                    {
                        if (_Speed > _Param.Sub.LimitSpeedWalk)
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

                //��_���[�W�`�F�b�N
                case MotionState.StateKind.Hurt:


                    break;
            }


            //�Z�����������
            if (_Param.Can.ShiftSlide && InputUtility.GetDodge && InputUtility.GetMoveDown)
            {
                _MovePower = 0f;
                _Param.MoveDirection = CalculateMoveDirection(InputUtility.GetMoveDirection).normalized;

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
                _Param.MoveDirection = CalculateMoveDirection(InputUtility.GetMoveDirection).normalized;
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
                _CommandHolder.GetComboForRun().DoRun(_Param, _Rb.component, GravityDirection, pp.ReticlePoint - pp.EyePoint.position, ref _AnimKind);
                _DoAction = true;
            }
            //�R�}���h���s����
            else if (_Param.Can.Command)
            {
                int index = -1;
                if(InputUtility.GetDownSkillCommand1)
                {
                    index = 0;
                }
                else if(InputUtility.GetDownSkillCommand2) 
                {
                    index = 1;
                }
                else if(InputUtility.GetDownSkillCommand3)
                {
                    index = 2;
                }
                else if(InputUtility.GetDownSkillCommand4)
                {
                    index = 3;
                }

                if(index > -1)
                {
                    PlayerParameter pp = _Param as PlayerParameter;
                    _CommandHolder.GetActiveSkillForRun(index).DoRun(_Param, _Rb.component, GravityDirection, pp.ReticlePoint - pp.EyePoint.position, ref _AnimKind);
                    _DoAction = true;

                    Debug.Log($"�R�}���h�� : {_CommandHolder.Running.Name}");
                    Debug.Log($"�A�N�V�����\�� : {_AnimKind}");
                }               
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
