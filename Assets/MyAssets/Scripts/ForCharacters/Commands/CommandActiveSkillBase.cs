using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommandActiveSkillBase : CommandBase
{
    #region �����o
    [SerializeField, Tooltip("�A�N�e�B�u�X�L���R�}���h�̏���")]
    protected byte _Priolity = 0;

    [SerializeField, Tooltip("�R�}���h�̐��l�e��\n�X�L��:����MP�R�X�g\n�A�C�e��:��\nCombo:�R���{�萔")]
    protected byte _Count = 1;

    [SerializeField, Tooltip("�U�����e�[�u��")]
    AttackPowerColumn[] _AttackPowerTable = null;

    [SerializeField, Tooltip("true : �R�}���h�{�H��")]
    protected bool _IsRunning = false;

    #endregion

    public CommandActiveSkillBase()
    {
        _Name = "EMPTY";
        _Explain = "�X�L���R�}���h�����}���ł��B";
    }

    #region �v���p�e�B
    /// <summary>�A�N�e�B�u�X�L���R�}���h�̏���</summary>
    public byte Priolity { get => _Priolity; }

    /// <summary>
    /// <para>�R�}���h�̐��l�e��</para>
    /// <para>Skill:����MP�R�X�g</para>
    /// <para>Item:��</para>
    /// <para>Combo:�R���{�萔</para>
    /// </summary>
    public byte Count { get => _Count; }

    /// <summary>true : �R�}���h�{�H��</summary>
    public bool IsRunning { get => _IsRunning; }
    #endregion

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


    /// <summary>�U�������\�����U���͈͂��w�肷��</summary>
    /// <param name="ID">�U����񃊃X�g�ɑΉ�����ID</param>
    /// <returns>�U�����</returns>
    public AttackPowerColumn SetAttackArea(int ID)
    {
        foreach (AttackPowerColumn col in _AttackPowerTable)
        {
            if (col.ID == ID)
            {
                return col;
            }
        }

        return null;
    }

}
