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

    /// <summary>InputActionの状態を見て、どの入力状態かを見る</summary>
    /// <param name="inputAction">入力状態を見る対象</param>
    /// <returns>入力状態情報</returns>
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
