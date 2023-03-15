using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageCollision : MonoBehaviour
{
    /// <summary>�U��-�h�� ��-1000�̎��̃_���[�W�l</summary>
    const short DAMAGE_ON_MIN_1000 = 10;

    /// <summary>�U��-�h�� ��1000�̎��̃_���[�W�l</summary>
    const short DAMAGE_ON_1000 = 90;

    /// <summary>���̃L�����N�^�[�̃p�����[�^</summary>
    CharacterParameter _Param = null;

    [SerializeField, Tooltip("���ʂɂ��_���[�W�␳")]
    float _DamageRatioOnPart = 1f;

    void Start()
    {
        _Param = GetComponentInParent<CharacterParameter>();
    }

    void OnTriggerEnter(Collider other)
    {
        //�ڐG�����G�΂��郌�C���[�̃R���C�_�[�̂����A�U���p�R���C�_�[�ɑ΂��Ĕ���
        if (other.CompareTag(TagManager.Instance.AttackCollider))
        {
            AttackCollision newGot;
            if (other.TryGetComponent(out newGot))
            {
                //�U��ID���قȂ�΃_���[�W�̔����������
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

                    //HP����
                    _Param.GaveDamage((int)(damage * _DamageRatioOnPart));

                    //�G�t�F�N�g����
                    newGot.CallHitEffect(other.ClosestPoint(transform.position));

                    Debug.Log($"{_Param.name} �� {damage} �_���[�W���������I");
                }
            }
        }
    }

    /// <summary>�_���[�W�v�Z�@ �U��-�h�� �̒l����_���[�W�l���o��</summary>
    /// <param name="atk_min_def">�U���҂̍U���l�����Q�҂̖h��l���������l</param>
    /// <param name="damageOnMin1000">�U��-�h�� ��-1000�̎��̃_���[�W�l</param>
    /// <param name="damageOn1000">�U��-�h�� ��1000�̎��̃_���[�W�l</param>
    /// <returns>�_���[�W�l�x�[�X</returns>
    int DamageCalculatorParamMin1000To1000(int atk_min_def, int damageOnMin1000, int damageOn1000)
    {
        //�X��
        float tilt = (damageOn1000 - damageOnMin1000) / 2000f;
        //�ؕ�
        float segment = damageOn1000 - (1000f * tilt);

        //�X���A�ؕЁA�U��-�h����_���[�W�l���Z�o
        return (int)((atk_min_def * tilt) + segment);
    }
}

