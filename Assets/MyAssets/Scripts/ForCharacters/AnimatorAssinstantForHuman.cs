using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMove))]
public class AnimatorAssinstantForHuman : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        _Cm = GetComponent<CharacterMove>();
        _Am = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_Cm.JumpFlag) _Am.SetTrigger(_ParamNameDoJump);
        _Am.SetFloat(_ParamNameSpeed, _Cm.Speed);
        _Am.SetBool(_ParamNameIsGround, _Cm.IsGround);
    }
}
