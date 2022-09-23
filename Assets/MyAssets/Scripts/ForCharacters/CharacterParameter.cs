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

    #region メンバ
    [SerializeField, Tooltip("キャラクターの目線位置")]
    protected Transform _EyePoint = default;

    [SerializeField, Tooltip("仲間と集まる際にとる距離")]
    protected float _AliseGatherRange = 5f;

    [SerializeField, Tooltip("自分の近接攻撃の射程:遠距離")]
    protected float _AttackRangeFar = 7f;

    [SerializeField, Tooltip("自分の近接攻撃の射程:近距離")]
    protected float _AttackRangeMiddle = 5f;

    [SerializeField, Tooltip("敵への追跡を継続する判定距離")]
    protected float _ChaseEnemyDistance = 40f;

    /// <summary>当該キャラクターが持つ時間軸コンポーネント</summary>
    protected Timeline _Tl = default;

    /// <summary>キャラクター正面方向情報</summary>
    protected Vector3 _CharacterDirection = default;

    [SerializeField, Tooltip("攻撃を当てる対象のレイヤー")]
    protected LayerMask _HostilityLayer = default;

    /// <summary>キャラクターの当たり判定コライダー</summary>
    protected Collider _HitArea = null;

    [SerializeField, Tooltip("キャラクターの行動状態")]
    protected MotionState _State = default;

    [SerializeField, Tooltip("操作可否情報")]
    protected InputAcceptance _Can = default;

    [SerializeField, Tooltip("歩行最高速")]
    protected float _LimitSpeedWalk = 2f;

    [SerializeField, Tooltip("走行最高速")]
    protected float _LimitSpeedRun = 5f;

    /// <summary>攻撃範囲情報</summary>
    protected List<AttackArea> _AttackAreas = new List<AttackArea>(10);
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
    /// <summary>キャラクター正面方向情報</summary>
    public Vector3 Direction { get => _CharacterDirection; set => _CharacterDirection = value; }
    /// <summary>攻撃を当てる対象のレイヤー</summary>
    public LayerMask HostilityLayer { get => _HostilityLayer; }
    /// <summary>キャラクターの当たり判定コライダー</summary>
    public Collider HitArea { get => _HitArea; }
    /// <summary>仲間と集まる際にとる距離</summary>
    public float AliseGatherRange { get => _AliseGatherRange; }
    /// <summary>自分の近接攻撃の射程:遠距離</summary>
    public float AttackRangeFar { get => _AttackRangeFar; }
    /// <summary>自分の近接攻撃の射程:近距離</summary>
    public float AttackRangeMiddle { get => _AttackRangeMiddle; }
    /// <summary>敵への追跡を継続する判定距離</summary>
    public float ChaseEnemyDistance { get => _ChaseEnemyDistance; }
    /// <summary>キャラクターの行動状態</summary>
    public MotionState State { get => _State; }
    /// <summary>操作可否情報</summary>
    public InputAcceptance Can { get => _Can; }
    /// <summary>歩行最高速</summary>
    public float LimitSpeedWalk { get => _LimitSpeedWalk; }
    /// <summary>走行最高速</summary>
    public float LimitSpeedRun { get => _LimitSpeedRun; }
    /// <summary>攻撃範囲情報</summary>
    public List<AttackArea> AttackAreas => _AttackAreas;
    #endregion

    /// <summary>本クラスの静的メンバに自コンポーネントを登録させるメソッド</summary>
    abstract protected void RegisterStaticReference();

    /// <summary>本クラスの静的メンバから自コンポーネントを抹消するメソッド</summary>
    abstract protected void EraseStaticReference();

    void Awake()
    {
        _State = new MotionState();
        _Can = new InputAcceptance();

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
        
    }


    
    void OnDrawGizmos()
    {
        if(_AttackAreas != null && _AttackAreas.Count > 0)
        {
            Gizmos.color = Color.red;
            foreach(AttackArea at in _AttackAreas)
            {
                Gizmos.DrawWireSphere(at.CenterPos, at.Radius);
                if(at.GetType() == typeof(AttackAreaCapsule))
                {
                    Gizmos.DrawWireSphere((at as AttackAreaCapsule).CenterPos2, at.Radius);
                }
            }
        }

        /*
        //エンゲージメントを書き出し
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRangeMiddle);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRangeFar);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ChaseEnemyDistance);
        */
    }
}
