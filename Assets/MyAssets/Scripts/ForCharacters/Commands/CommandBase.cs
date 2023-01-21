using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommandBase
{
    [SerializeField, Tooltip("�R�}���h��")]
    protected string _Name = "";

    [SerializeField, Tooltip("�R�}���h������"), TextArea(2, 3)]
    protected string _Explain = "";

    /// <summary>�R�}���h��</summary>
    public string Name { get => _Name; }

    /// <summary>�R�}���h������</summary>
    public string Explain { get => _Explain; }


    /// <summary>�U����񃊃X�g���\������J����</summary>
    [System.Serializable]
    public class AttackPowerColumn
    {
        [SerializeField, Tooltip("�e�[�u���J������ID\n������L�[�ɍU�������Ƃ�")]
        short _ID = 0;

        [SerializeField, Tooltip("���ڍU���̈З͕␳�l(%)")]
        short _DamageRatio = 0;

        [SerializeField, Tooltip("�ԐڍU���̈З͕␳�l(%)")]
        short _MagicDamageRatio = 0;

        [SerializeField, Tooltip("�U�����Ɏg�������蔻��Y�����W")]
        byte[] _ActivateAreasIndex = null;

        /// <summary>�e�[�u���J������ID n������L�[�ɍU�������Ƃ�</summary>
        public short ID { get => _ID; }
        /// <summary>���ڍU���̈З͕␳�l(%)</summary>
        public short DamageRatio { get => _DamageRatio; }
        /// <summary>�ԐڍU���̈З͕␳�l(%)</summary>
        public short MagicDamageRatio { get => _MagicDamageRatio; }
        /// <summary>�U�����Ɏg�������蔻��Y�����W</summary>
        public byte[] ActivateAreasIndex { get => _ActivateAreasIndex; }
    }

    /// <summary>�񕜏�񃊃X�g���\������J����</summary>
    [System.Serializable]
    public class HealPowerColumn
    {
        [SerializeField, Tooltip("�e�[�u���J������ID\n������L�[�ɉ񕜏����Ƃ�")]
        short _ID = 0;

        [SerializeField, Tooltip("�ő�HP�ɑ΂���񕜗ʊ���(%)")]
        short _HealRatioByHP = 0;

        [SerializeField, Tooltip("�񕜂ɔ�₷����(s)")]
        byte _HealTime = 0;

        /// <summary>�e�[�u���J������ID ������L�[�ɉ񕜏����Ƃ�</summary>
        public short ID { get => _ID; }
        /// <summary>�ő�HP�ɑ΂���񕜗ʊ���(%)</summary>
        public short HealRatioByHP { get => _HealRatioByHP; }
        /// <summary>�񕜂ɔ�₷����(s)</summary>
        public byte HealTime { get => _HealTime; }
    }
}
