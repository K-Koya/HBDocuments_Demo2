using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

[RequireComponent(typeof(Animator))]
public class AnimatorAssistantForHuman : MonoBehaviour
{
    /// <summary>該当のキャラクターを移動させるコンポーネント</summary>
    CharacterMove _Cm = default;

    /// <summary>該当のアニメーター</summary>
    Animator _Am = default;

    [SerializeField, Tooltip("Animatorのパラメーター名 : Speed")]
    static string _ParamNameSpeed = "Speed";

    [SerializeField, Tooltip("Animatorのパラメーター名 : IsGround")]
    static string _ParamNameIsGround = "IsGround";

    [SerializeField, Tooltip("Animatorのパラメーター名 : IsArmed")]
    static string _ParamNameIsArmed = "IsArmed";

    [SerializeField, Tooltip("Animatorのパラメーター名 : DoJump")]
    static string _ParamNameDoJump = "DoJump";

    [SerializeField, Tooltip("Animatorのパラメーター名 : DoAction")]
    static string _ParamNameDoAction = "DoAction";

    [SerializeField, Tooltip("Animatorのパラメーター名 : AnimationKind")]
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

                    //前後左右4方向のどれに近いか
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

    #region アニメーションイベント

    /// <summary>アニメーションイベントにて、アニメーション遷移におけるフリーズ回避のため、待機状態にする</summary>
    public void StateCallStaying()
    {
        _Cm.StateCallStaying();
    }

    /// <summary>アニメーションイベントにて、予備動作に入った情報を受け取る</summary>
    public void ProcessCallPreparation()
    {
        _Cm.ProcessCallPreparation();
    }

    /// <summary>アニメーションイベントにて、本動作に入った情報を受け取る</summary>
    public void ProcessCallPlaying()
    {
        _Cm.ProcessCallPlaying();
    }

    /// <summary>アニメーションイベントにて、動作の空き時間になった情報を受け取る</summary>
    public void ProcessCallInterval()
    {
        _Cm.ProcessCallInterval();
    }

    /// <summary>アニメーションイベントにて、動作終了予定の情報を受け取る</summary>
    public void ProcessCallEndSoon()
    {
        _Cm.ProcessCallEndSoon();
    }

    #endregion

    /// <summary>アニメーションの種類</summary>
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
