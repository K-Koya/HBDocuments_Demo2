using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBase
{
    [SerializeField, Tooltip("�A�N�e�B�u�X�L���R�}���h�̏���")]
    protected byte _Priolity = 0;

    /// <summary>�A�N�e�B�u�X�L���R�}���h�̏���</summary>
    public byte Priolity { get => _Priolity; }


    /// <summary>�R�}���h��ID</summary>
    public virtual ushort Id { get => 0; }

    /// <summary>�R�}���h��</summary>
    public virtual string Name { get => "EMPTY"; }

    /// <summary>�R�}���h������</summary>
    public virtual string Explain { get => ""; }

    /// <summary>�R�}���h�̏���MP</summary>
    public virtual byte MPCost { get => 0; }

    /// <summary>���݂̃A�C�e���̏�����</summary>
    public virtual byte CurrentInventory { get => 0; }

    /// <summary>�ő�̃A�C�e���̏�����</summary>
    public virtual byte MaxInventory { get => 0; }

    /// <summary>�R�}���h���</summary>
    public virtual CommandKind Kind { get => CommandKind.Blank; }


    /// <summary>�U�����e�[�u��</summary>
    protected virtual AttackPowerColumn[] AttackPowerTable { get => null; }

    
    /// <summary>�R�}���h�ۊǌɂ��畡�����郁�\�b�h</summary>
    /// <returns></returns>
    public CommandBase Clone()
    {
        return MemberwiseClone() as CommandBase;
    }

    /// <summary>�U�������\�����U���͈͂��擾����</summary>
    /// <param name="ID">�U����񃊃X�g�ɑΉ�����ID</param>
    /// <returns>�U�����</returns>
    public AttackPowerColumn GetAttackArea(int ID)
    {
        if (AttackPowerTable is null) return null;

        foreach (AttackPowerColumn col in AttackPowerTable)
        {
            if (col.ID == ID)
            {
                return col;
            }
        }

        return null;
    }


    /// <summary>�R�}���h�f�[�^�e�[�u�����擾</summary>
    /// <returns>�R�}���h��ID</returns>
    public virtual ushort LoadData() { return 0; }

    /// <summary>�R�}���h����������</summary>
    /// <param name="layer">�G�̃��C���������̃��C����</param>
    public virtual void Initialize(int layer) { }

    /// <summary>�R�}���h�J�n</summary>
    /// <param name="param">�Y���L�����N�^�[�̃p�����[�^</param>
    /// <param name="rb">���W�b�h�{�f�B</param>
    /// <param name="gravityDirection">�d�͕���</param>
    /// <param name="reticleDirection">�Ə�����</param>
    /// <param name="animKind">�v������A�j���[�V�����̎��</param>
    public virtual void DoRun(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {

    }

    /// <summary>�R�}���h���s��</summary>
    /// <param name="param">�Y���L�����N�^�[�̃p�����[�^</param>
    /// <param name="rb">���W�b�h�{�f�B</param>
    /// <param name="gravityDirection">�d�͕���</param>
    /// <param name="reticleDirection">�Ə�����</param>
    /// <param name="animKind">�v������A�j���[�V�����̎��</param>
    public virtual void Running(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {

    }

    /// <summary>�R�}���h�ǉ�����</summary>
    /// <param name="param">�Y���L�����N�^�[�̃p�����[�^</param>
    /// <param name="rb">���W�b�h�{�f�B</param>
    /// <param name="gravityDirection">�d�͕���</param>
    /// <param name="reticleDirection">�Ə�����</param>
    /// <param name="animKind">�v������A�j���[�V�����̎��</param>
    public virtual void Additional(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {

    }

    /// <summary>�R�}���h�I����</summary>
    /// <param name="param">�Y���L�����N�^�[�̃p�����[�^</param>
    /// <param name="rb">���W�b�h�{�f�B</param>
    /// <param name="gravityDirection">�d�͕���</param>
    /// <param name="animKind">�v������A�j���[�V�����̎��</param>
    public virtual void PostProcess(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, ref AnimationKind animKind)
    {

    }

    /// <summary>��������I�u�W�F�N�g�𐶐����鏈��</summary>
    /// <param name="param">���L�����N�^�[�̃��C���p�����[�^</param>
    /// <param name="info">�U�����</param>
    /// <param name="emitPoint">�ˏo���W</param>
    public virtual void ObjectCreation(CharacterParameter param, AttackInformation info, Vector3 emitPoint)
    {

    }
}

/// <summary>�R�}���h�̎��</summary>
public enum CommandKind : byte
{
    /// <summary>���}��</summary>
    Blank = 0,
    /// <summary>�񕜈ȊO�̃T�|�[�g�R�}���h</summary>
    Support,
    /// <summary>�ʏ�R���{�R�}���h</summary>
    Combo,
    /// <summary>�A�^�b�N�R�}���h</summary>
    Attack,
    /// <summary>�񕜈ȊO�̃A�C�e���R�}���h</summary>
    Item,
    /// <summary>�񕜃T�|�[�g�R�}���h</summary>
    SupportHeal,
    /// <summary>�񕜃A�C�e���R�}���h</summary>
    ItemHeal,
    /// <summary>�p�b�V�u�R�}���h</summary>
    Passive,
}

/// <summary>�U����񃊃X�g���\������J����</summary>
public class AttackPowerColumn
{
    /// <summary><para>�e�[�u���J������ID</para><para>������L�[�ɍU�������Ƃ�</para></summary>
    short _ID = 0;

    /// <summary>���ڍU���̈З͕␳�l(%)</summary>
    short _DamageRatio = 0;

    /// <summary>�ԐڍU���̈З͕␳�l(%)</summary>
    short _MagicDamageRatio = 0;

    /// <summary><para>�e�[�u���J������ID</para><para>������L�[�ɍU�������Ƃ�</para></summary>
    public short ID { get => _ID; }
    /// <summary>���ڍU���̈З͕␳�l(%)</summary>
    public short DamageRatio { get => _DamageRatio; }
    /// <summary>�ԐڍU���̈З͕␳�l(%)</summary>
    public short MagicDamageRatio { get => _MagicDamageRatio; }

    public AttackPowerColumn(short iD, short damageRatio, short magicDamageRatio)
    {
        _ID = iD;
        _DamageRatio = damageRatio;
        _MagicDamageRatio = magicDamageRatio;
    }
}

/// <summary>�񕜏�񃊃X�g���\������J����</summary>
public class HealPowerColumn
{
    /// <summary><para>�e�[�u���J������ID</para><para>������L�[�ɉ񕜏����Ƃ�</para></summary>
    short _ID = 0;

    /// <summary>�ő�HP�ɑ΂���񕜗ʊ���(%)</summary>
    short _HealRatioByHP = 0;

    /// <summary>�񕜂ɔ�₷����(s)</summary>
    byte _HealTime = 0;

    /// <summary>�e�[�u���J������ID ������L�[�ɉ񕜏����Ƃ�</summary>
    public short ID { get => _ID; }
    /// <summary>�ő�HP�ɑ΂���񕜗ʊ���(%)</summary>
    public short HealRatioByHP { get => _HealRatioByHP; }
    /// <summary>�񕜂ɔ�₷����(s)</summary>
    public byte HealTime { get => _HealTime; }

    public HealPowerColumn(short iD, short healRatioByHP, byte healTime)
    {
        _ID = iD;
        _HealRatioByHP = healRatioByHP;
        _HealTime = healTime;
    }
}


