using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBase
{
    [SerializeField, Tooltip("アクティブスキルコマンドの順番")]
    protected byte _Priolity = 0;

    /// <summary>アクティブスキルコマンドの順番</summary>
    public byte Priolity { get => _Priolity; }


    /// <summary>コマンドのID</summary>
    public virtual ushort Id { get => 0; }

    /// <summary>コマンド名</summary>
    public virtual string Name { get => "EMPTY"; }

    /// <summary>コマンド説明文</summary>
    public virtual string Explain { get => ""; }

    /// <summary>コマンドの消費MP</summary>
    public virtual byte MPCost { get => 0; }

    /// <summary>現在のアイテムの所持数</summary>
    public virtual byte CurrentInventory { get => 0; }

    /// <summary>最大のアイテムの所持数</summary>
    public virtual byte MaxInventory { get => 0; }

    /// <summary>コマンド種別</summary>
    public virtual CommandKind Kind { get => CommandKind.Blank; }


    /// <summary>攻撃情報テーブル</summary>
    protected virtual AttackPowerColumn[] AttackPowerTable { get => null; }

    
    /// <summary>コマンド保管庫から複製するメソッド</summary>
    /// <returns></returns>
    public CommandBase Clone()
    {
        return MemberwiseClone() as CommandBase;
    }

    /// <summary>攻撃情報を構成し攻撃範囲を取得する</summary>
    /// <param name="ID">攻撃情報リストに対応するID</param>
    /// <returns>攻撃情報</returns>
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


    /// <summary>コマンドデータテーブルを取得</summary>
    /// <returns>コマンドのID</returns>
    public virtual ushort LoadData() { return 0; }

    /// <summary>コマンド初期化処理</summary>
    /// <param name="layer">敵のレイヤか味方のレイヤか</param>
    public virtual void Initialize(int layer) { }

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

    /// <summary>コマンド追加入力</summary>
    /// <param name="param">該当キャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="gravityDirection">重力方向</param>
    /// <param name="reticleDirection">照準方向</param>
    /// <param name="animKind">要求するアニメーションの種類</param>
    public virtual void Additional(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
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
}

/// <summary>コマンドの種類</summary>
public enum CommandKind : byte
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
    /// <summary>パッシブコマンド</summary>
    Passive,
}

/// <summary>攻撃情報リストを構成するカラム</summary>
public class AttackPowerColumn
{
    /// <summary><para>テーブルカラムのID</para><para>これをキーに攻撃情報をとる</para></summary>
    short _ID = 0;

    /// <summary>直接攻撃の威力補正値(%)</summary>
    short _DamageRatio = 0;

    /// <summary>間接攻撃の威力補正値(%)</summary>
    short _MagicDamageRatio = 0;

    /// <summary><para>テーブルカラムのID</para><para>これをキーに攻撃情報をとる</para></summary>
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
public class HealPowerColumn
{
    /// <summary><para>テーブルカラムのID</para><para>これをキーに回復情報をとる</para></summary>
    short _ID = 0;

    /// <summary>最大HPに対する回復量割合(%)</summary>
    short _HealRatioByHP = 0;

    /// <summary>回復に費やす時間(s)</summary>
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


