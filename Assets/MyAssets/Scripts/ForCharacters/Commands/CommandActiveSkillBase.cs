using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommandActiveSkillBase : CommandBase
{
    #region メンバ
    [SerializeField, Tooltip("アクティブスキルコマンドの順番")]
    protected byte _Priolity = 0;

    [SerializeField, Tooltip("コマンドの数値各種\nスキル:消費MPコスト\nアイテム:個数\nCombo:コンボ手数")]
    protected byte _Count = 1;

    [SerializeField, Tooltip("攻撃情報テーブル")]
    protected AttackPowerColumn[] _AttackPowerTable = null;

    [SerializeField, Tooltip("true : コマンド施工中")]
    protected bool _IsRunning = false;

    public enum CommandKind
    {
        /// <summary>未挿入</summary>
        Blank = 0,
        /// <summary>回復以外のサポートコマンド</summary>
        Support, 
        /// <summary>通常コンボコマンド</summary>
        Combo,
        /// <summary>アタックコマンド</summary>
        Attack,
        /// <summary>回復以外のアイテムコマンド</summary>
        Item,
        /// <summary>回復サポートコマンド</summary>
        SupportHeal,
        /// <summary>回復アイテムコマンド</summary>
        ItemHeal,
    }
    /// <summary>コマンドの種類</summary>
    protected CommandKind _Kind = CommandKind.Blank;

    /// <summary>コマンドの種類</summary>
    public CommandKind Kind { get => _Kind; }

    #endregion

    public CommandActiveSkillBase()
    {
        _Name = "EMPTY";
        _Explain = "スキルコマンドが未挿入です。";

        _Kind = CommandKind.Blank;
    }

    #region プロパティ
    /// <summary>アクティブスキルコマンドの順番</summary>
    public byte Priolity { get => _Priolity; }

    /// <summary>
    /// <para>コマンドの数値各種</para>
    /// <para>Skill:消費MPコスト</para>
    /// <para>Item:個数</para>
    /// <para>Combo:コンボ手数</para>
    /// </summary>
    public byte Count { get => _Count; }

    /// <summary>true : コマンド施工中</summary>
    public bool IsRunning { get => _IsRunning; }
    #endregion

    /// <summary>コマンド開始</summary>
    /// <param name="param">該当キャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="gravityDirection">重力方向</param>
    /// <param name="reticleDirection">照準方向</param>
    /// <param name="animKind">要求するアニメーションの種類</param>
    public virtual void DoRun(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {

    }

    /// <summary>コマンド実行中</summary>
    /// <param name="param">該当キャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="gravityDirection">重力方向</param>
    /// <param name="reticleDirection">照準方向</param>
    /// <param name="animKind">要求するアニメーションの種類</param>
    public virtual void Running(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {

    }

    /// <summary>コマンド終了時</summary>
    /// <param name="param">該当キャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="gravityDirection">重力方向</param>
    /// <param name="animKind">要求するアニメーションの種類</param>
    public virtual void PostProcess(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, ref AnimationKind animKind)
    {

    }

    /// <summary>何かしらオブジェクトを生成する処理</summary>
    /// <param name="param">自キャラクターのメインパラメータ</param>
    /// <param name="info">攻撃情報</param>
    /// <param name="emitPoint">射出座標</param>
    public virtual void ObjectCreation(CharacterParameter param, AttackInformation info, Vector3 emitPoint)
    {

    }

    /// <summary>攻撃情報を構成し攻撃範囲を取得する</summary>
    /// <param name="ID">攻撃情報リストに対応するID</param>
    /// <returns>攻撃情報</returns>
    public AttackPowerColumn GetAttackArea(int ID)
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
