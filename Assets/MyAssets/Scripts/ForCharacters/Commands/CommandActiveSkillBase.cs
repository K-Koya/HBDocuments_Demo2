using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommandActiveSkillBase : CommandBase
{
    [SerializeField, Tooltip("�A�N�e�B�u�X�L���R�}���h�̏���")]
    protected byte _Priolity = 0;

    public CommandActiveSkillBase()
    {
        _Name = "EMPTY";
        _Explain = "�X�L���R�}���h�����}���ł��B";
    }

    /// <summary>�A�N�e�B�u�X�L���R�}���h�̏���</summary>
    public byte Priolity { get => _Priolity; }
}
