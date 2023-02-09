using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

[RequireComponent(typeof(Animator))]
public class AnimatorAssistant : MonoBehaviour
{
    /// <summary>Animator�̃p�����[�^�[�� : Speed</summary>
    protected const string _PARAM_NAME_SPEED = "Speed";

    /// <summary>Animator�̃p�����[�^�[�� : IsGround</summary>
    protected const string _PARAM_NAME_IS_GROUND = "IsGround";

    /// <summary>Animator�̃p�����[�^�[�� : IsArmed</summary>
    protected const string _PARAM_NAME_IS_ARMED = "IsArmed";

    /// <summary>Animator�̃p�����[�^�[�� : DoJump</summary>
    protected const string _PARAM_NAME_DO_JUMP = "DoJump";

    /// <summary>Animator�̃p�����[�^�[�� : DoAction</summary>
    protected const string _PARAM_NAME_DO_ACTION = "DoAction";

    /// <summary>Animator�̃p�����[�^�[�� : DoDamage</summary>
    protected const string _PARAM_NAME_DO_DAMAGE = "DoDamage";

    /// <summary>Animator�̃p�����[�^�[�� : AnimationKind</summary>
    protected const string _PARAM_NAME_ANIMATION_KIND = "AnimationKind";


    /// <summary>�Y���̃L�����N�^�[���ړ�������R���|�[�l���g</summary>
    protected CharacterMove _Cm = default;

    /// <summary>�Y���̃A�j���[�^�[</summary>
    protected Animator _Am = default;

    [SerializeField, Tooltip("�퓬�������o��������I�u�W�F�N�g")]
    protected GameObject[] _AppearOnAttack = null;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        _Cm = GetComponentInParent<CharacterMove>();

        Timeline ti;
        if (TryGetComponent(out ti))
        {
            _Am = ti.animator.component;
        }
        if (!_Am)
        {
            _Am = GetComponent<Animator>();
        }
    }

    #region �A�j���[�V�����C�x���g

    /// <summary>�A�j���[�V�����C�x���g�ɂāA�A�j���[�V�����J�ڂɂ�����t���[�Y����̂��߁A�ҋ@��Ԃɂ���</summary>
    public void StateCallStaying()
    {
        _Cm.StateCallStaying();
    }

    /// <summary>�A�j���[�V�����C�x���g�ɂāA�\������ɓ����������󂯎��</summary>
    public void ProcessCallPreparation()
    {
        _Cm.ProcessCallPreparation();
    }

    /// <summary>�A�j���[�V�����C�x���g�ɂāA�{����ɓ����������󂯎��</summary>
    public void ProcessCallPlaying()
    {
        _Cm.ProcessCallPlaying();
    }

    /// <summary>�A�j���[�V�����C�x���g�ɂāA����̋󂫎��ԂɂȂ��������󂯎��</summary>
    public void ProcessCallInterval()
    {
        _Cm.ProcessCallInterval();
    }

    /// <summary>�A�j���[�V�����C�x���g�ɂāA����I���\��̏����󂯎��</summary>
    public void ProcessCallEndSoon()
    {
        _Cm.ProcessCallEndSoon();
    }

    /// <summary>���s���R�}���h�̑ł��o�������I�u�W�F�N�g���A�N�e�B�u������</summary>
    /// <param name="index">���</param>
    public void CommandObjectShootCall(int index)
    {
        _Cm.CommandObjectShootCall(index);
    }

    /// <summary>�A�j���[�V�����C�x���g�ɂāA�U��������J�n�������|���󂯎��</summary>
    /// <param name="id">AttackPowerTable�ɃA�N�Z�X�������J����ID</param>
    public void AttackCallStart(int id)
    {
        _Cm.AttackCallStart(id);
    }

    /// <summary>�A�j���[�V�����C�x���g�ɂāA�U��������I���������|���󂯎��</summary>
    public void AttackCallEnd()
    {
        _Cm.AttackCallEnd();
    }

    #endregion
}


/// <summary>�A�j���[�V�����̎��</summary>
public enum AnimationKind : ushort
{
    NoCall = 0,
    Idle,
    Move,
    Jump,
    Fall,

    ShiftSlideFoward = 100,
    ShiftSlideBack,
    ShiftSlideRight,
    ShiftSlideLeft,
    LongTrip,

    GuardGroundFoward = 200,
    GuardGroundBack,
    GuardAirFoward,
    GuardAirBack,

    UseItemSpray = 300,
    UseItemFood,
    UseItemDrink,

    /* �ʏ�R���{ */
    /// <summary>�n��O���ߋ���</summary>
    ComboGroundFoward = 1000,
    /// <summary>�n��O��������</summary>
    ComboGroundFowardFar,
    /// <summary>�n����</summary>
    ComboGroundBack,
    /// <summary>�n��L�p</summary>
    ComboGroundWide,
    /// <summary>�n��t�B�j�b�V��</summary>
    ComboGroundFinish,
    /// <summary>�󒆑O���ߋ���</summary>
    ComboAirFoward,
    /// <summary>�󒆑O��������</summary>
    ComboAirFowardFar,
    /// <summary>�󒆌��</summary>
    ComboAirBack,
    /// <summary>�󒆍L�p</summary>
    ComboAirWide,
    /// <summary>�󒆃t�B�j�b�V��</summary>
    ComboAirFinish,

    /* ���ʋZ���[�V���� */
    /// <summary>���@�e�̂悤�Ȃ��̂𔭎�</summary>
    AttackMagicShoot = 2000,
    /// <summary>���[�U�[�Ȃǂ����̏ꂩ��Ǝ�</summary>
    AttackLaserShoot = 2001,
    /// <summary>���[�U�[�Ȃǂ����̏�ŃX�C���O���Ǝ�</summary>
    AttackLaserShootSwing = 2002,
}

/// <summary>�_���[�W���[�V�����̎��</summary>
public enum DamageKind : byte
{
    /*�Ռ��Ȃ�*/
    /// <summary>�m�[���A�N�V����</summary>
    None = 0,

    /*����*/
    /// <summary>���ʂ��珬���ȏՌ�</summary>
    FrontLittle = 10,
    /// <summary>���ʂ���傫�ȏՌ�</summary>
    FrontBig = 11,

    /*����*/
    /// <summary>���ʈȊO���珬���ȏՌ�</summary>
    SroundLittle = 20,
    /// <summary>���ʈȊO����傫�ȏՌ�</summary>
    SroundBig = 21,
}
