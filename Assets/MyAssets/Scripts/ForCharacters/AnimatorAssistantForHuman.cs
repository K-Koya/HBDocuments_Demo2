using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

[RequireComponent(typeof(Animator))]
public class AnimatorAssistantForHuman : MonoBehaviour
{
    /// <summary>�ړ������̊p�x</summary>
    const float _DIRECTION_OUT_OF_RANGE = 10000f;

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

    [SerializeField, Tooltip("Animator�̃p�����[�^�[�� : DoCombo")]
    static string _ParamNameDoCombo = "DoCombo";

    [SerializeField, Tooltip("Animator�̃p�����[�^�[�� : DoDodge")]
    static string _ParamNameDoDodge = "DoDodge";

    [SerializeField, Tooltip("Animator�̃p�����[�^�[�� : DirectionAngle")]
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

        //����s���p�A�j���[�V������ݒ�
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
}
