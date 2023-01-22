using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    /// <summary>攻撃を当てた相手の被弾コライダーに受け渡す攻撃情報</summary>
    AttackInformation _AttackInfo = null;

    /// <summary>攻撃を当てた相手の被弾コライダーに受け渡す攻撃情報</summary>
    public AttackInformation AttackInfo { set => _AttackInfo = value; }
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
    CharacterParameter _AttackerParam = null;


    /// <summary>同一攻撃からは連続でダメージを発生させないように割り振るID</summary>
    public byte AttackID { get => _AttackID; }
    /// <summary>攻撃情報リスト内のカラム</summary>
    public AttackPowerColumn Info { get => _Info; }
    /// <summary>攻撃者のパラメータ情報</summary>
    public CharacterParameter AttackerParam { get => _AttackerParam; }


    public AttackInformation(AttackPowerColumn　info, CharacterParameter param)
    {
        _AttackIDRecord = _AttackIDRecord == byte.MaxValue ? byte.MinValue : _AttackIDRecord++;
        _AttackID = _AttackIDRecord;
        _Info = info;
        _AttackerParam = param;
    }
}

