using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    /// <summary>攻撃を当てた相手の被弾コライダーに受け渡す攻撃情報</summary>
    AttackInformation _AttackInfo = null;



    /// <summary>攻撃を当てた相手の被弾コライダーに受け渡す攻撃情報</summary>
    public AttackInformation AttackInfo { set => _AttackInfo = value; }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/// <summary>攻撃コライダーから被弾コライダーに受け渡す攻撃情報の構造体</summary>
public class AttackInformation
{
    /// <summary>同一攻撃からは連続でダメージを発生させないように割り振るID</summary>
    public byte AttackID = 0;

    /// <summary>直接攻撃の威力補正値</summary>
    public float damageRatio = 0f;

    /// <summary>間接攻撃の威力補正値</summary>
    public float magicDamageRatio = 0f;

    /// <summary>攻撃者のパラメータ情報</summary>
    public CharacterController attackerParam = null;
}

