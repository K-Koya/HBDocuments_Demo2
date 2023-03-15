using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageCollision : MonoBehaviour
{
    /// <summary>攻撃-防御 が-1000の時のダメージ値</summary>
    const short DAMAGE_ON_MIN_1000 = 10;

    /// <summary>攻撃-防御 が1000の時のダメージ値</summary>
    const short DAMAGE_ON_1000 = 90;

    /// <summary>このキャラクターのパラメータ</summary>
    CharacterParameter _Param = null;

    [SerializeField, Tooltip("部位によるダメージ補正")]
    float _DamageRatioOnPart = 1f;

    void Start()
    {
        _Param = GetComponentInParent<CharacterParameter>();
    }

    void OnTriggerEnter(Collider other)
    {
        //接触した敵対するレイヤーのコライダーのうち、攻撃用コライダーに対して反応
        if (other.CompareTag(TagManager.Instance.AttackCollider))
        {
            AttackCollision newGot;
            if (other.TryGetComponent(out newGot))
            {
                //攻撃IDが異なればダメージの判定をかける
                if (newGot.AttackInfo != null && !_Param.GaveAttackIDs.Contains(newGot.AttackInfo.AttackID))
                {
                    _Param.GaveAttackIDs.Dequeue();
                    _Param.GaveAttackIDs.Enqueue(newGot.AttackInfo.AttackID);

                    MainParameter atkParam = newGot.AttackInfo.AttackerParam;
                    int atk_min_def = atkParam.Attack - _Param.Main.Defense;
                    int mag_min_sld = atkParam.Magic - _Param.Main.Shield;

                    int damage = 0;
                    AttackPowerColumn attackPower = newGot.AttackInfo.Info;
                    if (attackPower.DamageRatio > 0)
                    {
                        damage += DamageCalculatorParamMin1000To1000(atk_min_def, DAMAGE_ON_MIN_1000, DAMAGE_ON_1000) * attackPower.DamageRatio / 100;
                    }
                    if(attackPower.MagicDamageRatio > 0)
                    {
                        damage += DamageCalculatorParamMin1000To1000(mag_min_sld, DAMAGE_ON_MIN_1000, DAMAGE_ON_1000) * attackPower.MagicDamageRatio / 100;
                    }

                    //HP減少
                    _Param.GaveDamage((int)(damage * _DamageRatioOnPart));

                    //エフェクト発生
                    newGot.CallHitEffect(other.ClosestPoint(transform.position));

                    Debug.Log($"{_Param.name} が {damage} ダメージをうけた！");
                }
            }
        }
    }

    /// <summary>ダメージ計算機 攻撃-防御 の値からダメージ値を出す</summary>
    /// <param name="atk_min_def">攻撃者の攻撃値から被害者の防御値を引いた値</param>
    /// <param name="damageOnMin1000">攻撃-防御 が-1000の時のダメージ値</param>
    /// <param name="damageOn1000">攻撃-防御 が1000の時のダメージ値</param>
    /// <returns>ダメージ値ベース</returns>
    int DamageCalculatorParamMin1000To1000(int atk_min_def, int damageOnMin1000, int damageOn1000)
    {
        //傾き
        float tilt = (damageOn1000 - damageOnMin1000) / 2000f;
        //切片
        float segment = damageOn1000 - (1000f * tilt);

        //傾き、切片、攻撃-防御よりダメージ値を算出
        return (int)((atk_min_def * tilt) + segment);
    }
}

