using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

[RequireComponent(typeof(Animator))]
public class AnimatorAssistantForHuman : MonoBehaviour
{
    /// <summary>移動方向の角度</summary>
    const float _DIRECTION_OUT_OF_RANGE = 10000f;

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

    [SerializeField, Tooltip("Animatorのパラメーター名 : DoCombo")]
    static string _ParamNameDoCombo = "DoCombo";

    [SerializeField, Tooltip("Animatorのパラメーター名 : DoDodge")]
    static string _ParamNameDoDodge = "DoDodge";

    [SerializeField, Tooltip("Animatorのパラメーター名 : DirectionAngle")]
    static string _ParamNameDirectionAngle = "DirectionAngle";


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
        if (_Cm.DoCombo) _Am.SetTrigger(_ParamNameDoCombo);
        if (_Cm.DoDodge) _Am.SetTrigger(_ParamNameDoDodge);

        //回避行動用アニメーションを設定
        if(Vector3.SqrMagnitude(_Cm.MoveDirection) > 0f)
        {
            float angle = Vector3.Angle(_Cm.transform.forward, _Cm.MoveDirection);
            if (Vector3.Dot(_Cm.transform.right, _Cm.MoveDirection) < 0f) angle *= -1f;
            _Am.SetFloat(_ParamNameDirectionAngle, angle);
        }
        else
        {
            _Am.SetFloat(_ParamNameDirectionAngle, _DIRECTION_OUT_OF_RANGE);
        }
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
}
