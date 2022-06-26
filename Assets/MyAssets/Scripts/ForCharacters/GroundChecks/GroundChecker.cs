using System;
using UnityEngine;
using Chronos;

public class GroundChecker : MonoBehaviour
{
    #region メンバ
    /// <summary>True : 地面オブジェクトを見つけている</summary>
    protected bool _IsFindGroundObject = false;

    [SerializeField, Tooltip("True : 着地している")]
    protected bool _IsGround = false;

    [SerializeField, Tooltip("地面と壁の境界角度")]
    protected float _SlopeLimit = 45f;

    /// <summary>キャラクターの重力向き</summary>
    protected Vector3 _GravityDirection = Vector3.down;

    /// <summary>地面を探すためにCastをするメソッド</summary>
    protected Action SeekGround = default;

    /// <summary>コライダーから外れたオブジェクトが地面方向の接触だったかを判定するメソッド</summary>
    protected Action WithdrawalGround = default;
    #endregion

    #region プロパティ
    /// <summary>True : 着地している</summary>
    public bool IsGround { get => _IsGround; }

    /// <summary>キャラクターの重力向き</summary>
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
