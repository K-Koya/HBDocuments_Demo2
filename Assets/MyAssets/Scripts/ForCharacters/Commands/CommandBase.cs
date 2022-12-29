using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommandBase
{
    [SerializeField, Tooltip("コマンド名")]
    protected string _Name = "";

    [SerializeField, Tooltip("コマンド説明文"), TextArea(2, 3)]
    protected string _Explain = "";

    /// <summary>コマンド名</summary>
    public string Name { get => _Name; }

    /// <summary>コマンド説明文</summary>
    public string Explain { get => _Explain; }
}
