using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

public class PlayerMove : CharacterMove
{
    #region �����o
    /// <summary>MainCamera�̈ʒu���̏��</summary>
    Transform _MainCameraTransform = default;

    /// <summary>���Y�L�����N�^�[�̕��������R���|�[�l���g</summary>
    RigidbodyTimeline3D _Rb = default;

    /// <summary>�ړ����͂̑傫��</summary>
    float _CurrentMovePower = 0f;

    /// <summary>������u���[�L��</summary>
    Vector3 _ForceOfBrake = Vector3.zero;
    #endregion


    #region �v���p�e�B
    /// <summary>Rigidbody��velocity���ړ��������ʂɊ��Z��������</summary>
    Vector3 VelocityOnPlane => Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection);

    /// <summary>�ړ����x</summary>
    public override float Speed => VelocityOnPlane.magnitude;
    #endregion

    protected override void Start()
    {
        base.Start();

        _Rb = _Tl.rigidbody;
        _Rb.useGravity = false;
        _MainCameraTransform = Camera.main.transform;

        Move = MoveOnPlane;
    }

    void FixedUpdate()
    {
        //timeScale��0�Ȃ�|�[�Y��
        if (!(_Tl.timeScale > 0f)) return;

        //���n���Ă��邩�ۂ��ŕ���
        //���n��
        if (IsGround)
        {
            //�ړ��͂��������Ă���
            if (_CurrentMovePower > 0f)
            {
                //��]����
                CharacterRotation(_CharacterDirection, -GravityDirection, 360f);

                //�͂�������
                _Rb.AddForce(transform.forward * _CurrentMovePower * 5f, ForceMode.Acceleration);

                //���x(����)���A���͕����֐ݒ�
                _Rb.velocity = Quaternion.FromToRotation(Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection), transform.forward) * _Rb.velocity;

                //�d�͂�������
                _Rb.AddForce(GravityDirection * 2f, ForceMode.Acceleration);
            }
            //�ړ��͂��Ȃ���΁A���݂̑��x��臒l�������������0�ɂ���
            else
            {
                if (VelocityOnPlane.sqrMagnitude < VELOCITY_ZERO_BORDER)
                {
                    _Rb.velocity = Vector3.Project(_Rb.velocity, -GravityDirection);
                }

                //�d�͂�������
                _Rb.AddForce(GravityDirection * 1f, ForceMode.Acceleration);
            }
        }
        //��
        else
        {
            //�ړ��͂��������Ă���
            if (_CurrentMovePower > 0f)
            {
                //��]����
                CharacterRotation(_CharacterDirection, -GravityDirection, 90f);

                //�͂�������
                _Rb.AddForce(_CharacterDirection * _CurrentMovePower * 5f, ForceMode.Acceleration);
            }

            //�d�͂�������
            _Rb.AddForce(GravityDirection * 9.8f, ForceMode.Acceleration);
        }

        //���x������������
        if (_ForceOfBrake.sqrMagnitude > 0f)
        {
            _Rb.AddForce(_ForceOfBrake, ForceMode.Acceleration);
        }
    }



    /// <summary>�d�͕����̒���ω������Ȃ��ړ����\�b�h</summary>
    void MoveOnPlane()
    {
        //�O�����ƉE�������擾���A�ړ����͒l�𔽉f
        Vector3 vertical = Vector3.ProjectOnPlane(_MainCameraTransform.forward, -GravityDirection);
        vertical = vertical.normalized * InputUtility.GetAxis2DMoveDirection.y;
        Vector3 horizontal = Vector3.ProjectOnPlane(_MainCameraTransform.right, -GravityDirection);
        horizontal = horizontal.normalized * InputUtility.GetAxis2DMoveDirection.x;

        _CharacterDirection = vertical + horizontal;

        //���͂�����Έړ��͂̏���
        if(_CharacterDirection.sqrMagnitude > 0)
        {
            //�ړ����͂̑傫�����擾
            _CurrentMovePower = _CharacterDirection.magnitude;
            //�ړ��������擾
            _CharacterDirection *= 1 / _CurrentMovePower;
        }
        else
        {
            _CurrentMovePower = 0f;
            _CharacterDirection = Vector3.zero;
        }

        //�d�͕����ȊO�ňړ��ʐ������������ꍇ�A�u���[�L�ʂ��v�Z����
        bool isMoving = Vector3.SqrMagnitude(VelocityOnPlane) > 0.01f;
        if (isMoving)
        {
            _ForceOfBrake = -horizontal * 0.2f;
        }


        //�W�����v�͌���
        if (!InputUtility.GetJump && !IsGround && Vector3.Angle(-GravityDirection, _Rb.velocity) < 90f)
        {
            _Rb.velocity = Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection);
        }

        //���n���ɃW�����v����
        _JumpFlag = false;
        if (IsGround && InputUtility.GetDownJump)
        {
            _Rb.AddForce(-GravityDirection * 7f, ForceMode.VelocityChange);
            _JumpFlag = true;
        }
    }

    


    /// <summary> �L�����N�^�[���w������ɉ�]������ </summary>
    /// <param name="targetDirection">�ڕW����</param>
    /// <param name="up">������iVector.Zero�Ȃ��������w�肵�Ȃ��j</param>
    /// <param name="rotateSpeed">��]���x</param>
    protected void CharacterRotation(Vector3 targetDirection, Vector3 up, float rotateSpeed)
    {
        if (targetDirection.sqrMagnitude > 0.0f)
        {
            Vector3 trunDirection = transform.right;
            Quaternion charDirectionQuaternion = Quaternion.identity;
            if (up.sqrMagnitude > 0f) charDirectionQuaternion = Quaternion.LookRotation(targetDirection + (trunDirection * 0.001f), up);
            else charDirectionQuaternion = Quaternion.LookRotation(targetDirection + (trunDirection * 0.001f));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, charDirectionQuaternion, rotateSpeed * Time.deltaTime);
        }
    }
}
