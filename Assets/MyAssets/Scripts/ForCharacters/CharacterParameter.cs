using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>キャラクターの情報をまとめるコンポーネント</summary>
[RequireComponent(typeof(Timeline))]
abstract public class CharacterParameter : MonoBehaviour
{
    #region キャラ間のアクセッサ
    /// <summary>プレイヤーキャラクターを格納</summary>
    protected static CharacterParameter _Player = null;

    /// <summary>味方キャラクターを格納</summary>
    protected static List<CharacterParameter> _Allies = new List<CharacterParameter>(5);

    /// <summary>敵キャラクターを格納</summary>
    protected static List<CharacterParameter> _Enemies = new List<CharacterParameter>(12);
    #endregion

    #region メインパラメータ
    [SerializeField, Tooltip("最大HP")]
    protected short _HPMaximum = 1000;

    [SerializeField, Tooltip("現在のHP")]
    protected short _HPCurrent = 1000;

    [SerializeField, Tooltip("最大MP")]
    protected float _MPMaximum = 10f;

    [SerializeField, Tooltip("現在のMP")]
    protected float _MPCurrent = 10f;


    /// <summary>最大HP</summary>
    public short HPMaximum { get => _HPMaximum; }
    /// <summary>現在のHP</summary>
    public short HPCurrent { get => _HPCurrent; }
    /// <summary>最大MP</summary>
    public float MPMaximum { get => _MPMaximum; }
    /// <summary>現在のMP</summary>
    public float MPCurrent { get => _MPCurrent; }

    #endregion

    #region サブパラメータ
    [SerializeField, Tooltip("照準射程内")]
    protected float _LockMaxRange = 30f;

    [SerializeField, Tooltip("通常コンボ射程内")]
    protected float _ComboProximityRange = 5f;

    [SerializeField, Tooltip("歩行最高速")]
    protected float _LimitSpeedWalk = 2f;

    [SerializeField, Tooltip("走行最高速")]
    protected float _LimitSpeedRun = 5f;


    /// <summary>照準射程内</summary>
    public float LockMaxRange { get => _LockMaxRange; }
    /// <summary>通常コンボ射程内</summary>
    public float ComboProximityRange { get => _ComboProximityRange; }
    /// <summary>歩行最高速</summary>
    public float LimitSpeedWalk { get => _LimitSpeedWalk; }
    /// <summary>走行最高速</summary>
    public float LimitSpeedRun { get => _LimitSpeedRun; }

    #endregion

    #region メンバ
    [SerializeField, Tooltip("キャラクターの目線位置")]
    protected Transform _EyePoint = null;

    /// <summary>当該キャラクターが持つ時間軸コンポーネント</summary>
    protected Timeline _Tl = null;

    /// <summary>true : キャラクターの向きと移動方向を同期する</summary>
    protected bool _IsSyncDirection = true;

    /// <summary>キャラクター正面方向情報</summary>
    protected Vector3 _Direction = default;

    [SerializeField, Tooltip("攻撃を受ける対象のレイヤー")]
    protected LayerMask _HostilityLayer = default;

    /// <summary>注視している相手のパラメータ</summary>
    protected CharacterParameter _GazeAt = null;

    /// <summary>キャラクターの当たり判定コライダー</summary>
    protected Collider _HitArea = null;

    [SerializeField, Tooltip("キャラクターの行動状態")]
    protected MotionState _State = null;

    [SerializeField, Tooltip("操作可否情報")]
    protected InputAcceptance _Acceptance = null;

    

    /// <summary>攻撃範囲情報</summary>
    protected List<AttackArea> _AttackAreas = new List<AttackArea>(10);

    /// <summary>最も最近受けた攻撃の情報</summary>
    protected AttackInformation _GaveAttack = null;
    #endregion


    #region プロパティ
    /// <summary>プレイヤーキャラクターを格納</summary>
    public static CharacterParameter Player => _Player;
    /// <summary>味方キャラクターがリスト化されている</summary>
    public static IReadOnlyList<CharacterParameter> Allies => _Allies;
    /// <summary>敵キャラクターがリスト化されている</summary>
    public static IReadOnlyList<CharacterParameter> Enemies => _Enemies;
    /// <summary>キャラクターの目線位置</summary>
    public Transform EyePoint { get => _EyePoint; set => _EyePoint = value; }
    /// <summary>本キャラクターの時間情報</summary>
    public Timeline Tl { get => _Tl; }
    /// <summary>true : キャラクターの向きと移動方向を同期する</summary>
    public bool IsSyncDirection { get => _IsSyncDirection; set => _IsSyncDirection = value; }
    /// <summary>キャラクター正面方向情報</summary>
    public Vector3 Direction { get => _Direction; set => _Direction = value; }
    /// <summary>攻撃を受ける対象のレイヤー</summary>
    public LayerMask HostilityLayer { get => _HostilityLayer; }
    /// <summary>注視している相手のパラメータ</summary>
    public CharacterParameter GazeAt { get => _GazeAt; set => _GazeAt = value; }
    /// <summary>キャラクターの当たり判定コライダー</summary>
    public Collider HitArea { get => _HitArea; }
    /// <summary>キャラクターの行動状態</summary>
    public MotionState State { get => _State; }
    /// <summary>操作可否情報</summary>
    public InputAcceptance Can { get => _Acceptance; }
    /// <summary>攻撃範囲情報</summary>
    public List<AttackArea> AttackAreas => _AttackAreas;
    /// <summary>最も最近受けた攻撃の情報</summary>
    public AttackInformation GaveAttack { get => _GaveAttack; set => _GaveAttack = value; }
    #endregion

    /// <summary>本クラスの静的メンバに自コンポーネントを登録させるメソッド</summary>
    abstract protected void RegisterStaticReference();

    /// <summary>本クラスの静的メンバから自コンポーネントを抹消するメソッド</summary>
    abstract protected void EraseStaticReference();

    void Awake()
    {
        _State = new MotionState();
        _Acceptance = new InputAcceptance();

        RegisterStaticReference();
    }

    void OnDestroy()
    {
        EraseStaticReference();
    }

    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _Tl = GetComponent<Timeline>();
        _HitArea = GetComponent<Collider>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        SetAcceptant();
    }

    /// <summary>操作可否情報を様々なステート状態から設定する</summary>
    void SetAcceptant()
    {
        switch (_State.Kind)
        {
            case MotionState.StateKind.Stay:
            case MotionState.StateKind.Walk:
                _Acceptance.Move = true;
                _Acceptance.Jump = true;
                _Acceptance.ShiftSlide = true;
                _Acceptance.LongTrip = true;
                _Acceptance.Gurad = true;
                _Acceptance.ComboNormal = true;
                _Acceptance.ComboFinish = false;
                _IsSyncDirection = true;

                break;
            case MotionState.StateKind.Run:
                _Acceptance.Move = true;
                _Acceptance.Jump = true;
                _Acceptance.ShiftSlide = false;
                _Acceptance.LongTrip = true;
                _Acceptance.Gurad = true;
                _Acceptance.ComboNormal = true;
                _Acceptance.ComboFinish = false;
                _IsSyncDirection = true;

                break;
            case MotionState.StateKind.JumpNoraml:
            case MotionState.StateKind.FallNoraml:
                _Acceptance.Move = true;
                _Acceptance.Jump = false;
                _Acceptance.ShiftSlide = false;
                _Acceptance.LongTrip = false;
                _Acceptance.Gurad = false;
                _Acceptance.ComboNormal = true;
                _Acceptance.ComboFinish = false;
                _IsSyncDirection = true;

                break;
            case MotionState.StateKind.ShiftSlide:
                switch (_State.Process)
                {
                    case MotionState.ProcessKind.Preparation:
                    case MotionState.ProcessKind.Playing:
                        _Acceptance.Move = false;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = false;
                        _Acceptance.ComboFinish = false;
                        _IsSyncDirection = false;
                        break;

                    case MotionState.ProcessKind.EndSoon:
                        _State.Kind = MotionState.StateKind.Stay;
                        _State.Process = MotionState.ProcessKind.Playing;
                        _Acceptance.Move = true;
                        _Acceptance.Jump = true;
                        _Acceptance.ShiftSlide = true;
                        _Acceptance.LongTrip = true;
                        _Acceptance.Gurad = true;
                        _Acceptance.ComboNormal = true;
                        _Acceptance.ComboFinish = false;
                        _IsSyncDirection = true;
                        break;

                }

                break;
            case MotionState.StateKind.LongTrip:
                switch (_State.Process)
                {
                    case MotionState.ProcessKind.Preparation:
                    case MotionState.ProcessKind.Playing:
                        _Acceptance.Move = false;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = false;
                        _Acceptance.ComboFinish = false;
                        _IsSyncDirection = false;
                        break;

                    case MotionState.ProcessKind.EndSoon:
                        _State.Kind = MotionState.StateKind.FallNoraml;
                        _State.Process = MotionState.ProcessKind.Playing;
                        _Acceptance.Move = true;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = true;
                        _Acceptance.ComboFinish = false;
                        _IsSyncDirection = true;
                        break;
                }

                break;
            case MotionState.StateKind.Guard:
                _Acceptance.Move = false;
                _Acceptance.Jump = false;
                _Acceptance.ShiftSlide = false;
                _Acceptance.LongTrip = false;
                _Acceptance.Gurad = true;
                _Acceptance.ComboNormal = false;
                _Acceptance.ComboFinish = false;

                break;
            case MotionState.StateKind.ComboNormal:

                _IsSyncDirection = true;
                switch (_State.Process)
                {
                    case MotionState.ProcessKind.Interval:
                        _Acceptance.Move = false;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = true;
                        _Acceptance.ComboFinish = true;

                        break;
                    case MotionState.ProcessKind.EndSoon:
                        _State.Kind = MotionState.StateKind.Stay;
                        _State.Process = MotionState.ProcessKind.Playing;
                        _Acceptance.Move = true;
                        _Acceptance.Jump = true;
                        _Acceptance.ShiftSlide = true;
                        _Acceptance.LongTrip = true;
                        _Acceptance.Gurad = true;
                        _Acceptance.ComboNormal = true;
                        _Acceptance.ComboFinish = false;

                        break;
                    default:
                        _Acceptance.Move = false;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = false;
                        _Acceptance.ComboFinish = false;

                        break;
                }
                break;
            case MotionState.StateKind.Hurt:
                switch (_State.Process)
                {
                    case MotionState.ProcessKind.Preparation:
                    case MotionState.ProcessKind.Playing:
                        _Acceptance.Move = false;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = false;
                        _Acceptance.ComboFinish = false;
                        _IsSyncDirection = false;
                        break;

                    case MotionState.ProcessKind.EndSoon:
                        _State.Kind = MotionState.StateKind.FallNoraml;
                        _State.Process = MotionState.ProcessKind.Playing;
                        _Acceptance.Move = true;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = true;
                        _Acceptance.ComboFinish = false;
                        _IsSyncDirection = true;
                        break;
                }
                break;

            default: break;
        }
    }
}
