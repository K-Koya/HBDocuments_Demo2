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
        Move = MoveOnPlane;
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
    void MoveOnPlane()
    {
        //�ړ�����
        _Param.Direction = CalculateMoveDirection(InputUtility.GetMoveDirection);

        //���͂�����Έړ��͂̏���
        if (_Param.Direction.sqrMagnitude > 0)
        {
            //�ړ����͂̑傫�����擾
            _CurrentMovePower = _Param.Direction.magnitude;
            //�ړ��������擾
            _Param.Direction *= 1 / _CurrentMovePower;
        }
        else
        {
            _CurrentMovePower = 0f;
            _Param.Direction = Vector3.zero;
        }

        //�d�͕����ȊO�ňړ��ʐ������������ꍇ�A�u���[�L�ʂ��v�Z����
        bool isMoving = Vector3.SqrMagnitude(VelocityOnPlane) > 0.01f;
        if (isMoving)
        {
            _ForceOfBrake = -VelocityOnPlane.normalized * 0.2f;
        }


        //�W�����v�͌���
        if (InputUtility.GetJump && !IsGround && Vector3.Angle(-GravityDirection, _Rb.velocity) < 90f)
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
}
