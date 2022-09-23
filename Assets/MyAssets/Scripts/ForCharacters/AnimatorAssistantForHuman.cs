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
    string _ParamNameSpeed = "Speed";

    [SerializeField, Tooltip("Animatorのパラメーター名 : IsGround")]
    string _ParamNameIsGround = "IsGround";

    [SerializeField, Tooltip("Animatorのパラメーター名 : DoJump")]
    string _ParamNameDoJump = "DoJump";

    [SerializeField, Tooltip("Animatorのパラメーター名 : DoCombo")]
    string _ParamNameDoCombo = "DoCombo";

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
        if (_Cm.DoCombo) _Am.SetTrigger(_ParamNameDoCombo);
    }

    /// <summary>攻撃アニメーション開始情報を受け取る</summary>
    public void AttackStartCall()
    {
        _Cm.AttackStartCall();
    }

    /// <summary>攻撃アニメーションの攻撃部分の終了情報を受け取る</summary>
    public void AttackEndCall()
    {
        _Cm.AttackEndCall();
    }

    /// <summary>コンボフィニッシュアニメーションの攻撃部分の終了情報を受け取る</summary>
    public void ComboFinishEndCall()
    {
        _Cm.ComboFinishEndCall();
    }

    /// <summary>コンボ追加入力受付</summary>
    public void ComboAcceptCall()
    {
        _Cm.ComboAcceptCall();
    }

    /// <summary>攻撃アニメーションそのものの終了情報を受け取る</summary>
    public void AttackAnimationEndCall()
    {
        _Cm.AttackAnimationEndCall();
    }
}
