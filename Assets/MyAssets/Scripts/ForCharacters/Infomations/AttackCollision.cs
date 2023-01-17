using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    /// <summary>�U���𓖂Ă�����̔�e�R���C�_�[�Ɏ󂯓n���U�����</summary>
    AttackInformation _AttackInfo = null;



    /// <summary>�U���𓖂Ă�����̔�e�R���C�_�[�Ɏ󂯓n���U�����</summary>
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

/// <summary>�U���R���C�_�[�����e�R���C�_�[�Ɏ󂯓n���U�����̍\����</summary>
public class AttackInformation
{
    /// <summary>����U������͘A���Ń_���[�W�𔭐������Ȃ��悤�Ɋ���U��ID</summary>
    public byte AttackID = 0;

    /// <summary>���ڍU���̈З͕␳�l</summary>
    public float damageRatio = 0f;

    /// <summary>�ԐڍU���̈З͕␳�l</summary>
    public float magicDamageRatio = 0f;

    /// <summary>�U���҂̃p�����[�^���</summary>
    public CharacterController attackerParam = null;
}

