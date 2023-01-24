using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class AttackCollision : MonoBehaviour
{
    /// <summary>攻撃を当てた相手の被弾コライダーに受け渡す攻撃情報</summary>
    AttackInformation _AttackInfo = null;

    /// <summary>攻撃を当てた相手の被弾コライダーに受け渡す攻撃情報</summary>
    public AttackInformation AttackInfo { get => _AttackInfo; set => _AttackInfo = value; }
}

/// <summary>攻撃コライダーから被弾コライダーに受け渡す攻撃情報</summary>
public class AttackInformation
{
    /// <summary>最新の受け渡し攻撃情報の持つID</summary>
    static byte _AttackIDRecord = 0;

    /// <summary>同一攻撃からは連続でダメージを発生させないように割り振るID</summary>
    byte _AttackID = 0;

    /// <summary>攻撃情報リスト内のカラム</summary>
    AttackPowerColumn _Info = null;

    /// <summary>攻撃者のパラメータ情報</summary>
    MainParameter _AttackerParam = null;


    /// <summary>同一攻撃からは連続でダメージを発生させないように割り振るID</summary>
    public byte AttackID { get => _AttackID; }
    /// <summary>攻撃情報リスト内のカラム</summary>
    public AttackPowerColumn Info { get => _Info; }
    /// <summary>攻撃者のパラメータ情報</summary>
    public MainParameter AttackerParam { get => _AttackerParam; }


    public AttackInformation(AttackPowerColumn　info, MainParameter param)
    {
        _AttackIDRecord = _AttackIDRecord + 1 < byte.MaxValue ? (byte)(_AttackIDRecord + 1) : byte.MinValue;
        _AttackID = _AttackIDRecord;
        _Info = info;
        _AttackerParam = param;
    }
}

