using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorAssistantForDragon : AnimatorAssistant
{
    // Update is called once per frame
    void Update()
    {
        if (_Cm.JumpFlag) _Am.SetTrigger(_PARAM_NAME_DO_JUMP);
        _Am.SetFloat(_PARAM_NAME_SPEED, _Cm.Speed);

        if (_Cm.DoAction)
        {
            _Am.SetTrigger(_PARAM_NAME_DO_ACTION);
        }
    }
}
