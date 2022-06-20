using UnityEngine;
using Chronos;

[RequireComponent(typeof(CapsuleCollider))]
public class GroundCheckerForCapsuleCollider : GroundChecker
{
    /// <summary>�����I�ɒ��n���Ă��Ȃ���ԂƂ���܂ł̎���</summary>
    const float _ENFOERCE_NOT_GROUNDED_TIME = 0.1f;

    /// <summary>�n�ʂɐڂ��Ă��Ȃ����̌v������</summary>
    float _NotGroundedTimer = 0f;

    [SerializeField, Tooltip("���Y�I�u�W�F�N�g�̃R���C�_�[")]
    CapsuleCollider _Collider = default;

    /// <summary>SphereCast�����CapsuleCast���鎞�̊�_�ƂȂ���W1</summary>
    Vector3 _CastBasePosition1 = Vector3.zero;

    /// <summary>SphereCast�����CapsuleCast���鎞�̊�_�ƂȂ���W2</summary>
    Vector3? _CastBasePosition2 = null;

    /// <summary>�o����Ƃ݂Ȃ����߂̒��S�_����̋���</summary>
    float _SlopeAngleThreshold = 1f;

    /// <summary>���Y�L�����N�^�[�������Ԏ��R���|�[�l���g</summary>
    Timeline _Tl = default;



    // Start is called before the first frame update
    void Start()
    {
        _Tl = GetComponent<Timeline>();
        _Collider = GetComponent<CapsuleCollider>();

        float centerOffset = (_Collider.height - _Collider.radius * 2f) / 2f;

        //�J�v�Z���R���C�_�[�̌����ɂ����Cast������Ƃ��̃p�����[�^�[�̎�����ύX
        switch (_Collider.direction)
        {
            //Y-Axis
            case 1:
                _CastBasePosition1 = _Collider.center + Vector3.down * centerOffset;
                SeekGround = SeekGroundForSphereCast;
                WithdrawalGround = WithdrawalForSphereCast;

                //�~�ʔ��a����ʒ������߂����
                _SlopeAngleThreshold = 2f * _Collider.radius * Mathf.Sin(Mathf.Deg2Rad * _SlopeLimit / 2f);

                break;

            //X-Axis
            case 0:
                _CastBasePosition1 = _Collider.center + Vector3.right * centerOffset;
                _CastBasePosition2 = _Collider.center + Vector3.left * centerOffset;
                SeekGround = SeekGroundForCapsuleCast;
                WithdrawalGround = SeekGroundForCapsuleCast;

                break;

            //Z-Axis
            case 2:
                _CastBasePosition1 = _Collider.center + Vector3.forward * centerOffset;
                _CastBasePosition2 = _Collider.center + Vector3.back * centerOffset;
                SeekGround = SeekGroundForCapsuleCast;
                WithdrawalGround = SeekGroundForCapsuleCast;

                break;
            default: break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_IsFindGroundObject)
        {
            _NotGroundedTimer += _Tl.fixedDeltaTime;
            if(_NotGroundedTimer > _ENFOERCE_NOT_GROUNDED_TIME)
            {
                _IsGround = false;
                _NotGroundedTimer = 0f;
            }
        }
        else _NotGroundedTimer = 0f;

        _IsFindGroundObject = false;
    }

    /// <summary>SphereCast�Œn�ʂ�T�����\�b�h</summary>
    /// 
    void SeekGroundForSphereCast()
    {
        RaycastHit hit;
        if (Physics.SphereCast(_CastBasePosition1 + transform.position, _Collider.radius * 0.99f, _GravityDirection, out hit, _Collider.radius, _GroundLayer))
        {
            if (Vector3.SqrMagnitude(transform.position - hit.point) < _SlopeAngleThreshold * _SlopeAngleThreshold)
            {
                _IsGround = true;
                _IsFindGroundObject = true;
            }
        }
    }

    /// <summary>SphereCast�ŃR���C�_�[���E���n�ʂ����������m���߂郁�\�b�h</summary>
    void WithdrawalForSphereCast()
    {
        
    }

    /// <summary>CapsuleCast�Œn�ʂ�T�����\�b�h</summary>
    void SeekGroundForCapsuleCast()
    {
        RaycastHit hit;
        if (Physics.CapsuleCast(_CastBasePosition1, (Vector3)_CastBasePosition2, _Collider.radius * 0.9f, _GravityDirection, out hit, _Collider.radius * 1.1f, _GroundLayer))
        {
            if (Vector3.Angle(-_GravityDirection, hit.normal) > (90f - _SlopeLimit))
            {
                _IsGround = true;
                _IsFindGroundObject = true;
            }
        }
    }
}
