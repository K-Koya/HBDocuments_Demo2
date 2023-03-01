using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>キャラクターの情報をまとめるコンポーネント</summary>
[RequireComponent(typeof(Timeline))]
abstract public class CharacterParameter : MonoBehaviour
{
    #region 定数
    /// <summary>受けた攻撃IDの履歴をとる個数</summary>
    public const byte NUMBER_OF_RECORD_GAVE_ATTACK_ID = 8;

    #endregion

    #region キャラ間のアクセッサ
    /// <summary>プレイヤーキャラクターを格納</summary>
    protected static CharacterParameter _Player = null;

    /// <summary>味方キャラクターを格納</summary>
    protected static List<CharacterParameter> _Allies = new List<CharacterParameter>(5);

    /// <summary>敵キャラクターを格納</summary>
    protected static List<CharacterParameter> _Enemies = new List<CharacterParameter>(12);
    #endregion

    #region メインパラメータ
    [SerializeField, Tooltip("キャラクター名：アルファベット表記（CSVファイルロードに利用）")]
    protected string _NameAlphabet = "name";

    [SerializeField, Tooltip("キャラクター名：カタカナ")]
    protected string _Name = "名前";

    [SerializeField, Tooltip("メインパラメータ")]
    protected MainParameter _Main = null;

    [SerializeField, Tooltip("現在のHP")]
    protected short _HPCurrent = 1000;

    [SerializeField, Tooltip("最大MP")]
    protected float _MPMaximum = 100f;

    [SerializeField, Tooltip("現在のMP")]
    protected float _MPCurrent = 100f;
    #endregion

    #region サブパラメータ
    [System.Serializable]
    public class SubParameter
    {
        [SerializeField, Tooltip("照準射程内")]
        float _LockMaxRange = 30f;

        [SerializeField, Tooltip("通常コンボ射程内")]
        float _ComboProximityRange = 5f;

        [SerializeField, Tooltip("歩行最高速")]
        float _LimitSpeedWalk = 2f;

        [SerializeField, Tooltip("走行最高速")]
        float _LimitSpeedRun = 5f;


        /// <summary>照準射程内</summary>
        public float LockMaxRange { get => _LockMaxRange; }
        /// <summary>通常コンボ射程内</summary>
        public float ComboProximityRange { get => _ComboProximityRange; }
        /// <summary>歩行最高速</summary>
        public float LimitSpeedWalk { get => _LimitSpeedWalk; }
        /// <summary>走行最高速</summary>
        public float LimitSpeedRun { get => _LimitSpeedRun; }
    }

    [SerializeField, Tooltip("サブパラメータ")]
    SubParameter _Sub = new SubParameter();
    #endregion

    #region メンバ
    [SerializeField, Tooltip("キャラクターの目線位置")]
    protected Transform _EyePoint = null;

    /// <summary>照準器の位置</summary>
    protected Vector3 _ReticlePoint = Vector3.zero;

    /// <summary>当該キャラクターが持つ時間軸コンポーネント</summary>
    protected Timeline _Tl = null;

    /// <summary>キャラクターの向き情報</summary>
    protected Vector3 _CharacterDirection = default;

    /// <summary>キャラクターの移動方向情報</summary>
    protected Vector3 _MoveDirection = default;

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


    [SerializeField, Tooltip("オブジェクトの射出位置情報")]
    protected Transform[] _EmitPoints = null;

    [System.Serializable]
    public class CollidersForOneWeapon
    {
        [SerializeField]
        public AttackCollision[] _AttackCollisions = null;
    }
    [SerializeField, Tooltip("攻撃範囲情報")]
    protected CollidersForOneWeapon[] _AttackAreas = null;


    /// <summary>受けた攻撃のID履歴</summary>
    protected Queue<byte> _GaveAttackIDs = new Queue<byte>(NUMBER_OF_RECORD_GAVE_ATTACK_ID);
    #endregion


    #region プロパティ
    /// <summary>プレイヤーキャラクターを格納</summary>
    public static CharacterParameter Player => _Player;
    /// <summary>味方キャラクターがリスト化されている</summary>
    public static IReadOnlyList<CharacterParameter> Allies => _Allies;
    /// <summary>敵キャラクターがリスト化されている</summary>
    public static IReadOnlyList<CharacterParameter> Enemies => _Enemies;

    /// <summary>キャラクター名：アルファベット表記</summary>
    public string NameAlphabet => _NameAlphabet;
    /// <summary>キャラクター名：カタカナ</summary>
    public string Name => _Name;
    /// <summary>メインパラメータ</summary>
    public MainParameter Main => _Main;
    /// <summary>最大のHP</summary>
    public virtual short HPMaximum => _Main.HPMaximum;
    /// <summary>現在のHP</summary>
    public virtual short HPCurrent => _HPCurrent;
    /// <summary>最大MP</summary>
    public virtual float MPMaximum => _MPMaximum;
    /// <summary>現在のMP</summary>
    public virtual float MPCurrent => _MPCurrent;
    /// <summary>サブパラメータ</summary>
    public SubParameter Sub => _Sub;

    /// <summary>キャラクターの目線位置</summary>
    public Transform EyePoint { get => _EyePoint; set => _EyePoint = value; }
    /// <summary>照準器の位置</summary>
    public Vector3 ReticlePoint { get => _ReticlePoint; set => _ReticlePoint = value; }
    /// <summary>本キャラクターの時間情報</summary>
    public Timeline Tl { get => _Tl; }
    /// <summary>キャラクターの向き情報</summary>
    public Vector3 CharacterDirection { get => _CharacterDirection; set => _CharacterDirection = value; }
    /// <summary>キャラクターの移動方向情報</summary>
    public Vector3 MoveDirection { get => _MoveDirection; set => _MoveDirection = value; }
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

    /// <summary>オブジェクトの射出位置情報</summary>
    public Transform[] EmitPoints => _EmitPoints;
    /// <summary>攻撃範囲情報</summary>
    public CollidersForOneWeapon[] AttackAreas => _AttackAreas;
    /// <summary>受けた攻撃のID履歴</summary>
    public Queue<byte> GaveAttackIDs => _GaveAttackIDs;
    #endregion

    /// <summary>本クラスの静的メンバに自コンポーネントを登録させるメソッド</summary>
    abstract protected void RegisterStaticReference();

    /// <summary>本クラスの静的メンバから自コンポーネントを抹消するメソッド</summary>
    abstract protected void EraseStaticReference();


    void Awake()
    {
        _Main = new MainParameter();
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

        //被攻撃履歴を初期化
        for(int i = 0; i < NUMBER_OF_RECORD_GAVE_ATTACK_ID; i++)
        {
            _GaveAttackIDs.Enqueue(byte.MaxValue);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        SetAcceptant();
    }

    /// <summary>ダメージ処理</summary>
    /// <param name="damage">被ダメージ値</param>
    public virtual void GaveDamage(int damage)
    {
        _HPCurrent -= (short)damage;
    }

    /// <summary>回復処理</summary>
    /// <param name="ratioOfHP">最大HPに対する回復量割合</param>
    public virtual void GaveHeal(float ratioOfHP)
    {
        short recover = (short)(ratioOfHP * _Main.HPMaximum);
        _HPCurrent += recover;
        _HPCurrent = _HPCurrent > _Main.HPMaximum ? _Main.HPMaximum : _HPCurrent;
        Debug.Log($"{_Name}に、HPを{recover}回復する効果");

        GameObject eff = EffectManager.Instance.HealEffects.Instansiate();
        eff.transform.position = _EyePoint.position;
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
                _Acceptance.Command = true;

                break;
            case MotionState.StateKind.Run:
                _Acceptance.Move = true;
                _Acceptance.Jump = true;
                _Acceptance.ShiftSlide = false;
                _Acceptance.LongTrip = true;
                _Acceptance.Gurad = true;
                _Acceptance.ComboNormal = true;
                _Acceptance.ComboFinish = false;
                _Acceptance.Command = true;

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
                _Acceptance.Command = true;

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
                        _Acceptance.Command = false;
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
                        _Acceptance.Command = true;
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
                        _Acceptance.Command = false;
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
                        _Acceptance.Command = true;
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
                _Acceptance.Command = false;

                break;
            case MotionState.StateKind.ComboNormal:

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
                        _Acceptance.Command = false;

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
                        _Acceptance.Command = true;

                        break;
                    default:
                        _Acceptance.Move = false;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = false;
                        _Acceptance.ComboFinish = false;
                        _Acceptance.Command = false;

                        break;
                }

                break;
            case MotionState.StateKind.AttackCommand:
            case MotionState.StateKind.HealCommand:
            case MotionState.StateKind.SupportCommand:
                switch (_State.Process)
                {
                    case MotionState.ProcessKind.Interval:
                        _Acceptance.Move = false;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = false;
                        _Acceptance.ComboFinish = false;
                        _Acceptance.Command = true;

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
                        _Acceptance.Command = true;

                        break;
                    default:
                        _Acceptance.Move = false;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = false;
                        _Acceptance.ComboFinish = false;
                        _Acceptance.Command = false;

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
                        _Acceptance.Command = false;
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
                        _Acceptance.Command = true;
                        break;
                }
                break;

            default: break;
        }
    }
}

[System.Serializable]
public class MainParameter
{
    [SerializeField, Tooltip("最大HP")]
    short _HPMaximum = 1000;

    [SerializeField, Tooltip("直接攻撃")]
    short _Attack = 1000;

    [SerializeField, Tooltip("直接防御")]
    short _Defense = 1000;

    [SerializeField, Tooltip("間接攻撃")]
    short _Magic = 1000;

    [SerializeField, Tooltip("間接防御")]
    short _Shield = 1000;

    [SerializeField, Tooltip("敏捷性")]
    short _Rapid = 1000;

    [SerializeField, Tooltip("技術力")]
    short _Technique = 1000;

    [SerializeField, Tooltip("調子")]
    short _Luck = 1000;

    /// <summary>最大HP</summary>
    public short HPMaximum { get => _HPMaximum; }
    /// <summary>直接攻撃</summary>
    public short Attack { get => _Attack; }
    /// <summary>直接防御</summary>
    public short Defense { get => _Defense; }
    /// <summary>間接攻撃</summary>
    public short Magic { get => _Magic; }
    /// <summary>間接防御</summary>
    public short Shield { get => _Shield; }
    /// <summary>敏捷性</summary>
    public short Rapid { get => _Rapid; }
    /// <summary>技術力</summary>
    public short Technique { get => _Technique; }
    /// <summary>調子</summary>
    public short Luck { get => _Luck; }
}