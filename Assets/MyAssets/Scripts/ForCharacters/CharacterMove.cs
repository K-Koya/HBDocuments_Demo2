using System.Collections.Generic;
using UnityEngine;
using Chronos;


[RequireComponent(typeof(Rigidbody), typeof(CharacterParameter))]
abstract public class CharacterMove : MonoBehaviour
{
    #region �萔
    /// <summary>���x��0�ł���Ƃ݂Ȃ����l</summary>
    protected const float VELOCITY_ZERO_BORDER = 0.5f;
    #endregion

    #region �����o
    /// <summary>�ړ������ɗ͂������鎞�̗͂̑傫��</summary>
    float _MovePower = 3.0f;

    /// <summary>���ʂ̈ړ����x</summary>
    float _Speed = 0.0f;

    /// <summary>true : �U���A�j���[�V�������I������</summary>
    protected bool _IsAttackEnd = true;

    /// <summary>true : �R���{�U������</summary>
    protected bool _DoCombo = false;

    /// <summary>�L�����N�^�[�̎����</summary>
    protected CharacterParameter _Param = null;

    /// <summary>���Y�L�����N�^�[�̕��������R���|�[�l���g</summary>
    protected RigidbodyTimeline3D _Rb = null;

    /// <summary>True : �W�����v����</summary>
    protected bool _JumpFlag = false;

    /// <summary>���n���������R���|�[�l���g</summary>
    GroundChecker _GroundChecker = default;

    /// <summary>�ړ����͂̑傫��</summary>
    protected float _MoveInputRate = 0f;

    /// <summary>������u���[�L��</summary>
    protected Vector3 _ForceOfBrake = Vector3.zero;

    /// <summary>�ړ��p���\�b�h</summary>
    protected System.Action Move = null;

    /// <summary>�U���p���\�b�h</summary>
    protected System.Action Attack = null;

    #endregion

    #region �v���p�e�B
    /// <summary>true : �R���{�U�����͂�������(����t���O��܂�)</summary>
    public bool DoCombo => _DoCombo;
    /// <summary>True : ���n���Ă���</summary>
    public bool IsGround => _GroundChecker.IsGround;
    /// <summary>�d�͕���</summary>
    protected Vector3 GravityDirection => _GroundChecker.GravityDirection;
    /// <summary>Rigidbody��velocity���ړ��������ʂɊ��Z��������</summary>
    protected Vector3 VelocityOnPlane => Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection);
    /// <summary>�ړ������ɗ͂������鎞�̗͂̑傫��</summary>
    public float MovePower { set => _MovePower = value; }
    /// <summary>���ʂ̈ړ����x</summary>
    public float Speed => _Speed;
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
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //timeScale��0�Ȃ�|�[�Y��
        if (!(_Param.Tl.timeScale > 0f)) return;

        //���x����
        _Speed = VelocityOnPlane.magnitude;
        Move?.Invoke();
        Attack?.Invoke();
    }

    void FixedUpdate()
    {
        //���n���Ă��邩�ۂ��ŕ���
        //���n��
        if (IsGround)
        {
            //�ړ��͂��������Ă���
            if (_MoveInputRate > 0f)
            {
                //��]����
                CharacterRotation(_Param.Direction, -GravityDirection, 360f);

                //�͂�������
                _Rb.AddForce(transform.forward * _MoveInputRate * _MovePower, ForceMode.Acceleration);

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
            if (_MoveInputRate > 0f)
            {
                //��]����
                CharacterRotation(_Param.Direction, -GravityDirection, 90f);

                //�͂�������
                _Rb.AddForce(_Param.Direction * _MoveInputRate * _MovePower, ForceMode.Acceleration);
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

    
    /// <summary>�A�j���[�V�����C�x���g�ɂāA�U���A�j���[�V�����J�n�����󂯎��</summary>
    public void AttackStartCall()
    {
        _IsAttackEnd = false;
        _Param.State.Process = MotionState.ProcessKind.Playing;

        //�U�����쐬
        foreach(AttackArea at in _Param.AttackAreas)
        {
            CharacterParameter[] attackHits = at.EmitArea(_Param.HostilityLayer);
        }
    }

    /// <summary>�A�j���[�V�����C�x���g�ɂāA�U���A�j���[�V�����̍U�������̏I�������󂯎��</summary>
    public void AttackEndCall()
    {
        _Param.AttackAreas.Clear();
        _Param.State.Process = MotionState.ProcessKind.Interval;
    }

    /// <summary>�A�j���[�V�����C�x���g�ɂāA�R���{�t�B�j�b�V���A�j���[�V�����̍U�������̏I�������󂯎��</summary>
    public void ComboFinishEndCall()
    {
        _Param.State.Process = MotionState.ProcessKind.EndSoon;
    }

    /// <summary>�R���{�ǉ����͎�t</summary>
    public void ComboAcceptCall()
    {
        _IsAttackEnd = true;
    }

    /// <summary>�A�j���[�V�����C�x���g�ɂāA�U���A�j���[�V�������̂��̂̏I�������󂯎��</summary>
    public void AttackAnimationEndCall()
    {
        _IsAttackEnd = true;
        _Param.State.Process = MotionState.ProcessKind.NotPlaying;
    }
}
