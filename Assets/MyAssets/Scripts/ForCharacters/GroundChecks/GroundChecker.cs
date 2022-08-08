using System;
using UnityEngine;
using Chronos;

public class GroundChecker : MonoBehaviour
{
    #region メンバ
    [SerializeField, Tooltip("当該オブジェクトのカプセルコライダー")]
    CapsuleCollider _Collider = default;

    [SerializeField, Tooltip("True : 着地している")]
    bool _IsGround = false;

    [SerializeField, Tooltip("地面と壁の境界角度")]
    float _SlopeLimit = 45f;

    /// <summary>キャラクターの重力向き</summary>
    Vector3 _GravityDirection = Vector3.down;

    /// <summary>SphereCastおよびCapsuleCastする時の基準点となる座標1</summary>
    Vector3 _CastBasePosition = Vector3.zero;

    /// <summary>登れる坂とみなすための中心点からの距離</summary>
    float _SlopeAngleThreshold = 1f;
    #endregion

    #region プロパティ
    /// <summary>True : 着地している</summary>
    public bool IsGround { get => _IsGround; }

    /// <summary>キャラクターの重力向き</summary>
    public Vector3 GravityDirection { get => _GravityDirection; set => _GravityDirection = value; }
    #endregion

    void Start()
    {
        _Collider = GetComponent<CapsuleCollider>();
        _CastBasePosition = _Collider.center + Vector3.down * ((_Collider.height - _Collider.radius * 2f) / 2f);

        //円弧半径から弧長を求める公式
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
