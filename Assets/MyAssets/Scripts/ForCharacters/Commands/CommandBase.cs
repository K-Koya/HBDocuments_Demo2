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
}
