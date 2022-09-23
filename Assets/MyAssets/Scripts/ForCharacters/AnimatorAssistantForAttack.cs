using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

[RequireComponent(typeof(Animator))]
public class AnimatorAssistantForAttack : MonoBehaviour
{
    /// <summary>該当のキャラクターを移動させるコンポーネント</summary>
    CharacterMove _Cm = default;

    /// <summary>該当のアニメーター</summary>
    Animator _Am = default;

    [SerializeField, Tooltip("Animatorのパラメーター名 : DoCombo")]
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
