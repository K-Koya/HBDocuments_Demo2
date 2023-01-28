using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class AttackCollision : MonoBehaviour
{
    /// <summary>�U���𓖂Ă�����̔�e�R���C�_�[�Ɏ󂯓n���U�����</summary>
    AttackInformation _AttackInfo = null;

    [SerializeField, Tooltip("�U���𓖂Ă����ɏo��G�t�F�N�g�̃v���n�u")]
    GameObject _HitEffectPref = null;

    /// <summary>�U���𓖂Ă����ɏo��G�t�F�N�g�̃v�[��</summary>
    GameObjectPool _HitEffects = null;

    /// <summary>�U���𓖂Ă�����̔�e�R���C�_�[�Ɏ󂯓n���U�����</summary>
    public AttackInformation AttackInfo { get => _AttackInfo; set => _AttackInfo = value; }

    void Start()
    {
        if (_HitEffectPref)
        {
            _HitEffects = new GameObjectPool(_HitEffectPref, 5);
        }
    }

    /// <summary>DamageCollision���ŌĂяo���A�q�b�g�G�t�F�N�g�̔������\�b�h</summary>
    public void CallHitEffect()
    {
        GameObject eff = _HitEffects.Instansiate();
        eff.transform.position = transform.position;
    }
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
    MainParameter _AttackerParam = null;


    /// <summary>����U������͘A���Ń_���[�W�𔭐������Ȃ��悤�Ɋ���U��ID</summary>
    public byte AttackID { get => _AttackID; }
    /// <summary>�U����񃊃X�g���̃J����</summary>
    public AttackPowerColumn Info { get => _Info; }
    /// <summary>�U���҂̃p�����[�^���</summary>
    public MainParameter AttackerParam { get => _AttackerParam; }


    public AttackInformation(AttackPowerColumn�@info, MainParameter param)
    {
        _AttackIDRecord = _AttackIDRecord + 1 < byte.MaxValue ? (byte)(_AttackIDRecord + 1) : byte.MinValue;
        _AttackID = _AttackIDRecord;
        _Info = info;
        _AttackerParam = param;
    }
}

