using System;
using UnityEngine;
using Chronos;

public class GroundChecker : MonoBehaviour
{
    #region �����o
    [SerializeField, Tooltip("���Y�I�u�W�F�N�g�̃J�v�Z���R���C�_�[")]
    CapsuleCollider _Collider = default;

    [SerializeField, Tooltip("True : ���n���Ă���")]
    bool _IsGround = false;

    [SerializeField, Tooltip("�n�ʂƕǂ̋��E�p�x")]
    float _SlopeLimit = 45f;

    /// <summary>�L�����N�^�[�̏d�͌���</summary>
    Vector3 _GravityDirection = Vector3.down;

    /// <summary>SphereCast�����CapsuleCast���鎞�̊�_�ƂȂ���W1</summary>
    Vector3 _CastBasePosition = Vector3.zero;

    /// <summary>�o����Ƃ݂Ȃ����߂̒��S�_����̋���</summary>
    float _SlopeAngleThreshold = 1f;
    #endregion

    #region �v���p�e�B
    /// <summary>True : ���n���Ă���</summary>
    public bool IsGround { get => _IsGround; }

    /// <summary>�L�����N�^�[�̏d�͌���</summary>
    public Vector3 GravityDirection { get => _GravityDirection; set => _GravityDirection = value; }
    #endregion

    void Start()
    {
        _Collider = GetComponent<CapsuleCollider>();
        _CastBasePosition = _Collider.center + Vector3.down * ((_Collider.height - _Collider.radius * 2f) / 2f);

        //�~�ʔ��a����ʒ������߂����
        _SlopeAngleThreshold = 2f * _Collider.radius * Mathf.Sin(Mathf.Deg2Rad * _SlopeLimit / 2f);
    }

    void Update()
    {
        _IsGround = false;
        RaycastHit hit;
        if (Physics.SphereCast(_CastBasePosition + transform.position, _Collider.radius * 0.99f, _GravityDirection, out hit, _Collider.radius, LayerManager.Ins.AllGround))
        {
            if (Vector3.SqrMagnitude(transform.position - hit.point) < _SlopeAngleThreshold * _SlopeAngleThreshold)
            {
                _IsGround = true;
            }
        }
    }
}
