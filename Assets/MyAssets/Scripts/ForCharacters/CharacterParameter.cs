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
    protected static List<CharacterParameter> _Allies = new List<CharacterParameter>();

    /// <summary>敵キャラクターを格納</summary>
    protected static List<CharacterParameter> _Enemies = new List<CharacterParameter>();
    #endregion

    #region メンバ
    [SerializeField, Tooltip("キャラクターの目線位置")]
    protected Transform _EyePoint = default;

    /// <summary>当該キャラクターが持つ時間軸コンポーネント</summary>
    protected Timeline _Tl = default;

    /// <summary>キャラクター正面方向情報</summary>
    protected Vector3 _CharacterDirection = default;

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
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
