using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollision : MonoBehaviour
{
    /// <summary>���̃L�����N�^�[�̃p�����[�^</summary>
    CharacterParameter _Param = null;

    void Start()
    {
        _Param = GetComponentInParent<CharacterParameter>();
    }

    void OnTriggerEnter(Collider other)
    {
        //�ڐG�����G�΂��郌�C���[�̃R���C�_�[�̂����A�U���p�R���C�_�[�ɑ΂��Ĕ���
        if (other.CompareTag(TagManager.Instance.AttackCollider))
        {
            AttackInformation newGot = null;
            if (TryGetComponent(out newGot))
            {
                //�U��ID���قȂ�΃_���[�W�̔����������
                if(newGot.AttackID != _Param.GaveAttack.AttackID)
                {
                    _Param.GaveAttack = newGot;

                    Debug.Log($"{_Param.name} ���_���[�W���������I");
                }
            }
        }
    }


}

