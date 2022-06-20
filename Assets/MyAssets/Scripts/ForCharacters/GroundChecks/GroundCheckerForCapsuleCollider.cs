using UnityEngine;
using Chronos;

[RequireComponent(typeof(CapsuleCollider))]
public class GroundCheckerForCapsuleCollider : GroundChecker
{
    /// <summary>強制的に着地していない状態とするまでの時間</summary>
    const float _ENFOERCE_NOT_GROUNDED_TIME = 0.1f;

    /// <summary>地面に接していない時の計測時間</summary>
    float _NotGroundedTimer = 0f;

    [SerializeField, Tooltip("当該オブジェクトのコライダー")]
    CapsuleCollider _Collider = default;

    /// <summary>SphereCastおよびCapsuleCastする時の基準点となる座標1</summary>
    Vector3 _CastBasePosition1 = Vector3.zero;

    /// <summary>SphereCastおよびCapsuleCastする時の基準点となる座標2</summary>
    Vector3? _CastBasePosition2 = null;

    /// <summary>登れる坂とみなすための中心点からの距離</summary>
    float _SlopeAngleThreshold = 1f;

    /// <summary>当該キャラクターが持つ時間軸コンポーネント</summary>
    Timeline _Tl = default;



    // Start is called before the first frame update
    void Start()
    {
        _Tl = GetComponent<Timeline>();
        _Collider = GetComponent<CapsuleCollider>();

        float centerOffset = (_Collider.height - _Collider.radius * 2f) / 2f;

        //カプセルコライダーの向きによってCastをするときのパラメーターの取り方を変更
        switch (_Collider.direction)
        {
            //Y-Axis
            case 1:
                _CastBasePosition1 = _Collider.center + Vector3.down * centerOffset;
                SeekGround = SeekGroundForSphereCast;
                WithdrawalGround = WithdrawalForSphereCast;

                //円弧半径から弧長を求める公式
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

    /// <summary>SphereCastで地面を探すメソッド</summary>
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

    /// <summary>SphereCastでコライダー離脱が地面だったかを確かめるメソッド</summary>
    void WithdrawalForSphereCast()
    {
        
    }

    /// <summary>CapsuleCastで地面を探すメソッド</summary>
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
