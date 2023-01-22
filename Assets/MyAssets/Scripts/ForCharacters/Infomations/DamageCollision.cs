using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollision : MonoBehaviour
{
    /// <summary>このキャラクターのパラメータ</summary>
    CharacterParameter _Param = null;

    void Start()
    {
        _Param = GetComponentInParent<CharacterParameter>();
    }

    void OnTriggerEnter(Collider other)
    {
        //接触した敵対するレイヤーのコライダーのうち、攻撃用コライダーに対して反応
        if (other.CompareTag(TagManager.Instance.AttackCollider))
        {
            AttackInformation newGot = null;
            if (TryGetComponent(out newGot))
            {
                //攻撃IDが異なればダメージの判定をかける
                if(newGot.AttackID != _Param.GaveAttack.AttackID)
                {
                    _Param.GaveAttack = newGot;

                    Debug.Log($"{_Param.name} がダメージをうけた！");
                }
            }
        }
    }


}

