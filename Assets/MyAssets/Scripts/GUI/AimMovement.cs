using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>
/// プレイヤー用の照準移動処理
/// </summary>
public class AimMovement : MonoBehaviour
{
    #region メンバ
    /// <summary>時間軸コンポーネント</summary>
    Timeline _Tl = null;

    /// <summary>メインカメラコンポーネント</summary>
    Camera _MainCamera = null;

    /// <summary>プレイヤーのパラメータ</summary>
    PlayerParameter _Param = null;

    /// <summary>照準を合わせている対象のパラメータ</summary>
    CharacterParameter _FocusedParam = null;


    /// <summary>照準までの距離の実数値</summary>
    float _Distance = 0.0f;

    /// <summary>照準までの距離の識別</summary>
    DistanceType _DistanceType = DistanceType.OutOfRange;
    #endregion


    #region プロパティ
    /// <summary>照準までの距離の実数値</summary>
    public float Distance { get => _Distance; }
    /// <summary>照準までの距離の識別</summary>
    public DistanceType DistType { get => _DistanceType; }
    /// <summary>照準を合わせている対象のパラメータ</summary>
    public CharacterParameter FocusedStatus { get => _FocusedParam; }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _Tl = GetComponent<Timeline>();
        _MainCamera = Camera.main;
        _Param = FindObjectOfType<PlayerParameter>();
        _FocusedParam = null;
    }


    void FixedUpdate()
    {
        //timeScaleが0ならポーズ中
        if (!(_Param.Tl.timeScale > 0f)) return;

        //地面レイ検索用クラス
        RaycastHit rayhitGround = default;
        //Rayの地面への接触点
        Vector3 rayhitPos = Vector3.zero;

        //プレイヤー位置からカメラ前方方向に地面を探索
        if (Physics.Raycast(_Param.EyePoint.transform.position, _MainCamera.transform.forward, out rayhitGround, _Param.LockMaxRange, LayerManager.Ins.OnTheReticle))
        {
            //確認できたら該当座標を保存
            rayhitPos = rayhitGround.point;

            //(所持していれば)対象のステータスコンポーネントを取得
            _FocusedParam = rayhitGround.transform.GetComponent<CharacterParameter>();
            
            //照準位置までの実数距離から識別値を設定
            if (_Distance < _Param.ComboProximityRange)
            {
                _DistanceType = DistanceType.WithinProximity;
            }
            else if (_Distance < _Param.LockMaxRange)
            {
                _DistanceType = DistanceType.OutOfProximity;
            }
        }
        else
        {
            //確認できなければ、最大射程距離を参照
            rayhitPos = _Param.EyePoint.transform.position + _MainCamera.transform.forward * _Param.LockMaxRange;
            //照準までの距離の識別値を射程外に
            _DistanceType = DistanceType.OutOfRange;
        }

        //照準を配置
        transform.position = rayhitPos;

        //照準位置までの距離を計算(各プレイヤーの最大射程距離を限界値とする)
        _Distance = Vector3.Distance(transform.position, _Param.EyePoint.transform.position);
    }
}

/// <summary>
/// 照準までの距離の識別値
/// </summary>
public enum DistanceType : byte
{
    /// <summary>
    /// 射程外
    /// </summary>
    OutOfRange,
    /// <summary>
    /// 近接攻撃範囲外
    /// </summary>
    OutOfProximity,
    /// <summary>
    /// 近接攻撃範囲内
    /// </summary>
    WithinProximity
}
