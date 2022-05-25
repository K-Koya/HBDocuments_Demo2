using System;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    #region �����o
    [SerializeField, Tooltip("True : ���n���Ă���")]
    protected bool _IsGround = false;

    [SerializeField, Tooltip("�n�ʂƔ��ʂ���I�u�W�F�N�g�̃��C���[��")]
    protected LayerMask _GroundLayer = default;

    [SerializeField, Tooltip("�n�ʂƕǂ̋��E�p�x")]
    protected float _SlopeLimit = 45f;

    /// <summary>�L�����N�^�[�̏d�͌���</summary>
    protected Vector3 _GravityDirection = Vector3.down;

    /// <summary>�n�ʂ�T�����߂�Cast�����郁�\�b�h</summary>
    protected Action SeekGround = default;

    /// <summary>�R���C�_�[����O�ꂽ�I�u�W�F�N�g���n�ʕ����̐ڐG���������𔻒肷�郁�\�b�h</summary>
    protected Action WithdrawalGround = default;
    #endregion

    #region �v���p�e�B
    /// <summary>True : ���n���Ă���</summary>
    public bool IsGround { get => _IsGround; }

    /// <summary>�L�����N�^�[�̏d�͌���</summary>
    public Vector3 GravityDirection { get => _GravityDirection; set => _GravityDirection = value; }
    #endregion


    protected void OnCollisionStay(Collision collision)
    {
        SeekGround();
    }

    protected void OnCollisionExit(Collision collision)
    {
        WithdrawalGround();
    }
}