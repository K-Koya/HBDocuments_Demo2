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

/// <summary>�U���R���C�_�[�����e�R���C�_�[�Ɏ󂯓n���U�����̍\����</summary>
public class AttackInformation
{
    /// <summary>����U������͘A���Ń_���[�W�𔭐������Ȃ��悤�Ɋ���U��ID</summary>
    public byte AttackID = 0;

    /// <summary>AttackPowerTable�ɃA�N�Z�X�������J����ID</summary>
    public short AttackPowerTableID = 0;

    /// <summary>�U���҂̃p�����[�^���</summary>
    public CharacterController AttackerParam = null;
}

