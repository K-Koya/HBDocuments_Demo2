using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMove))]
public class AnimatorAssinstantForHuman : MonoBehaviour
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
