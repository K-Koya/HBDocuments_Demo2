using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommandBase
{
    [SerializeField, Tooltip("コマンド名")]
    protected string _Name = "";

    [SerializeField, Tooltip("コマンド説明文"), TextArea(3, 5)]
    protected string _Explain = "";

    /// <summary>コマンド名</summary>
    public string Name { get => _Name; }

    /// <summary>コマンド説明文</summary>
    public string Explain { get => _Explain; }


    /// <summary>コマンド初期化処理</summary>
    public virtual void Initialize() { }

    /// <summary>何かしらオブジェクトを生成する処理</summary>
    /// <param name="index">種類</param>
    public virtual void ObjectCreation(int index) { }
}

/// <summary>攻撃情報リストを構成するカラム</summary>
[System.Serializable]
public class AttackPowerColumn
{
    [SerializeField, Tooltip("テーブルカラムのID\nこれをキーに攻撃情報をとる")]
    short _ID = 0;

    [SerializeField, Tooltip("直接攻撃の威力補正値(%)")]
    short _DamageRatio = 0;

    [SerializeField, Tooltip("間接攻撃の威力補正値(%)")]
    short _MagicDamageRatio = 0;
    
    /// <summary>テーブルカラムのID nこれをキーに攻撃情報をとる</summary>
    public short ID { get => _ID; }
    /// <summary>直接攻撃の威力補正値(%)</summary>
    public short DamageRatio { get => _DamageRatio; }
    /// <summary>間接攻撃の威力補正値(%)</summary>
    public short MagicDamageRatio { get => _MagicDamageRatio; }

    public AttackPowerColumn(short iD, short damageRatio, short magicDamageRatio)
    {
        _ID = iD;
        _DamageRatio = damageRatio;
        _MagicDamageRatio = magicDamageRatio;
    }
}

/// <summary>回復情報リストを構成するカラム</summary>
[System.Serializable]
public class HealPowerColumn
{
    [SerializeField, Tooltip("テーブルカラムのID\nこれをキーに回復情報をとる")]
    short _ID = 0;

    [SerializeField, Tooltip("最大HPに対する回復量割合(%)")]
    short _HealRatioByHP = 0;

    [SerializeField, Tooltip("回復に費やす時間(s)")]
    byte _HealTime = 0;

    /// <summary>テーブルカラムのID これをキーに回復情報をとる</summary>
    public short ID { get => _ID; }
    /// <summary>最大HPに対する回復量割合(%)</summary>
    public short HealRatioByHP { get => _HealRatioByHP; }
    /// <summary>回復に費やす時間(s)</summary>
    public byte HealTime { get => _HealTime; }

    public HealPowerColumn(short iD, short healRatioByHP, byte healTime)
    {
        _ID = iD;
        _HealRatioByHP = healRatioByHP;
        _HealTime = healTime;
    }
}
