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

    /// <summary>キャラクターの行動状態</summary>
    protected MotionState _State = default;

    /// <summary>操作可否情報</summary>
    protected InputAcceptance _Can = default;

    /// <summary>ターゲットキャラクターにむける周回移動ルート情報</summary>
    protected OrbitalSystem _Orbit = default;
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
    /// <summary>仲間と集まる際にとる距離</summary>
    public float AliseGatherRange { get => _AliseGatherRange; }
    /// <summary>自分の近接攻撃の射程:遠距離</summary>
    public float AttackRangeFar { get => _AttackRangeFar; }
    /// <summary>自分の近接攻撃の射程:近距離</summary>
    public float AttackRangeMiddle { get => _AttackRangeMiddle; }
    /// <summary>敵への追跡を継続する判定距離</summary>
    public float ChaseEnemyDistance { get => _ChaseEnemyDistance; }
    /// <summary>キャラクターの行動状態</summary>
    public MotionState State { get => _State; set => _State = value; }
    /// <summary>操作可否情報</summary>
    public InputAcceptance Can { get => _Can; set => _Can = value; }
    /// <summary>ターゲットキャラクターにむける周回移動ルート情報</summary>
    public OrbitalSystem Orbit { get => _Orbit; }
    #endregion

    /// <summary>本クラスの静的メンバに自コンポーネントを登録させるメソッド</summary>
    abstract protected void RegisterStaticReference();

    /// <summary>本クラスの静的メンバから自コンポーネントを抹消するメソッド</summary>
    abstract protected void EraseStaticReference();

    void Awake()
    {
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
        _Orbit = GetComponentInChildren<OrbitalSystem>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    /*
    /// <summary>エンゲージメントを書き出し</summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRangeMiddle);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRangeFar);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ChaseEnemyDistance);
    }
    */
}
