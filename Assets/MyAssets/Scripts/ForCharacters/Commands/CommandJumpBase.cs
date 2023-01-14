using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommandJumpBase : CommandBase
{
    public CommandJumpBase()
    {
        _Name = "�W�����v";
        _Explain = "�ʏ�̃W�����v�B�n�ɑ������Ă���Ƃ��ɃW�����v�{�^���������ƁA��яオ��B���������ō������ς��B";
    }

    /// <summary>�W�����v����</summary>
    /// <param name="param">�Y���L�����N�^�[�̃p�����[�^</param>
    /// <param name="rb">���W�b�h�{�f�B</param>
    /// <param name="gravityDirection">�d�͕���</param>
    /// <param name="animKind">�v������A�j���[�V�����̎��</param>
    public virtual void JumpOrder(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, ref AnimationKind animKind)
    {
        rb.AddForce(-gravityDirection * 7f, ForceMode.VelocityChange);
        param.State.Kind = MotionState.StateKind.JumpNoraml;
        param.State.Process = MotionState.ProcessKind.Playing;
    }

    /// <summary>�W�����v��̋󒆏���</summary>
    /// <param name="param">�Y���L�����N�^�[�̃p�����[�^</param>
    /// <param name="rb">���W�b�h�{�f�B</param>
    /// <param name="gravityDirection">�d�͕���</param>
    public virtual void JumpOrderOnAir(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection)
    {
        rb.velocity = Vector3.ProjectOnPlane(rb.velocity, -gravityDirection);
        param.State.Kind = MotionState.StateKind.FallNoraml;
        param.State.Process = MotionState.ProcessKind.Playing;
    }

    /// <summary>���n���̏���</summary>
    /// <param name="param">�Y���L�����N�^�[�̃p�����[�^</param>
    public virtual void LandingProcess(CharacterParameter param)
    {
        param.State.Kind = MotionState.StateKind.Stay;
        param.State.Process = MotionState.ProcessKind.Playing;
    }
}
