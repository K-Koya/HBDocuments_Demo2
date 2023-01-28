using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFireShot : CommandActiveSkillBase
{
    [SerializeField, Tooltip("�Ή��e�v���n�u")]
    GameObject _FireBallPref = null;

    /// <summary>�Ή��e�I�u�W�F�N�g�̃v�[��</summary>
    GameObjectPool _FireBalls = null;

    /// <summary>���̃R���{�̎萔</summary>
    byte _Step = 0;

    public CommandFireShot()
    {
        _Name = "�t�@�C�A�V���b�g";
        _Explain = "�Ή��e���Ə������֕��B�ǉ����͂�5���܂ŕ��Ă邪�A�����I����܂Ŗ��h���ɂȂ�B";
    }

    public override void Initialize()
    {
        if (_FireBallPref)
        {
            _FireBalls = new GameObjectPool(_FireBallPref, 5);
        }
    }

    /// <summary>�Ή��e���˃��\�b�h</summary>
    /// <param name="param">�Y���L�����N�^�[�̃p�����[�^</param>
    /// <param name="rb">���W�b�h�{�f�B</param>
    /// <param name="gravityDirection">�d�͕���</param>
    /// <param name="reticleDirection">�Ə�����</param>
    /// <param name="animKind">�v������A�j���[�V�����̎��</param>
    public override void DoRun(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {
        //�Ə������Ɍ�������
        rb.transform.forward = Vector3.ProjectOnPlane(reticleDirection, -gravityDirection);

        animKind = AnimationKind.ComboGroundFinish;
        //�R���{�t�B�j�b�V��
        if (_Step > 4)
        {
            _Step = 1;
        }

        param.State.Kind = MotionState.StateKind.AttackCommand;
        param.State.Process = MotionState.ProcessKind.Preparation;
    }
}
