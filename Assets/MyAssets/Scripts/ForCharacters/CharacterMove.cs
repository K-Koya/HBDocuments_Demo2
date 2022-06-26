using System;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

[RequireComponent(typeof(Rigidbody), typeof(Timeline))]
abstract public class CharacterMove : MonoBehaviour
{
    /// <summary>プレイヤーキャラクターを格納</summary>
    protected static CharacterMove _Player = null;

    /// <summary>味方キャラクターを格納</summary>
    protected static List<CharacterMove> _Allies = new List<CharacterMove>();

    /// <summary>敵キャラクターを格納</summary>
    protected static List<CharacterMove> _Enemies = new List<CharacterMove>();

    #region 定数
    /// <summary>速度が0であるとみなす数値</summary>
    protected const float VELOCITY_ZERO_BORDER = 0.5f;
    #endregion

    #region メンバ
    [SerializeField, Tooltip("キャラクターの目線位置")]
    protected Transform _EyePoint = default;

    /// <summary>当該キャラクターが持つ時間軸コンポーネント</summary>
    protected Timeline _Tl = default;

    /// <summary>キャラクター正面方向情報</summary>
    protected Vector3 _CharacterDirection = default;

    /// <summary>true : ジャンプ直後</summary>
    protected bool _JumpFlag = false;

    /// <summary>着地判定をするコンポーネント</summary>
    GroundChecker _GroundChecker = default;


    /// <summary>移動用メソッド</summary>
    protected Action Move = default;


    #endregion

    #region プロパティ
    /// <summary>キャラクターの目線位置</summary>
    public Transform EyePoint { get => _EyePoint; set => _EyePoint = value; }
    /// <summary>True : 着地している</summary>
    public virtual bool IsGround => _GroundChecker.IsGround;
    /// <summary>重力方向</summary>
    protected Vector3 GravityDirection => _GroundChecker.GravityDirection;
    /// <summary>移動速度</summary>
    abstract public float Speed { get; }
    /// <summary>ジャンプ直後フラグ</summary>
    public bool JumpFlag => _JumpFlag;
    /// <summary>プレイヤーキャラクターを格納</summary>
    public static CharacterMove Player => _Player;
    /// <summary>味方キャラクターを格納</summary>
    public static IReadOnlyList<CharacterMove> Allies => _Allies;
    /// <summary>敵キャラクターを格納</summary>
    public static IReadOnlyList<CharacterMove> Enemies => _Enemies;
    #endregion


    protected virtual void Awake()
    {

    }

    protected virtual void OnDestroy()
    {

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _Tl = GetComponent<Timeline>();
        _GroundChecker = GetComponent<GroundChecker>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //timeScaleが0ならポーズ中
        if (!(_Tl.timeScale > 0f)) return;

        Move();
    }
}
