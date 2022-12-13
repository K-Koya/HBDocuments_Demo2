using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

[RequireComponent(typeof(Animator))]
public class AnimatorAssistant : MonoBehaviour
{
    /// <summary>Animatorのパラメーター名 : Speed</summary>
    protected const string _PARAM_NAME_SPEED = "Speed";

    /// <summary>Animatorのパラメーター名 : IsGround</summary>
    protected const string _PARAM_NAME_IS_GROUND = "IsGround";

    /// <summary>Animatorのパラメーター名 : IsArmed</summary>
    protected const string _PARAM_NAME_IS_ARMED = "IsArmed";

    /// <summary>Animatorのパラメーター名 : DoJump</summary>
    protected const string _PARAM_NAME_DO_JUMP = "DoJump";

    /// <summary>Animatorのパラメーター名 : DoAction</summary>
    protected const string _PARAM_NAME_DO_ACTION = "DoAction";

    /// <summary>Animatorのパラメーター名 : AnimationKind</summary>
    protected const string _PARAM_NAME_ANIMATION_KIND = "AnimationKind";


    /// <summary>該当のキャラクターを移動させるコンポーネント</summary>
    protected CharacterMove _Cm = default;

    /// <summary>該当のアニメーター</summary>
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
