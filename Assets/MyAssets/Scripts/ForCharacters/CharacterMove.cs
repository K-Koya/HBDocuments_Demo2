using System;
using System.Collections.Generic;
using UnityEngine;
using Chronos;


[RequireComponent(typeof(Rigidbody), typeof(CharacterParameter))]
public class CharacterMove : MonoBehaviour
{
    #region �萔
    /// <summary>���x��0�ł���Ƃ݂Ȃ����l</summary>
    protected const float VELOCITY_ZERO_BORDER = 0.5f;
    #endregion

    #region �����o
    [SerializeField, Tooltip("�ړ�����ɂ����鐳�ʕ������w��")]
    MoveForwardMode _MoveForwardMode = MoveForwardMode.WorldTransformZ;

    /// <summary>MainCamera�̈ʒu���̏��</summary>
    Transform _MainCameraTransform = default;

    /// <summary>���͏�</summary>
    IController _Controller = null;

    /// <summary>�L�����N�^�[�̎����</summary>
    protected CharacterParameter _Param = null;

    /// <summary>���Y�L�����N�^�[�̕��������R���|�[�l���g</summary>
    protected RigidbodyTimeline3D _Rb = null;

    /// <summary>true : �W�����v����</summary>
    protected bool _JumpFlag = false;

    /// <summary>���n���������R���|�[�l���g</summary>
    GroundChecker _GroundChecker = default;

    /// <summary>�ړ����͂̑傫��</summary>
    protected float _CurrentMovePower = 0f;

    /// <summary>������u���[�L��</summary>
    protected Vector3 _ForceOfBrake = Vector3.zero;


    /// <summary>�ړ��p���\�b�h</summary>
    protected Action Move = default;

    #endregion

    #region �v���p�e�B

    public CharacterParameter Status => _Param;
    /// <summary>True : ���n���Ă���</summary>
    public bool IsGround => _GroundChecker.IsGround;
    /// <summary>�d�͕���</summary>
    protected Vector3 GravityDirection => _GroundChecker.GravityDirection;
    /// <summary>Rigidbody��velocity���ړ��������ʂɊ��Z��������</summary>
    protected Vector3 VelocityOnPlane => Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection);
    /// <summary>�ړ����x</summary>
    public float Speed => VelocityOnPlane.magnitude;
    /// <summary>�W�����v����t���O</summary>
    public bool JumpFlag => _JumpFlag;
    
    #endregion



    // Start is called before the first frame update
    protected virtual void Start()
    {
        _Param = GetComponent<CharacterParameter>();
        _GroundChecker = GetComponent<GroundChecker>();

        _Rb = _Param.Tl.rigidbody;
        _Rb.useGravity = false;

        _MainCameraTransform = Camera.main.transform;
        _Controller = GetComponent<IController>();

        Move = MoveOnPlane;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //timeScale��0�Ȃ�|�[�Y��
        if (!(_Param.Tl.timeScale > 0f)) return;

        Move?.Invoke();
    }

    void FixedUpdate()
    {
        //���n���Ă��邩�ۂ��ŕ���
        //���n��
        if (IsGround)
        {
            //�ړ��͂��������Ă���
            if (_CurrentMovePower > 0f)
            {
                //��]����
                CharacterRotation(_Param.Direction, -GravityDirection, 360f);

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
                CharacterRotation(_Param.Direction, -GravityDirection, 90f);

                //�͂�������
                _Rb.AddForce(_Param.Direction * _CurrentMovePower * 5f, ForceMode.Acceleration);
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
        //�ړ�����ɂ����鐳�ʕ����̎w��ɂ���āA���ʕ�����������������
        switch (_MoveForwardMode)
        {
            case MoveForwardMode.MainCameraTransformZ:
                _Param.Direction = CalculateMoveDirection(_Controller.MoveDirection());
                break;
            default: break;
        }

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
        if (_Controller.Jump() == InputState.Untouched && !IsGround && Vector3.Angle(-GravityDirection, _Rb.velocity) < 90f)
        {
            _Rb.velocity = Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection);
        }

        //���n���ɃW�����v����
        _JumpFlag = false;
        if (IsGround && _Controller.Jump() == InputState.PushDown)
        {
            _Rb.AddForce(-GravityDirection * 7f, ForceMode.VelocityChange);
            _JumpFlag = true;
        }
    }


    /// <summary>�ړ�����ɂ����鐳�ʕ������[�h</summary>
    public enum MoveForwardMode : byte
    {
        /// <summary>���[���h���W��Ԃ�Z���̐�����</summary>
        WorldTransformZ,
        /// <summary>MainCamera�̐��ʕ���</summary>
        MainCameraTransformZ,
    }
}
