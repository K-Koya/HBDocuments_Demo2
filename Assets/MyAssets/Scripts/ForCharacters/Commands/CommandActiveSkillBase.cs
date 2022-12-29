using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommandActiveSkillBase : CommandBase
{
    [SerializeField, Tooltip("アクティブスキルコマンドの順番")]
    protected byte _Priolity = 0;

    public CommandActiveSkillBase()
    {
        _Name = "EMPTY";
        _Explain = "スキルコマンドが未挿入です。";
    }

    /// <summary>アクティブスキルコマンドの順番</summary>
    public byte Priolity { get => _Priolity; }
}
