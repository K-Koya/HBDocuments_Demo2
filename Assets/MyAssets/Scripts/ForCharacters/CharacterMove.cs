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
    protected float _MovePower = 3.0f;

    /// <summary>���ʂ̈ړ����x</summary>
    protected float _Speed = 0.0f;

    /// <summary>�L�����N�^�[�́A������o�����肵�ėՐ�Ԑ��ɂȂ�p���^�C�}�[</summary>
    protected float _ArmedTimer = 0.0f;

    /// <summary>True : �W�����v����</summary>
    protected bool _JumpFlag = false;

    /// <summary>true : �e��A�N�V���������{����</summary>
    protected bool _DoAction = false;

    /// <summary>�L�����N�^�[�̎����</summary>
    protected CharacterParameter _Param = null;

    /// <summary>���Y�L�����N�^�[�̕��������R���|�[�l���g</summary>
    protected RigidbodyTimeline3D _Rb = null;

    /// <summary>���n���������R���|�[�l���g</summary>
    GroundChecker _GroundChecker = default;

    /// <summary>�ړ����͂̑傫��</summary>
    protected float _MoveInputRate = 0f;

    /// <summary>������u���[�L��</summary>
    protected Vector3 _ForceOfBrake = Vector3.zero;

    /// <summary>�ړ��p���\�b�h</summary>
    protected System.Action Move = null;

    /// <summary>�e��s���p���\�b�h</summary>
    protected System.Action Act = null;

    /// <summary>�R�}���h���i�[����R���|�[�l���g</summary>
    protected CommandHolder _CommandHolder = null;

    /// <summary>���p����A�j���[�V�����̎��</summary>
    protected AnimationKind _AnimKind = AnimationKind.NoCall;
    #endregion

    #region �v���p�e�B
    /// <summary>true : �e��A�N�V�������͂�������</summary>
    public bool DoAction { get => _DoAction; }
    /// <summary>���݂̍s�����</summary>
    public MotionState.StateKind State { get => _Param.State.Kind; }
    /// <summary>�L�����N�^�[�́A������o�����肵�ėՐ�Ԑ��ɂȂ�p���^�C�}�[</summary>
    public float ArmedTimer { get => _ArmedTimer; }
    /// <summary>True : ���n���Ă���</summary>
    public bool IsGround { get => _GroundChecker.IsGround; }
    /// <summary>�d�͕���</summary>
    public Vector3 GravityDirection { get => _GroundChecker.GravityDirection; }
    /// <summary>Rigidbody��velocity���ړ��������ʂɊ��Z��������</summary>
    public Vector3 VelocityOnPlane { get => Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection); }
    /// <summary>�ړ������ɗ͂������鎞�̗͂̑傫��</summary>
    public float MovePower { set => _MovePower = value; }
    /// <summary>���ʂ̈ړ����x</summary>
    public float Speed { get => _Speed; }
    /// <summary>�W�����v����t���O</summary>
    public bool JumpFlag { get => _JumpFlag; }
    /// <summary>�ړ�����</summary>
    public Vector3 MoveDirection { get => _Param.Direction; }
    /// <summary>���p����A�j���[�V�����̎��</summary>
    public AnimationKind AnimKind { get => _AnimKind; }
    #endregion



    // Start is called before the first frame update
    protected virtual void Start()
    {
        _Param = GetComponent<CharacterParameter>();
        _GroundChecker = GetComponent<GroundChecker>();

        _CommandHolder = GetComponentInChildren<CommandHolder>();

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

        //����
        Move?.Invoke();
        Act?.Invoke();

        //�Ր�Ԑ�
        SetArmedTimer();
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
                Vector3 moveDirection = _Param.Direction;

                //��]����
                if (_Param.IsSyncDirection) 
                {
                    CharacterRotation(_Param.Direction, -GravityDirection, 360f);
                    moveDirection = transform.forward;
                }

                //�͂�������
                _Rb.AddForce(moveDirection * _MoveInputRate * _MovePower, ForceMode.Acceleration);

                //���x(����)���A���͕����֐ݒ�
                _Rb.velocity = Quaternion.FromToRotation(Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection), moveDirection) * _Rb.velocity;

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
                if (_Param.IsSyncDirection) CharacterRotation(_Param.Direction, -GravityDirection, 90f);

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

    /// <summary>�Ր�Ԑ����`�F�b�N</summary>
    void SetArmedTimer()
    {
        if(State == MotionState.StateKind.ComboNormal)
        {
            _ArmedTimer = 10f;
        }
        else if(_ArmedTimer > 0f)
        {
            _ArmedTimer -= Time.deltaTime;
            if(_ArmedTimer < 0f) _ArmedTimer = 0f;
        }
    }

    #region �A�j���[�V�����C�x���g

    /// <summary>�A�j���[�V�����C�x���g�ɂāA�A�j���[�V�����J�ڂɂ�����t���[�Y����̂��߁A�ҋ@��Ԃɂ���</summary>
    public void StateCallStaying()
    {
        _Param.State.Kind = MotionState.StateKind.Stay;
        _Param.State.Process = MotionState.ProcessKind.Playing;
    }

    /// <summary>�A�j���[�V�����C�x���g�ɂāA�\������ɓ����������󂯎��</summary>
    public void ProcessCallPreparation()
    {
        _Param.State.Process = MotionState.ProcessKind.Preparation;
    }

    /// <summary>�A�j���[�V�����C�x���g�ɂāA�{����ɓ����������󂯎��</summary>
    public void ProcessCallPlaying()
    {
        _Param.State.Process = MotionState.ProcessKind.Playing;
    }

    /// <summary>�A�j���[�V�����C�x���g�ɂāA����̋󂫎��ԂɂȂ��������󂯎��</summary>
    public void ProcessCallInterval()
    {
        _Param.State.Process = MotionState.ProcessKind.Interval;
    }

    /// <summary>�A�j���[�V�����C�x���g�ɂāA����I���\��̏����󂯎��</summary>
    public void ProcessCallEndSoon()
    {
        _Param.State.Process = MotionState.ProcessKind.EndSoon;

        //�R���{�萔�����Z�b�g����
        _CommandHolder.Combo.ComboReset();
    }

    /// <summary>�A�j���[�V�����C�x���g�ɂāA�U��������J�n�������|���󂯎��</summary>
    public void AttackCallStart(int power)
    {
        
    }

    /// <summary>�A�j���[�V�����C�x���g�ɂāA�U��������I���������|���󂯎��</summary>
    public void AttackCallEnd()
    {

    }

    #endregion
}
