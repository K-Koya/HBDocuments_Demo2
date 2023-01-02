using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommandActiveSkillBase : CommandBase
{
    #region メンバ
    [SerializeField, Tooltip("アクティブスキルコマンドの順番")]
    protected byte _Priolity = 0;

    [SerializeField, Tooltip("コマンドの数値各種\nスキル:消費MPコスト\nアイテム:個数")]
    protected byte _Count = 1;

    [SerializeField, Tooltip("true : コマンド施工中")]
    protected bool _IsRunning = false;
    #endregion

    public CommandActiveSkillBase()
    {
        _Name = "EMPTY";
        _Explain = "スキルコマンドが未挿入です。";
    }

    #region プロパティ
    /// <summary>アクティブスキルコマンドの順番</summary>
    public byte Priolity { get => _Priolity; }

    /// <summary>
    /// <para>コマンドの数値各種</para>
    /// <para>Skill:消費MPコスト</para>
    /// <para>Item:個数</para>
    /// </summary>
    public byte Count { get => _Count; }

    /// <summary>true : コマンド施工中</summary>
    public bool IsRunning { get => _IsRunning; }
    #endregion

    /// <summary>コマンド開始</summary>
    /// <param name="param">該当のキャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="gravityDirection">重力方向</param>
    public virtual void DoRun(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection)
    {

    }

    /// <summary>コマンド実行中</summary>
    /// <param name="param">該当のキャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="gravityDirection">重力方向</param>
    public virtual void Running(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection)
    {

    }
}
