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
    string _ParamNameSpeed = "Speed";

    [SerializeField, Tooltip("Animator�̃p�����[�^�[�� : IsGround")]
    string _ParamNameIsGround = "IsGround";

    [SerializeField, Tooltip("Animator�̃p�����[�^�[�� : DoJump")]
    string _ParamNameDoJump = "DoJump";

    [SerializeField, Tooltip("Animator�̃p�����[�^�[�� : DoCombo")]
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

    /// <summary>�U���A�j���[�V�����J�n�����󂯎��</summary>
    public void AttackStartCall()
    {
        _Cm.AttackStartCall();
    }

    /// <summary>�U���A�j���[�V�����̍U�������̏I�������󂯎��</summary>
    public void AttackEndCall()
    {
        _Cm.AttackEndCall();
    }

    /// <summary>�R���{�t�B�j�b�V���A�j���[�V�����̍U�������̏I�������󂯎��</summary>
    public void ComboFinishEndCall()
    {
        _Cm.ComboFinishEndCall();
    }

    /// <summary>�R���{�ǉ����͎�t</summary>
    public void ComboAcceptCall()
    {
        _Cm.ComboAcceptCall();
    }

    /// <summary>�U���A�j���[�V�������̂��̂̏I�������󂯎��</summary>
    public void AttackAnimationEndCall()
    {
        _Cm.AttackAnimationEndCall();
    }
}
