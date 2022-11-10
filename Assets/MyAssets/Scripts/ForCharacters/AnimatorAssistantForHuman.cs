using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

[RequireComponent(typeof(Animator))]
public class AnimatorAssistantForHuman : MonoBehaviour
{
    /// <summary>�Y���̃L�����N�^�[���ړ�������R���|�[�l���g</summary>
    CharacterMove _Cm = default;

    /// <summary>�Y���̃A�j���[�^�[</summary>
    Animator _Am = default;

    [SerializeField, Tooltip("Animator�̃p�����[�^�[�� : Speed")]
    static string _ParamNameSpeed = "Speed";

    [SerializeField, Tooltip("Animator�̃p�����[�^�[�� : IsGround")]
    static string _ParamNameIsGround = "IsGround";

    [SerializeField, Tooltip("Animator�̃p�����[�^�[�� : IsArmed")]
    static string _ParamNameIsArmed = "IsArmed";

    [SerializeField, Tooltip("Animator�̃p�����[�^�[�� : DoJump")]
    static string _ParamNameDoJump = "DoJump";

    [SerializeField, Tooltip("Animator�̃p�����[�^�[�� : DoAction")]
    static string _ParamNameDoAction = "DoAction";

    [SerializeField, Tooltip("Animator�̃p�����[�^�[�� : AnimationKind")]
    static string _ParamNameAnimationKind = "AnimationKind";



    // Start is called before the first frame update
    void Start()
    {
        _Cm = GetComponentInParent<CharacterMove>();

        Timeline ti;
        if(TryGetComponent(out ti))
        {
            _Am = ti.animator.component;
        }
        if(!_Am)
        {
            _Am = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_Cm.JumpFlag) _Am.SetTrigger(_ParamNameDoJump);
        _Am.SetFloat(_ParamNameSpeed, _Cm.Speed);
        _Am.SetBool(_ParamNameIsGround, _Cm.IsGround);
        _Am.SetBool(_ParamNameIsArmed, _Cm.ArmedTimer > 0f);

        if (_Cm.DoAction)
        {
            _Am.SetTrigger(_ParamNameDoAction);
            switch (_Cm.State)
            {
                case MotionState.StateKind.ShiftSlide:

                    //�O�㍶�E4�����̂ǂ�ɋ߂���
                    float fowardCheck = Vector3.Dot(_Cm.transform.forward, _Cm.MoveDirection);
                    float rightCheck = Vector3.Dot(_Cm.transform.right, _Cm.MoveDirection);
                    int val = (int)AnimationKind.ShiftSlideBack;
                    if (Mathf.Abs(fowardCheck) > Mathf.Abs(rightCheck))
                    {
                        if(fowardCheck > 0f) val = (int)AnimationKind.ShiftSlideFoward;
                    }
                    else
                    {
                        if (rightCheck > 0f) val = (int)AnimationKind.ShiftSlideRight;
                        else val = (int)AnimationKind.ShiftSlideLeft;
                    }
                    _Am.SetInteger(_ParamNameAnimationKind, val);

                    break;
                case MotionState.StateKind.LongTrip:
                    _Am.SetInteger(_ParamNameAnimationKind, (int)AnimationKind.LongTrip);
                    break;
                case MotionState.StateKind.ComboNormal:
                    _Am.SetInteger(_ParamNameAnimationKind, (int)AnimationKind.ComboGroundFoward);
                    break;
                default: 
                    _Am.SetInteger(_ParamNameAnimationKind, (int)AnimationKind.NoCall);
                    break;
            }
        }
        else _Am.SetInteger(_ParamNameAnimationKind, (int)AnimationKind.NoCall);
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
