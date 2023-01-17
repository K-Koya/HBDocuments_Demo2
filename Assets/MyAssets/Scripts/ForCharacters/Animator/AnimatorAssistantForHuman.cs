using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorAssistantForHuman : AnimatorAssistant
{
    /// <summary>アニメーターのレイヤ名 : 武装時</summary>
    const string LAYER_NAME_ARMED_MOTION = "Armed Motion";

    /// <summary>アニメーターのレイヤ番号 : 武装時</summary>
    int layerNumberArmedMotion = 0;

    protected override void Start()
    {
        base.Start();

        layerNumberArmedMotion = _Am.GetLayerIndex(LAYER_NAME_ARMED_MOTION);
    }

    // Update is called once per frame
    void Update()
    {
        if (_Cm.JumpFlag) _Am.SetTrigger(_PARAM_NAME_DO_JUMP);
        _Am.SetFloat(_PARAM_NAME_SPEED, _Cm.Speed);
        _Am.SetBool(_PARAM_NAME_IS_GROUND, _Cm.IsGround);

        if(_Cm.ArmedTimer > 0.5f)
        {
            _Am.SetLayerWeight(layerNumberArmedMotion, 1f);
            foreach(GameObject aoa in _AppearOnAttack)
            {
                aoa.SetActive(true);
            }
        }
        else
        {
            _Am.SetLayerWeight(layerNumberArmedMotion, Mathf.Clamp01(_Cm.ArmedTimer * 2f));
            foreach (GameObject aoa in _AppearOnAttack)
            {
                aoa.SetActive(false);
            }
        }
        
        if (_Cm.DoAction)
        {
            _Am.SetInteger(_PARAM_NAME_ANIMATION_KIND, (int)_Cm.AnimKind);
            _Am.SetTrigger(_PARAM_NAME_DO_ACTION);
        }
        else _Am.SetInteger(_PARAM_NAME_ANIMATION_KIND, (int)AnimationKind.NoCall);
    }
}
