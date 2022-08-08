using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersControlTower : MonoBehaviour, IController
{
    public InputState AimCommand()
    {
        return SwitchForInputState(InputUtility.AimCommandAction);
    }

    public InputState Attack()
    {
        return SwitchForInputState(InputUtility.AttackAction);
    }

    public InputState DodgeLong()
    {
        return SwitchForInputState(InputUtility.DodgeAction);
    }

    public InputState DodgeShort()
    {
        return SwitchForInputState(InputUtility.DodgeAction);
    }

    public InputState Guard()
    {
        return SwitchForInputState(InputUtility.GuardAction);
    }

    public InputState Jump()
    {
        return SwitchForInputState(InputUtility.JumpAction);
    }

    public InputState Move()
    {
        return SwitchForInputState(InputUtility.MoveDirectionAction);
    }

    public Vector2 MoveDirection()
    {
        return InputUtility.MoveDirectionAction.ReadValue<Vector2>();
    }

    /// <summary>InputAction�̏�Ԃ����āA�ǂ̓��͏�Ԃ�������</summary>
    /// <param name="inputAction">���͏�Ԃ�����Ώ�</param>
    /// <returns>���͏�ԏ��</returns>
    InputState SwitchForInputState(InputAction inputAction)
    {
        InputState returnal = InputState.Untouched;
        if (inputAction.WasReleasedThisFrame())
        {
            returnal = InputState.PushUp;
        }
        else if (inputAction.WasPressedThisFrame())
        {
            returnal = InputState.PushDown;
        }
        else if (inputAction.IsPressed())
        {
            returnal = InputState.Pushing;
        }
        return returnal;
    }
}
