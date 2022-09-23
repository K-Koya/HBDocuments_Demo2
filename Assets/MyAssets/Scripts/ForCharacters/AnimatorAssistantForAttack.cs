using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

[RequireComponent(typeof(Animator))]
public class AnimatorAssistantForAttack : MonoBehaviour
{
    /// <summary>�Y���̃L�����N�^�[���ړ�������R���|�[�l���g</summary>
    CharacterMove _Cm = default;

    /// <summary>�Y���̃A�j���[�^�[</summary>
    Animator _Am = default;

    [SerializeField, Tooltip("Animator�̃p�����[�^�[�� : DoCombo")]
    string _ParamNameDoCombo = "DoCombo";

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

    // Update is called once per frame
    void Update()
    {
        if (_Cm.DoCombo) _Am.SetTrigger(_ParamNameDoCombo);
    }
}
