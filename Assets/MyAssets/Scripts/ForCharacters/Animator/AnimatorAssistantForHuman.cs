using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorAssistantForHuman : AnimatorAssistant
{
    /// <summary>�A�j���[�^�[�̃��C���� : ������</summary>
    const string LAYER_NAME_ARMED_MOTION = "Armed Motion";

    /// <summary>�A�j���[�^�[�̃��C���ԍ� : ������</summary>
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

        if(_Cm.ArmedTimer > 0f)
        {
            _Am.SetLayerWeight(layerNumberArmedMotion, 1f);
        }
        else
        {
            _Am.SetLayerWeight(layerNumberArmedMotion, 0f);
        }
        

        if (_Cm.DoAction)
        {
            _Am.SetTrigger(_PARAM_NAME_DO_ACTION);
            switch (_Cm.State)
            {
                case MotionState.StateKind.ShiftSlide:

                    //�O�㍶�E4�����̂ǂ�ɋ߂���
                    float fowardCheck = Vector3.Dot(_Cm.transform.forward, _Cm.MoveDirection);
                    float rightCheck = Vector3.Dot(_Cm.transform.right, _Cm.MoveDirection);
                    int val = (int)AnimationKind.ShiftSlideBack;
                    if (Mathf.Abs(fowardCheck) > Mathf.Abs(rightCheck))
                    {
                        if(fowardCheck > 0f) val = (int)AnimationKind.ShiftSlideFoward;
                    }
                    else
                    {
                        if (rightCheck > 0f) val = (int)AnimationKind.ShiftSlideRight;
                        else val = (int)AnimationKind.ShiftSlideLeft;
                    }
                    _Am.SetInteger(_PARAM_NAME_ANIMATION_KIND, val);

                    break;
                case MotionState.StateKind.LongTrip:
                    _Am.SetInteger(_PARAM_NAME_ANIMATION_KIND, (int)AnimationKind.LongTrip);
                    break;
                case MotionState.StateKind.ComboNormal:
                    _Am.SetInteger(_PARAM_NAME_ANIMATION_KIND, (int)AnimationKind.ComboGroundFoward);
                    break;
                default: 
                    _Am.SetInteger(_PARAM_NAME_ANIMATION_KIND, (int)AnimationKind.NoCall);
                    break;
            }
        }
        else _Am.SetInteger(_PARAM_NAME_ANIMATION_KIND, (int)AnimationKind.NoCall);
    }
}
