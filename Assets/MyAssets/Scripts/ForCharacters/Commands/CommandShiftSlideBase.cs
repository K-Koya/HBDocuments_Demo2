using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommandShiftSlideBase : CommandBase
{
    public CommandShiftSlideBase()
    {
        _Name = "�V�t�g�X���C�h";
        _Explain = "�Z�����̉����i�B�n�ɑ����ė����~�܂�A����{�^���������Ȃ���ړ�����ƁA���������������Ȃ���O�㍶�E�ɑ̂��͂炤�B";
    }

    /// <summary>�Z��������i�V�t�g�X���C�h�j����</summary>
    /// <param name="param">�Y���L�����N�^�[�̃p�����[�^</param>
    /// <param name="rb">���W�b�h�{�f�B</param>
    /// <param name="gravityDirection">�d�͕���</param>
    public virtual void ShiftSlideOrder(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, ref AnimationKind animKind)
    {
        rb.velocity = Vector3.Project(rb.velocity, gravityDirection);
        rb.AddForce(param.Direction * 6f, ForceMode.VelocityChange);
        param.State.Kind = MotionState.StateKind.ShiftSlide;
        param.State.Process = MotionState.ProcessKind.Playing;
    }

    /// <summary>�Z��������i�V�t�g�X���C�h�j��������</summary>
    /// <param name="param">�Y���L�����N�^�[�̃p�����[�^</param>
    /// <param name="rb">���W�b�h�{�f�B</param>
    /// <param name="gravityDirection">�d�͕���</param>
    public virtual void ShiftSlidePostProcess(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection)
    {
        if (param.State.Process == MotionState.ProcessKind.Interval)
        {
            rb.velocity = Vector3.Project(rb.velocity, -gravityDirection);
            param.State.Process = MotionState.ProcessKind.Preparation;
        }
    }
}
