using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommandLongTripBase : CommandBase
{
    public CommandLongTripBase()
    {
        _Name = "�����O�g���b�v";
        _Explain = "�������̉����i�B�n�ɑ����đ����Ă���Ƃ��ɉ���{�^���������ƁA�����Ă��������Ɋ���悤�ɍ����ړ�������B";
    }

    /// <summary>����������i�����O�g���b�v�j����</summary>
    /// <param name="param">�Y���L�����N�^�[�̃p�����[�^</param>
    /// <param name="rb">���W�b�h�{�f�B</param>
    public virtual void LongTripOrder(CharacterParameter param, Rigidbody rb)
    {
        rb.AddForce(param.Direction * 8f, ForceMode.VelocityChange);
        param.State.Kind = MotionState.StateKind.LongTrip;
        param.State.Process = MotionState.ProcessKind.Playing;
    }


    /// <summary>����������i�����O�g���b�v�j��������</summary>
    /// <param name="param">�Y���L�����N�^�[�̃p�����[�^</param>
    /// <param name="rb">���W�b�h�{�f�B</param>
    /// <param name="gravityDirection">�d�͕���</param>
    public virtual void LongTripPostProcess(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection)
    {
        if (param.State.Process == MotionState.ProcessKind.Interval)
        {
            rb.velocity = Vector3.Project(rb.velocity, -gravityDirection);
            param.State.Process = MotionState.ProcessKind.Preparation;
        }
    }
}
