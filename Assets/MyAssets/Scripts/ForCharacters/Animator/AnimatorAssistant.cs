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

    /// <summary>Animator�̃p�����[�^�[�� : AnimationKind</summary>
    protected const string _PARAM_NAME_ANIMATION_KIND = "AnimationKind";


    /// <summary>�Y���̃L�����N�^�[���ړ�������R���|�[�l���g</summary>
    protected CharacterMove _Cm = default;

    /// <summary>�Y���̃A�j���[�^�[</summary>
    protected Animator _Am = default;


    // Start is called before the first frame update
    void Start()
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

    #endregion

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

        UseItemSpray = 500,
        UseItemFood,
        UseItemDrink,

        ComboGroundFoward = 1000,
        ComboGroundFowardFar,
        ComboGroundBack,
        ComboGroundWide,
        ComboGroundFinish,
        ComboAirFoward,
        ComboAirFowardFar,
        ComboAirBack,
        ComboAirWide,
        ComboAirFinish,


    }
}
