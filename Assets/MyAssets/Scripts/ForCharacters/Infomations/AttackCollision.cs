using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    /// <summary>�U���𓖂Ă�����̔�e�R���C�_�[�Ɏ󂯓n���U�����</summary>
    AttackInformation _AttackInfo = null;

    /// <summary>�U���𓖂Ă�����̔�e�R���C�_�[�Ɏ󂯓n���U�����</summary>
    public AttackInformation AttackInfo { set => _AttackInfo = value; }
}

/// <summary>�U���R���C�_�[�����e�R���C�_�[�Ɏ󂯓n���U�����</summary>
public class AttackInformation
{
    /// <summary>�ŐV�̎󂯓n���U�����̎���ID</summary>
    static byte _AttackIDRecord = 0;

    /// <summary>����U������͘A���Ń_���[�W�𔭐������Ȃ��悤�Ɋ���U��ID</summary>
    byte _AttackID = 0;

    /// <summary>�U����񃊃X�g���̃J����</summary>
    AttackPowerColumn _Info = null;

    /// <summary>�U���҂̃p�����[�^���</summary>
    CharacterParameter _AttackerParam = null;


    /// <summary>����U������͘A���Ń_���[�W�𔭐������Ȃ��悤�Ɋ���U��ID</summary>
    public byte AttackID { get => _AttackID; }
    /// <summary>�U����񃊃X�g���̃J����</summary>
    public AttackPowerColumn Info { get => _Info; }
    /// <summary>�U���҂̃p�����[�^���</summary>
    public CharacterParameter AttackerParam { get => _AttackerParam; }


    public AttackInformation(AttackPowerColumn�@info, CharacterParameter param)
    {
        _AttackIDRecord = _AttackIDRecord == byte.MaxValue ? byte.MinValue : _AttackIDRecord++;
        _AttackID = _AttackIDRecord;
        _Info = info;
        _AttackerParam = param;
    }
}

