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
            _Am.SetInteger(_PARAM_NAME_ANIMATION_KIND, (int)_Cm.AnimKind);
            _Am.SetTrigger(_PARAM_NAME_DO_ACTION);

            Debug.Log($"ƒAƒNƒVƒ‡ƒ“‚ªŒÄ‚Î‚ê‚é : {_Cm.AnimKind}");
        }
        else _Am.SetInteger(_PARAM_NAME_ANIMATION_KIND, (int)AnimationKind.NoCall);
    }
}
